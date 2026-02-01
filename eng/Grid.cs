namespace misery.Eng;

public class Grid
{
        public int Rows { get; }
        public int Columns { get; }

        private readonly State[,] _grid;

        public Grid(int rows, int columns)
        {
                Rows = rows;
                Columns = columns;
                _grid = new State[rows, columns];
                for (int row = 0; row < rows; row++)
                for (int column = 0; column < columns; column++)
                {
                        _grid[row, column] = new State(0);
                }
        }

        public State ReadState(Coordinate coordinate) => _grid[coordinate.Row, coordinate.Column];
        public State ReadState(int row, int column) => _grid[row, column];

        public void SetState(int row, int column, State state) => _grid[row, column] = state;
        public void SetState(Coordinate coordinate, State state) => SetState(coordinate.Row, coordinate.Column, state);

        public bool IsInside(int row, int column) => row < Rows && column < Columns && column >= 0 && row >= 0;
        public bool IsInside(Coordinate coordinate) => IsInside(coordinate.Row, coordinate.Column);
}