using misery.Eng;

namespace misery.Components;

public sealed class VisualGrid : Panel
{
        private Automaton _automaton;

        public VisualGrid(Automaton initial)
        {
                _automaton = initial;
                DoubleBuffered = true;
                MouseMove += OnMouse;
                MouseClick += OnMouse;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
                base.OnPaint(e);
                var graphics = e.Graphics;

                var cellWidth = (float)Width / _automaton.Columns;
                var cellHeight = (float)Height / _automaton.Rows;


                for (var row = 0; row < _automaton.Rows; row++)
                for (var column = 0; column < _automaton.Columns; column++)
                {
                        var state = _automaton.GetReadyGrid().ReadState(row, column);
                        var color = Settings.GetColorByState(state);

                        using Brush brush = new SolidBrush(color);
                        graphics.FillRectangle(brush, column * cellWidth, row * cellHeight, cellWidth, cellHeight);
                }
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
                        Invalidate();
                }
        }

        public void ReplaceGrid(Grid update)
        {
                // _automaton.TheGrid = update;
                Invalidate();
        }
}