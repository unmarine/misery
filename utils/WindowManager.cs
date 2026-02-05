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
            _space = new Space(form.ClientSize.Height, form.ClientSize.Width, divisionsVertical, divisionsHorizontal);
        }

        public void PlaceControl(Control control, int firstRow, int firstColumn, int secondRow, int secondColumn)
        {
            _form.Controls.Add(control);
            Tile a = _space.Tiles[firstRow, firstColumn];
            Tile b = _space.Tiles[secondRow, secondColumn];

            control.Left = _space.GetLowestLeft(a, b);
            control.Top = _space.GetLowestTop(a, b);
            control.Width = _space.GetWidthBetween(a, b);
            control.Height = _space.GetHeightBetween(a, b);
        }

        public void Debug(Graphics g)
        {
            for (int row = 0; row < _space.Tiles.GetLength(0); row++)
            {
                for (int column = 0; column < _space.Tiles.GetLength(1); column++)
                {
                    Tile tile = _space.Tiles[row, column];
                    int sum = row + column;
                    Color c;
                    if (sum % 2 == 0) c = Color.Black;
                    else c = Color.White;
                    using Brush brush = new SolidBrush(c);
                    using Font font = new Font("Mono", 5);
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    using Brush textBrush = new SolidBrush(Color.Red);
                    RectangleF rect = new RectangleF(tile.Left, tile.Top, tile.Width, tile.Height);
                    g.FillRectangle(brush, tile.Left, tile.Top, tile.Width, tile.Height);
                    g.DrawString(row + ", " + column, font , textBrush, rect, format);
                }
            }
        }

        public void DebugColorPart(Graphics g, int firstRow, int firstColumn, int secondRow, int secondColumn)
        {
            Tile a = _space.Tiles[firstRow, firstColumn];
            Tile b = _space.Tiles[secondRow, secondColumn];

            int left = _space.GetLowestLeft(a, b);
            int top = _space.GetLowestTop(a, b);

            int width = _space.GetWidthBetween(a, b);
            int height = _space.GetHeightBetween(a, b);
            
            using Brush brush = new SolidBrush(Color.Red);
            g.FillRectangle(brush, left, top, width, height);
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

                    Tile tile = new Tile(top, left, _divisionHeight, _divisionWidth);

                    Tiles[row, column] = tile;
                }
            }

        }

        public int GetHeightBetween(Tile a, Tile b) => Math.Abs( (a.Top + a.Height) - (b.Top + b.Height));
        public int GetWidthBetween(Tile a, Tile b) => Math.Abs(a.Left - b.Left);
        
        public int GetLowestTop(Tile a, Tile b) => Math.Min(a.Top, b.Top);
        public int GetLowestLeft(Tile a, Tile b) => Math.Min(a.Left, b.Left);
        public (int, int) GetHeightWidthBetween(Tile a, Tile b) => (GetHeightBetween(a, b), GetWidthBetween(a, b));
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
