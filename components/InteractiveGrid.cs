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
        _rgbaValues = new byte[Math.Abs(_canvas.Width * 4) * _canvas.Height];

        MouseMove += OnMouse;
        MouseClick += OnMouse;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        UpdateBitmap();
        var g = e.Graphics;

        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
        g.DrawImage(_canvas, 0, 0, Width, Height);
    }

    private void UpdateBitmap()
    {
        Grid grid = _automaton.GetReadyGrid();
        BitmapData data = _canvas.LockBits(new Rectangle(0, 0, _canvas.Width, _canvas.Height), ImageLockMode.WriteOnly, _canvas.PixelFormat);

        int bytes = Math.Abs(data.Stride) * _canvas.Height;

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

        Marshal.Copy(_rgbaValues, 0, data.Scan0, bytes);
        _canvas.UnlockBits(data);
    }

    private void OnMouse(object? sender, MouseEventArgs e)
    {
        if (sender == null) return;
        var cellWidth = (float)Width / _automaton.Columns;
        var cellHeight = (float)Height / _automaton.Rows;

        var column = (int)(e.X / cellWidth);
        var row = (int)(e.Y / cellHeight);

        if (e.Button == MouseButtons.Left && _automaton.GetExploitedGrid().IsInside(row, column))
        {
            // not sure
            _automaton.ForceState(row, column, new State(1));
            if (!Settings.DisplayedTimer.Enabled) Invalidate();
        }
    }

    public void ReplaceGrid(Grid update)
    {
        Invalidate();
    }
}