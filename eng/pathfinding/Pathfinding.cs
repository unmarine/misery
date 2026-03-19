using misery.eng.automaton;

namespace misery.eng.pathfinding
{
    public abstract class Pathfinding
    {
        protected struct Cell
        {
            public int ParentRow, ParentColumn;
            public double F, G, H;
        }

        protected static List<Coordinate> TracePath(Cell[,] cellDetails, Coordinate dest)
        {
            Stack<Coordinate> pathStack = new Stack<Coordinate>();
            int row = dest.Row;
            int column = dest.Column;

            while (!(cellDetails[row, column].ParentRow == row && cellDetails[row, column].ParentColumn == column))
            {
                pathStack.Push(new Coordinate(row, column));
                (row, column) = (cellDetails[row, column].ParentRow, cellDetails[row, column].ParentColumn);
            }
            pathStack.Push(new Coordinate(row, column));

            return pathStack.ToList();
        }

        public abstract List<Coordinate> FindPath(Grid grid, Coordinate src, Coordinate dest);

        public override string ToString() => "Unnamed";
    }
}
