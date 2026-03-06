using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using misery.Eng;

namespace misery.eng
{
    public abstract class Pathfinding
    {
        protected struct Cell
        {
            public int ParentRow, ParentCol;
            public double F, G, H;
        }

        protected static List<Coordinate> TracePath(Cell[,] cellDetails, Coordinate dest)
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

        public abstract List<Coordinate> FindPath(Grid grid, Coordinate src, Coordinate dest);

        public override string ToString()
        {
            return "Unnamed";
        }
    }
}
