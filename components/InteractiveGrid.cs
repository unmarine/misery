using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using misery.Eng;

namespace misery.Components;

public sealed class InteractiveGrid : Panel
{
    private Automaton _automaton;
    private Bitmap _canvas;
    private byte[] _rgbaValues;

    public InteractiveGrid(Automaton initial)
    {
        _automaton = initial;
        DoubleBuffered = true;
        _canvas = new Bitmap(_automaton.Columns, _automaton.Rows, PixelFormat.Format32bppPArgb);
        _rgbaValues = new byte[_canvas.Width * _canvas.Height * 4];

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

        foreach (Coordinate c in _automaton.Path)
        {
            int index = (c.Row * data.Stride) + (c.Column * 4);
            _rgbaValues[index] = 0;
            _rgbaValues[index + 1] = 0xff;
            _rgbaValues[index + 2] = 0;
            _rgbaValues[index + 3] = 0xff;
        }

        Marshal.Copy(_rgbaValues, 0, data.Scan0, _rgbaValues.Length);
        _canvas.UnlockBits(data);
    }

    private void OnMouse(object? sender, MouseEventArgs e)
    {
        if (_automaton == null) return;

        var cellWidth = (float)Width / _automaton.Columns;
        var cellHeight = (float)Height / _automaton.Rows;

        int column = (int)(e.X / cellWidth);
        int row = (int)(e.Y / cellHeight);

        if (_automaton.GetExploitedGrid().IsInside(row, column))
        {
            if (e.Button == MouseButtons.Left)
            {
                _automaton.ForceState(row, column, new State(1));
                
                if (Settings.DisplayedTimer != null && !Settings.DisplayedTimer.Enabled) 
                    Invalidate();
            }
        }
    }
}