using misery.Eng;

namespace misery.eng
{
    public class AStarSearch
    {
        private struct Cell
        {
            public int ParentRow, ParentCol;
            public double F, G, H;
        }

        public static List<Coordinate> FindPath(Grid grid, Coordinate src, Coordinate dest)
        {
            int rows = grid.Rows;
            int cols = grid.Columns;

            if (!grid.IsInside(src) || !grid.IsInside(dest)) return new List<Coordinate>();
            
            if (grid.ReadState(src).Value != 0 || grid.ReadState(dest).Value != 0) 
                return new List<Coordinate>();

            if (src.Row == dest.Row && src.Column == dest.Column)
                return [src];

            bool[,] closedList = new bool[rows, cols];
            Cell[,] cellDetails = new Cell[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    cellDetails[i, j].F = double.MaxValue;
                    cellDetails[i, j].G = double.MaxValue;
                    cellDetails[i, j].H = double.MaxValue;
                    cellDetails[i, j].ParentRow = -1;
                    cellDetails[i, j].ParentCol = -1;
                }
            }

            int row = src.Row, column = src.Column;
            cellDetails[row, column].F = 0.0;
            cellDetails[row, column].G = 0.0;
            cellDetails[row, column].H = 0.0;
            cellDetails[row, column].ParentRow = row;
            cellDetails[row, column].ParentCol = column;

            var openList = new List<(double f, Coordinate coord)>();
            openList.Add((0.0, src));

            while (openList.Count > 0)
            {
                var current = openList.OrderBy(t => t.f).First();
                openList.Remove(current);

                row = current.coord.Row;
                column = current.coord.Column;
                closedList[row, column] = true;

                (int dR, int dC)[] cardinalMoves = [(-1, 0), (1, 0), (0, -1), (0, 1)];

                foreach (var move in cardinalMoves)
                {
                    int newR = row + move.dR;
                    int newC = column + move.dC;

                    if (grid.IsInside(newR, newC))
                    {
                        if (newR == dest.Row && newC == dest.Column)
                        {
                            cellDetails[newR, newC].ParentRow = row;
                            cellDetails[newR, newC].ParentCol = column;
                            return TracePath(cellDetails, dest);
                        }

                        if (!closedList[newR, newC] && grid.ReadState(newR, newC).Value == 0)
                        {
                            double gNew = cellDetails[row, column].G + 1.0;
                            double hNew = CalculateHValue(newR, newC, dest);
                            double fNew = gNew + hNew;

                            float tolerance = 0.05f;
                            if (Math.Abs(cellDetails[newR, newC].F - double.MaxValue) < tolerance || cellDetails[newR, newC].F > fNew)
                            {
                                openList.Add((fNew, new Coordinate(newR, newC)));
                                cellDetails[newR, newC].F = fNew;
                                cellDetails[newR, newC].G = gNew;
                                cellDetails[newR, newC].H = hNew;
                                cellDetails[newR, newC].ParentRow = row;
                                cellDetails[newR, newC].ParentCol = column;
                            }
                        }
                    }
                }
            }
            
            return new List<Coordinate>();
        }

        private static double CalculateHValue(int row, int col, Coordinate dest)
        {
            return Math.Sqrt(Math.Pow(row - dest.Row, 2) + Math.Pow(col - dest.Column, 2));
        }

        private static List<Coordinate> TracePath(Cell[,] cellDetails, Coordinate dest)
        {
            Stack<Coordinate> pathStack = new Stack<Coordinate>();
            int row = dest.Row;
            int column = dest.Column;

            while (!(cellDetails[row, column].ParentRow == row && cellDetails[row, column].ParentCol == column))
            {
                pathStack.Push(new Coordinate(row, column));
                int tempR = cellDetails[row, column].ParentRow;
                int tempC = cellDetails[row, column].ParentCol;
                row = tempR;
                column = tempC;
            }
            pathStack.Push(new Coordinate(row, column));

            return pathStack.ToList();
        }
    }
}