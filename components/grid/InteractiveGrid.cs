using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using misery.eng;
using misery.eng.automaton;

namespace misery.components.grid;


public sealed class InteractiveGrid : Panel
{
        private readonly Automaton _automaton;

        private readonly GridDrawing _gridDrawing;

        private List<Coordinate> _previousPath = new();

        public InteractiveGrid(Automaton initial)
        {
                _automaton = initial;
                CurrentMode = InteractiveGridMode.DrawCells;
                DoubleBuffered = true;


                var canvas = new Bitmap(_automaton.Columns, _automaton.Rows, PixelFormat.Format32bppPArgb);
                var rgba = new byte[canvas.Width * canvas.Height * 4];
                _gridDrawing = new GridDrawing(canvas, rgba);

                _automaton.GridUpdated += Invalidate;
                MouseMove += OnMouse;
                MouseClick += OnMouse;
                Settings.ColorsChanged += Invalidate;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public InteractiveGridMode CurrentMode { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
                UpdateBitmap();
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                e.Graphics.DrawImage(_gridDrawing.GetCanvas(), 0, 0, Width, Height);
        }

        private void UpdateBitmap()
        {
                var grid = _automaton.DoubleBuffer.ReadBuffer;
                _gridDrawing.UpdateCanvas(grid);

                List<Coordinate> path = new();
                if (_automaton.DoubleBuffer.ReadBuffer.IsInside(_automaton.PathStart) &&
                    _automaton.DoubleBuffer.ReadBuffer.IsInside(_automaton.PathEnd))
                        path = _automaton.PathFinder.FindPath(_automaton.DoubleBuffer.ReadBuffer, _automaton.PathStart,
                                _automaton.PathEnd);
                if (path != _previousPath)
                {
                        _previousPath = path;
                        if (path.Count > 1) _gridDrawing.DrawPathOver(path);
                }

                var canvas = _gridDrawing.GetCanvas();
                var rgba = _gridDrawing.GetRgba();
                var data = canvas.LockBits(new Rectangle(0, 0, canvas.Width, canvas.Height),
                        ImageLockMode.WriteOnly, canvas.PixelFormat);
                Marshal.Copy(rgba, 0, data.Scan0, rgba.Length);
                canvas.UnlockBits(data);
        }

        private void OnMouse(object? sender, MouseEventArgs e)
        {
                if (e.Button != MouseButtons.Left) return;

                var cellWidth = (float)Width / _automaton.Columns;
                var cellHeight = (float)Height / _automaton.Rows;
                var column = (int)(e.X / cellWidth);
                var row = (int)(e.Y / cellHeight);


                if (!_automaton.DoubleBuffer.ReadBuffer.IsInside(row, column)) return;
                var clickedCoordinate = new Coordinate(row, column);

                switch (CurrentMode)
                {
                        case InteractiveGridMode.DrawCells:
                        {
                                if (Settings.BrushSize == 1)
                                        _automaton.DoubleBuffer.ForceState(row, column, new State(Settings.BrushState));
                                else
                                        for (var i = row - Settings.BrushSize;
                                             i < row + Settings.BrushSize / 2 - 1;
                                             i++)
                                        for (var j = column - Settings.BrushSize;
                                             j < column + Settings.BrushSize / 2 - 1;
                                             j++)
                                                _automaton.DoubleBuffer.ForceState(i, j,
                                                        new State(Settings.BrushState));
                                break;
                        }
                        case InteractiveGridMode.SetStart:
                        {
                                _automaton.PathStart = clickedCoordinate;
                                _automaton.DoubleBuffer.ForceState(row, column, new State());
                                break;
                        }
                        case InteractiveGridMode.SetEnd:
                        {
                                _automaton.PathEnd = clickedCoordinate;
                                _automaton.DoubleBuffer.ForceState(row, column, new State());
                                break;
                        }
                }

                CurrentMode = InteractiveGridMode.DrawCells;

                if (_automaton.Clock == null) return;
                if (!_automaton.Clock.Enabled) Invalidate();
        }
}