using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace misery.utils
{
    public class WindowManager
    {
        private Form _form;
        public WindowManager(Form form)
        {
            _form = form;
        }
    }

    class Space
    {
        Tile[,] tile;

        public Space()
        {

        }

        public int GetHightBetween(Tile a, Tile b)
        {
            return Math.Abs(a.top - b.top);
        }

        public int GetWidthBetween(Tile a, Tile b)
        {
            return Math.Abs(a.left - b.left);
        }

        public (int, int) GetHeightWidthBetween(Tile a, Tile b)
        {
            return (GetHightBetween(a, b), GetWidthBetween(a, b));
        }
    }

    class Tile
    {
        public 
        int top, left;
    }
}
