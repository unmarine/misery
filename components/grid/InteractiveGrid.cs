using System.ComponentModel;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using misery.eng.automaton;
using misery.eng;

namespace misery.components.grid;

public enum InteractiveGridMode
{
    DrawCells,
    SetStart,
    SetEnd
}

public sealed class InteractiveGrid : Panel
{
    private Automaton _automaton;

    public GridDrawing gridDrawing;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public InteractiveGridMode CurrentMode { get; set; }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Coordinate StartPoint { get; set; }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Coordinate EndPoint { get; set; }
    List<Coordinate> PreviousPath = new List<Coordinate>();

    public InteractiveGrid(Automaton initial)
    {
        _automaton = initial;
        CurrentMode = InteractiveGridMode.DrawCells;
        DoubleBuffered = true;


        var canvas = new Bitmap(_automaton.Columns, _automaton.Rows, PixelFormat.Format32bppPArgb);
        var rgba = new byte[canvas.Width * canvas.Height * 4];
        gridDrawing = new GridDrawing(canvas, rgba);

        _automaton.GridUpdated += Invalidate;
        MouseMove += OnMouse;
        MouseClick += OnMouse;
        Settings.ColorsChanged += Invalidate;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        UpdateBitmap();
        e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
        e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
        e.Graphics.DrawImage(gridDrawing.GetCanvas(), 0, 0, Width, Height);
    }

    private void UpdateBitmap()
    {
        Grid grid = _automaton.doubleBuffer.ReadBuffer;
        gridDrawing.UpdateCanvas(grid);
        
        List<Coordinate> path = new();
        if (_automaton.doubleBuffer.ReadBuffer.IsInside(_automaton.PathStart) && _automaton.doubleBuffer.ReadBuffer.IsInside(_automaton.PathEnd))
        {
            path = _automaton.PathFinder.FindPath(_automaton.doubleBuffer.ReadBuffer, _automaton.PathStart, _automaton.PathEnd);
        }
        if (!(path == PreviousPath))
        {
            PreviousPath = path;
            if (path.Count > 1)
            {
                gridDrawing.DrawPathOver(path);
            }
        }

        var canvas = gridDrawing.GetCanvas();
        var rgba = gridDrawing.GetRgba();
        BitmapData data = canvas.LockBits(new Rectangle(0, 0, canvas.Width, canvas.Height),
                         ImageLockMode.WriteOnly, canvas.PixelFormat);
        Marshal.Copy(rgba, 0, data.Scan0, rgba.Length);
        canvas.UnlockBits(data);
    }

    private void OnMouse(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;

        var cellWidth = (float)Width / _automaton.Columns;
        var cellHeight = (float)Height / _automaton.Rows;
        int column = (int)(e.X / cellWidth);
        int row = (int)(e.Y / cellHeight);


        if (!_automaton.doubleBuffer.ReadBuffer.IsInside(row, column)) return;
        Coordinate clickedCoordinate = new Coordinate(row, column);

        switch (CurrentMode)
        {
            case InteractiveGridMode.DrawCells:
                {
                    if (Settings.brushSize == 1) _automaton.doubleBuffer.ForceState(row, column, new State(Settings.brushState));
                    else for (int i = row - Settings.brushSize; i < row + Settings.brushSize / 2 - 1; i++)
                            for (int j = column - Settings.brushSize; j < column + Settings.brushSize / 2 - 1; j++)
                                _automaton.doubleBuffer.ForceState(i, j, new State(Settings.brushState));
                    break;
                }
            case InteractiveGridMode.SetStart:
                {
                    StartPoint = clickedCoordinate;
                    _automaton.PathStart = clickedCoordinate;
                    _automaton.doubleBuffer.ForceState(row, column, new State());
                    break;
                }
            case InteractiveGridMode.SetEnd:
                {
                    EndPoint = clickedCoordinate;
                    _automaton.PathEnd = clickedCoordinate;
                    _automaton.doubleBuffer.ForceState(row, column, new State());
                    break;
                }
        }

        CurrentMode = InteractiveGridMode.DrawCells;

        if (_automaton.Clock != null)
            if (!_automaton.Clock.Enabled) Invalidate();
    }
}