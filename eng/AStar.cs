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

            int r = src.Row, c = src.Column;
            cellDetails[r, c].F = 0.0;
            cellDetails[r, c].G = 0.0;
            cellDetails[r, c].H = 0.0;
            cellDetails[r, c].ParentRow = r;
            cellDetails[r, c].ParentCol = c;

            var openList = new List<(double f, Coordinate coord)>();
            openList.Add((0.0, src));

            while (openList.Count > 0)
            {
                var current = openList.OrderBy(t => t.f).First();
                openList.Remove(current);

                r = current.coord.Row;
                c = current.coord.Column;
                closedList[r, c] = true;

                (int dR, int dC)[] cardinalMoves = [(-1, 0), (1, 0), (0, -1), (0, 1)];

                foreach (var move in cardinalMoves)
                {
                    int newR = r + move.dR;
                    int newC = c + move.dC;

                    if (grid.IsInside(newR, newC))
                    {
                        if (newR == dest.Row && newC == dest.Column)
                        {
                            cellDetails[newR, newC].ParentRow = r;
                            cellDetails[newR, newC].ParentCol = c;
                            return TracePath(cellDetails, dest);
                        }

                        if (!closedList[newR, newC] && grid.ReadState(newR, newC).Value == 0)
                        {
                            double gNew = cellDetails[r, c].G + 1.0;
                            double hNew = CalculateHValue(newR, newC, dest);
                            double fNew = gNew + hNew;

                            float tolerance = 0.05f;
                            if (Math.Abs(cellDetails[newR, newC].F - double.MaxValue) < tolerance || cellDetails[newR, newC].F > fNew)
                            {
                                openList.Add((fNew, new Coordinate(newR, newC)));
                                cellDetails[newR, newC].F = fNew;
                                cellDetails[newR, newC].G = gNew;
                                cellDetails[newR, newC].H = hNew;
                                cellDetails[newR, newC].ParentRow = r;
                                cellDetails[newR, newC].ParentCol = c;
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
            int r = dest.Row;
            int c = dest.Column;

            while (!(cellDetails[r, c].ParentRow == r && cellDetails[r, c].ParentCol == c))
            {
                pathStack.Push(new Coordinate(r, c));
                int tempR = cellDetails[r, c].ParentRow;
                int tempC = cellDetails[r, c].ParentCol;
                r = tempR;
                c = tempC;
            }
            pathStack.Push(new Coordinate(r, c));

            return pathStack.ToList();
        }
    }
}