namespace misery.utils;

public class WindowManager
{
        private readonly Form _form;
        private readonly Space _space;

        public WindowManager(Form form, int divisionsVertical, int divisionsHorizontal)
        {
                _form = form;
                _space = new Space(form.ClientSize.Height, form.ClientSize.Width, divisionsVertical,
                        divisionsHorizontal);
        }

        public static void MoveForms(Form from, Form to)
        {
                from.Hide();
                to.Show();
                to.FormClosed += (_, _) => from.Close();
        }

        public void PlaceControl(Control control, int firstRow, int firstColumn, int secondRow, int secondColumn)
        {
                _form.Controls.Add(control);
                var a = _space.Tiles[firstRow, firstColumn];
                var b = _space.Tiles[secondRow, secondColumn];

                control.Left = (int)float.Round(Space.GetLowestLeft(a, b));
                control.Top = (int)float.Round(Space.GetLowestTop(a, b));
                control.Width = (int)float.Round(Space.GetWidthBetween(a, b));
                control.Height = (int)float.Round(Space.GetHeightBetween(a, b));
                
                control.BackColor = Color.White;
                control.BackColor = Color.FromArgb(250, 250, 250);
                control.ForeColor = Color.FromArgb(30, 30, 30);
                control.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

                _form.BackColor = Color.FromArgb(230, 230, 230);

                switch (control)
                {
                        case Button button:
                        {
                                button.FlatStyle = FlatStyle.Flat;
                                button.FlatAppearance.BorderSize = 1;
                                button.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
                                button.Padding = new Padding(6, 4, 6, 4);
                                button.TextAlign = ContentAlignment.MiddleCenter;
                                button.BackColor = Color.FromArgb(240, 240, 240);
                                button.Cursor = Cursors.Hand;

                                break;
                        }
                        case Label label:
                        {
                                label.AutoSize = false;
                                label.TextAlign = ContentAlignment.BottomLeft;
                                label.Padding = new Padding(4, 2, 4, 2);
                                break;
                        }
                        case TextBox textBox:
                        {
                                textBox.BorderStyle = BorderStyle.FixedSingle;
                                textBox.Margin = new Padding(0);
                                break;
                        }
                }
        }

        public void PlaceLabel(string text, int firstRow, int firstColumn, int secondRow, int secondColumn)
        {
                var label = new Label { Text = text };
                PlaceControl(label, firstRow, firstColumn, secondRow, secondColumn);
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

                var left = Space.GetLowestLeft(a, b);
                var top = Space.GetLowestTop(a, b);

                var width = Space.GetWidthBetween(a, b);
                var height = Space.GetHeightBetween(a, b);

                using Brush brush = new SolidBrush(Color.Red);
                g.FillRectangle(brush, left, top, width, height);
        }
}

internal class Space
{
        public readonly Tile[,] Tiles;


        public Space(int height, int width, int divisionsVertical, int divisionsHorizontal)
        {
                var divisionHeight = height / divisionsVertical;
                var divisionWidth = width / divisionsHorizontal;

                Tiles = new Tile[divisionsVertical, divisionsHorizontal];

                for (var row = 0; row < divisionsVertical; row++)
                for (var column = 0; column < divisionsHorizontal; column++)
                {
                        var top = row * divisionHeight;
                        var left = column * divisionWidth;

                        var tile = new Tile(top, left, divisionHeight, divisionWidth);

                        Tiles[row, column] = tile;
                }
        }

        public static float GetHeightBetween(Tile a, Tile b)
        {
                var lowestTop = GetLowestTop(a, b);
                var highestBottom = Math.Max(a.Top + a.Height, b.Top + b.Height);
                return (int)(highestBottom - lowestTop);
        }

        public static float GetWidthBetween(Tile a, Tile b)
        {
                var lowestLeft = GetLowestLeft(a, b);
                var highestRight = Math.Max(a.Left + a.Width, b.Left + b.Width);
                return (int)(highestRight - lowestLeft);
        }

        public static float GetLowestTop(Tile a, Tile b)
        {
                return Math.Min(a.Top, b.Top);
        }

        public static float GetLowestLeft(Tile a, Tile b)
        {
                return Math.Min(a.Left, b.Left);
        }
}

internal class Tile
{
        public readonly float Top; // upper left corner
        public readonly float Left; // upper left corner
        public readonly float Width;
        public readonly float Height;

        public Tile(float top, float left, float height, float width)
        {
                Top = top;
                Left = left;
                Width = width;
                Height = height;
        }
}