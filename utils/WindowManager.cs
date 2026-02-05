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
        Tile[,] tiles;
        int _divisionsVertical, _divisionsHorizontal;
        int _height, _width;


        int _divisionHeight, _divisionWidth;

        public Space(int height, int width, int divisionsVertical, int divisionsHorizontal)
        {
            _height = height;
            _width = width;

            _divisionsVertical = divisionsVertical;
            _divisionsHorizontal = divisionsHorizontal;

            _divisionHeight = height / divisionsVertical;
            _divisionWidth = width / divisionsHorizontal;

            tiles = new Tile[divisionsVertical, divisionsHorizontal];

            for (int row = 0; row < _divisionsVertical; row++)
            {
                for (int column = 0; column < _divisionsHorizontal; column++)
                {
                    int top = row * _divisionHeight;
                    int left = column * _divisionWidth;

                    Tile tile = new Tile(top, left, _divisionHeight, _divisionsVertical);
                }
            }

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

        public Tile(int top, int left, int height, int width)
        {
            Top = top;
            Left = left;
            Width = width;
            Height = height;
        }
    }
}
