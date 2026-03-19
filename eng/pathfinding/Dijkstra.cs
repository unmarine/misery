using misery.eng.automaton;

namespace misery.eng.pathfinding
{
    public class DijkstraSearch : Pathfinding
    {
        public override string ToString() => "Dijkstra";


        public override List<Coordinate> FindPath(Grid grid, Coordinate src, Coordinate dest)
        {
            int rows = grid.Rows;
            int cols = grid.Columns;

            if (!grid.IsInside(src) || !grid.IsInside(dest)) return new List<Coordinate>();

            if (grid.ReadState(src).Value != 0 || grid.ReadState(dest).Value != 0)
                return new List<Coordinate>();

            if (src.Row == dest.Row && src.Column == dest.Column)
                return new List<Coordinate> { src };

            bool[,] closedList = new bool[rows, cols];
            Cell[,] cellDetails = new Cell[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    cellDetails[i, j].G = double.MaxValue;
                    cellDetails[i, j].ParentRow = -1;
                    cellDetails[i, j].ParentCol = -1;
                }
            }

            int startRow = src.Row, startCol = src.Column;
            cellDetails[startRow, startCol].G = 0.0;
            cellDetails[startRow, startCol].ParentRow = startRow;
            cellDetails[startRow, startCol].ParentCol = startCol;

            var openList = new List<(double g, Coordinate coord)>();
            openList.Add((0.0, src));

            while (openList.Count > 0)
            {
                var current = openList.OrderBy(t => t.g).First();
                openList.Remove(current);

                int row = current.coord.Row;
                int column = current.coord.Column;
                closedList[row, column] = true;

                (int dR, int dC)[] cardinalMoves = { (-1, 0), (1, 0), (0, -1), (0, 1) };

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

                            if (gNew < cellDetails[newRow, newColumn].G)
                            {
                                openList.Add((gNew, new Coordinate(newRow, newColumn)));
                                cellDetails[newRow, newColumn].G = gNew;
                                cellDetails[newRow, newColumn].ParentRow = row;
                                cellDetails[newRow, newColumn].ParentCol = column;
                            }
                        }
                    }
                }
            }

            return new List<Coordinate>();
        }
    }
}
