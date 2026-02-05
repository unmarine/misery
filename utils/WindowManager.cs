namespace misery.utils;

public class WindowManager
{
        private readonly Form _form;
        private readonly Space _space; // haha

        public WindowManager(Form form, int divisionsVertical, int divisionsHorizontal)
        {
                _form = form;
                _space = new Space(form.ClientSize.Height, form.ClientSize.Width, divisionsVertical,
                        divisionsHorizontal);
        }

        public void PlaceControl(Control control, int firstRow, int firstColumn, int secondRow, int secondColumn)
        {
                _form.Controls.Add(control);
                var a = _space.Tiles[firstRow, firstColumn];
                var b = _space.Tiles[secondRow, secondColumn];

                control.Left = _space.GetLowestLeft(a, b);
                control.Top = _space.GetLowestTop(a, b);
                control.Width = _space.GetWidthBetween(a, b);
                control.Height = _space.GetHeightBetween(a, b);
        }

        public void Debug(Graphics g)
        {
                for (var row = 0; row < _space.Tiles.GetLength(0); row++)
                for (var column = 0; column < _space.Tiles.GetLength(1); column++)
                {
                        var tile = _space.Tiles[row, column];
                        var sum = row + column;
                        Color c;
                        if (sum % 2 == 0) c = Color.Black;
                        else c = Color.White;
                        using Brush brush = new SolidBrush(c);
                        using var font = new Font("Mono", 5);
                        var format = new StringFormat();
                        format.Alignment = StringAlignment.Center;
                        format.LineAlignment = StringAlignment.Center;
                        using Brush textBrush = new SolidBrush(Color.Red);
                        var rect = new RectangleF(tile.Left, tile.Top, tile.Width, tile.Height);
                        g.FillRectangle(brush, tile.Left, tile.Top, tile.Width, tile.Height);
                        g.DrawString(row + ", " + column, font, textBrush, rect, format);
                }
        }

        public void DebugColorPart(Graphics g, int firstRow, int firstColumn, int secondRow, int secondColumn)
        {
                var a = _space.Tiles[firstRow, firstColumn];
                var b = _space.Tiles[secondRow, secondColumn];

                var left = _space.GetLowestLeft(a, b);
                var top = _space.GetLowestTop(a, b);

                var width = _space.GetWidthBetween(a, b);
                var height = _space.GetHeightBetween(a, b);

                using Brush brush = new SolidBrush(Color.Red);
                g.FillRectangle(brush, left, top, width, height);
        }
}

internal class Space
{
        public Tile[,] Tiles;
        private int _divisionsVertical, _divisionsHorizontal;
        private int _height, _width;


        private int _divisionHeight, _divisionWidth;

        public Space(int height, int width, int divisionsVertical, int divisionsHorizontal)
        {
                _height = height;
                _width = width;

                _divisionsVertical = divisionsVertical;
                _divisionsHorizontal = divisionsHorizontal;

                _divisionHeight = height / divisionsVertical;
                _divisionWidth = width / divisionsHorizontal;

                Tiles = new Tile[divisionsVertical, divisionsHorizontal];

                for (var row = 0; row < _divisionsVertical; row++)
                for (var column = 0; column < _divisionsHorizontal; column++)
                {
                        var top = row * _divisionHeight;
                        var left = column * _divisionWidth;

                        var tile = new Tile(top, left, _divisionHeight, _divisionWidth);

                        Tiles[row, column] = tile;
                }
        }

        public int GetHeightBetween(Tile a, Tile b)
        {
                return Math.Abs(a.Top + a.Height - (b.Top + b.Height));
        }

        public int GetWidthBetween(Tile a, Tile b)
        {
                return Math.Abs(a.Left - b.Left);
        }

        public int GetLowestTop(Tile a, Tile b)
        {
                return Math.Min(a.Top, b.Top);
        }

        public int GetLowestLeft(Tile a, Tile b)
        {
                return Math.Min(a.Left, b.Left);
        }

        public (int, int) GetHeightWidthBetween(Tile a, Tile b)
        {
                return (GetHeightBetween(a, b), GetWidthBetween(a, b));
        }
}

internal class Tile
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