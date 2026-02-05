using misery.Eng;

namespace misery.Components;

public class VisualGrid: Panel
{
        Grid grid;
        
        public VisualGrid(Grid initial)
        {
                grid = initial;
                DoubleBuffered = true;
                this.MouseMove += OnMouse;
                this.MouseClick += OnMouse;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
                base.OnPaint(e);
                Graphics graphics = e.Graphics;

                float cellWidth = (float)Width / grid.Columns;
                float cellHeight = (float)Height / grid.Rows;
                
                
                for (int row = 0; row < grid.Rows; row++)
                {
                        for (int column = 0; column < grid.Columns; column++)
                        {
                                State state = grid.ReadState(row, column);
                                Color color = Settings.GetColorByState(state);

                                using Brush brush = new SolidBrush(color);
                                graphics.FillRectangle(brush, column * cellWidth, row * cellHeight, cellWidth, cellHeight);
                        }
                }
        }


    private void OnMouse(object sender, MouseEventArgs e)
    {
        float cellWidth = (float)Width / grid.Columns;
        float cellHeight = (float)Height / grid.Rows;

        int column = (int)(e.X / cellWidth);
        int row = (int)(e.Y / cellHeight);
        
        

        if (e.Button == MouseButtons.Left && grid.IsInside(row, column))
        {
            State currentState = grid.ReadState(row, column);

            grid.SetState(row, column, new State(1)); 
            Invalidate();
        }
    }

    public void ReplaceGrid(Grid update)
        {
            if (update != null)
            {
                grid = update;
                Invalidate();
            }
        }
}