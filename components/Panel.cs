using misery.Eng;

namespace misery.Components;

public class VisualGrid: Panel
{
        Grid grid;
        
        
        public VisualGrid(Grid initial)
        {
                grid = initial;
                DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
                base.OnPaint(e);
                Graphics graphics = e.Graphics;
                float cellSize = Math.Min(this.Width / grid.Columns, this.Height / grid.Rows);

                for (int row = 0; row < grid.Rows; row++)
                {
                        for (int column = 0; column < grid.Columns; column++)
                        {
                                State state = grid.ReadState(row, column);
                                Color color = Settings.GetColorByState(state);

                                using Brush brush = new SolidBrush(color);
                                graphics.FillRectangle(brush, column * cellSize, row * cellSize, cellSize, cellSize);
                        }
                }
        }

        public void ReplaceGrid(Grid update)
        {
                grid = update;
                Invalidate();
        }
}