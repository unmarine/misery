using misery.eng.automaton;

namespace misery.eng.pathfinding;

public class DijkstraSearch : Pathfinding
{
        public override string ToString()
        {
                return "Dijkstra";
        }


        public override List<Coordinate> FindPath(Grid grid, Coordinate src, Coordinate dest)
        {
                var rows = grid.Rows;
                var cols = grid.Columns;

                if (!grid.IsInside(src) || !grid.IsInside(dest)) return [];

                if (grid.ReadState(src).Value != 0 || grid.ReadState(dest).Value != 0)
                        return [];

                if (src.Row == dest.Row && src.Column == dest.Column)
                        return [src];

                var closedList = new bool[rows, cols];
                var cellDetails = new Cell[rows, cols];

                for (var i = 0; i < rows; i++)
                for (var j = 0; j < cols; j++)
                {
                        cellDetails[i, j].G = double.MaxValue;
                        cellDetails[i, j].ParentRow = -1;
                        cellDetails[i, j].ParentColumn = -1;
                }

                int startRow = src.Row, startCol = src.Column;
                cellDetails[startRow, startCol].G = 0.0;
                cellDetails[startRow, startCol].ParentRow = startRow;
                cellDetails[startRow, startCol].ParentColumn = startCol;

                var openList = new List<(double g, Coordinate coord)> { (0.0, src) };

                while (openList.Count > 0)
                {
                        var current = openList.OrderBy(t => t.g).First();
                        openList.Remove(current);

                        var row = current.coord.Row;
                        var column = current.coord.Column;
                        closedList[row, column] = true;

                        (int dR, int dC)[] cardinalMoves = [(-1, 0), (1, 0), (0, -1), (0, 1)];

                        foreach (var move in cardinalMoves)
                        {
                                var newRow = row + move.dR;
                                var newColumn = column + move.dC;

                                if (!grid.IsInside(newRow, newColumn)) continue;
                                
                                if (newRow == dest.Row && newColumn == dest.Column)
                                {
                                        cellDetails[newRow, newColumn].ParentRow = row;
                                        cellDetails[newRow, newColumn].ParentColumn = column;
                                        return TracePath(cellDetails, dest);
                                }

                                if (closedList[newRow, newColumn] ||
                                    grid.ReadState(newRow, newColumn).Value != 0) continue;
                                
                                var gNew = cellDetails[row, column].G + 1.0;
                                
                                if (!(gNew < cellDetails[newRow, newColumn].G)) continue;
                                
                                openList.Add((gNew, new Coordinate(newRow, newColumn)));
                                cellDetails[newRow, newColumn].G = gNew;
                                cellDetails[newRow, newColumn].ParentRow = row;
                                cellDetails[newRow, newColumn].ParentColumn = column;
                        }
                }

                return [];
        }
}