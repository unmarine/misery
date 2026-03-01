using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using misery.Eng;
using System.ComponentModel;

namespace misery.Components;

public enum InteractiveGridMode
{
    DrawCells,
    SetStart,
    SetEnd
}

public sealed class InteractiveGrid : Panel
{
    private Automaton _automaton;
    private Bitmap _canvas;
    private byte[] _rgbaValues;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public InteractiveGridMode CurrentMode { get; set; }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Coordinate StartPoint { get; set; }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Coordinate EndPoint { get; set; }
    
    public InteractiveGrid(Automaton initial)
    {
        _automaton = initial;
        CurrentMode = InteractiveGridMode.DrawCells;
        DoubleBuffered = true;
        _canvas = new Bitmap(_automaton.Columns, _automaton.Rows, PixelFormat.Format32bppPArgb);
        _rgbaValues = new byte[_canvas.Width * _canvas.Height * 4];
        _automaton.GridUpdated += Invalidate;
        MouseMove += OnMouse;
        MouseClick += OnMouse;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        UpdateBitmap();
        e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
        e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
        e.Graphics.DrawImage(_canvas, 0, 0, Width, Height);
    }

    private void UpdateBitmap()
    {
        Grid grid = _automaton.GetReadyGrid();
        BitmapData data = _canvas.LockBits(new Rectangle(0, 0, _canvas.Width, _canvas.Height), 
                                         ImageLockMode.WriteOnly, _canvas.PixelFormat);

        for (int row = 0; row < _automaton.Rows; row++)
        {
            for (int column = 0; column < _automaton.Columns; column++)
            {
                State state = grid.ReadState(row, column);
                Color color = Settings.GetColorByState(state);

                
                int index = (row * data.Stride) + (column * 4);

                _rgbaValues[index] = color.B;
                _rgbaValues[index + 1] = color.G;
                _rgbaValues[index + 2] = color.R;
                _rgbaValues[index + 3] = color.A;
            }
        }

        if (_automaton.Path.Count > 1)
        {
            foreach (Coordinate c in _automaton.Path)
            {
                int index = (c.Row * data.Stride) + (c.Column * 4);
                _rgbaValues[index] = 0;
                _rgbaValues[index + 1] = 0xff;
                _rgbaValues[index + 2] = 0;
                _rgbaValues[index + 3] = 0xff;
            }
        }

        Marshal.Copy(_rgbaValues, 0, data.Scan0, _rgbaValues.Length);
        _canvas.UnlockBits(data);
    }

    private void OnMouse(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;

        var cellWidth = (float)Width / _automaton.Columns;
        var cellHeight = (float)Height / _automaton.Rows;
        int column = (int)(e.X / cellWidth);
        int row = (int)(e.Y / cellHeight);


        if (!_automaton.GetExploitedGrid().IsInside(row, column)) return;
        Coordinate clickedCoordinate = new Coordinate(row, column);

        switch (CurrentMode)
        {
            case InteractiveGridMode.DrawCells:
            {
                _automaton.ForceState(row, column, new State(1));
                break;
            }
            case InteractiveGridMode.SetStart:
            {
                StartPoint = clickedCoordinate;
                _automaton.PathStart = clickedCoordinate;
                _automaton.ForceState(row, column, new State());
                break;
            }
            case InteractiveGridMode.SetEnd:
            {
                EndPoint = clickedCoordinate;
                _automaton.PathEnd = clickedCoordinate;
                _automaton.ForceState(row, column, new State());
                break;
            }
        }
        
        CurrentMode = InteractiveGridMode.DrawCells;
        
        if (!Settings.DisplayedTimer.Enabled) Invalidate();
    }
}