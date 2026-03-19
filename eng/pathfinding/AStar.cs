using misery.eng.automaton;

namespace misery.eng.pathfinding
{
    public class AStarSearch : Pathfinding
    {
        public override string ToString() => "AStar";


        public override List<Coordinate> FindPath(Grid grid, Coordinate src, Coordinate dest)
        {
            int rows = grid.Rows;
            int columns = grid.Columns;

            if (!grid.IsInside(src) || !grid.IsInside(dest)) return new List<Coordinate>();

            if (grid.ReadState(src).Value != 0 || grid.ReadState(dest).Value != 0)
                return new List<Coordinate>();

            if (src.Row == dest.Row && src.Column == dest.Column)
                return [src];

            bool[,] closedList = new bool[rows, columns];
            Cell[,] cellDetails = new Cell[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
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
                    int newRow = row + move.dR;
                    int newColumn = column + move.dC;

                    if (grid.IsInside(newRow, newColumn))
                    {
                        if (newRow == dest.Row && newColumn == dest.Column)
                        {
                            cellDetails[newRow, newColumn].ParentRow = row;
                            cellDetails[newRow, newColumn].ParentCol = column;
                            return TracePath(cellDetails, dest);
                        }

                        if (!closedList[newRow, newColumn] && grid.ReadState(newRow, newColumn).Value == 0)
                        {
                            double gNew = cellDetails[row, column].G + 1.0;
                            double hNew = CalculateHValue(newRow, newColumn, dest);
                            double fNew = gNew + hNew;

                            float tolerance = 0.05f;
                            if (Math.Abs(cellDetails[newRow, newColumn].F - double.MaxValue) < tolerance || cellDetails[newRow, newColumn].F > fNew)
                            {
                                openList.Add((fNew, new Coordinate(newRow, newColumn)));
                                cellDetails[newRow, newColumn].F = fNew;
                                cellDetails[newRow, newColumn].G = gNew;
                                cellDetails[newRow, newColumn].H = hNew;
                                cellDetails[newRow, newColumn].ParentRow = row;
                                cellDetails[newRow, newColumn].ParentCol = column;
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
    }
}