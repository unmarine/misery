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

        public Space(int height, int width, int divisions)
        {




        }

        public int GetHightBetween(Tile a, Tile b)
        {
            return Math.Abs(a.Top - b.Top);
        }

        public int GetWidthBetween(Tile a, Tile b)
        {
            return Math.Abs(a.Left - b.Left);
        }

        public (int, int) GetHeightWidthBetween(Tile a, Tile b)
        {
            return (GetHightBetween(a, b), GetWidthBetween(a, b));
        }
    }

    class Tile
    {
        public int Top, Left; // upper left corner
        public int Width, Height;

        public Tile(int top, int left, int width, int height)
        {
            Top = top;
            Left = left;
            Width = width;
            Height = height;
        }
    }
}
