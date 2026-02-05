using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace misery.utils
{
    public class WindowManager
    {
        private Form _form;
        private Space _space; // haha

        public WindowManager(Form form, int divisionsVertical, int divisionsHorizontal)
        {
            _form = form;
            _space = new Space(form.Height, form.Width, divisionsVertical, divisionsHorizontal);
        }

        public void Debug()
        {
            Graphics g = _form.CreateGraphics();
            foreach (var tile in _space.Tiles)
            {
                using Brush brush = new SolidBrush(Color.Red);
                g.FillRectangle(brush, tile.Left, tile.Top, tile.Width, tile.Height);
            }
        }
    }

    class Space
    {
        public Tile[,] Tiles;
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

            Tiles = new Tile[divisionsVertical, divisionsHorizontal];

            for (int row = 0; row < _divisionsVertical; row++)
            {
                for (int column = 0; column < _divisionsHorizontal; column++)
                {
                    int top = row * _divisionHeight;
                    int left = column * _divisionWidth;

                    Tile tile = new Tile(top, left, _divisionHeight, _divisionsVertical);

                    Tiles[row, column] = tile;
                }
            }

        }

        public int GetHightBetween(Tile a, Tile b) => Math.Abs(a.Top - b.Top);
        public int GetWidthBetween(Tile a, Tile b) => Math.Abs(a.Left - b.Left);
        public (int, int) GetHeightWidthBetween(Tile a, Tile b) => (GetHightBetween(a, b), GetWidthBetween(a, b));
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
