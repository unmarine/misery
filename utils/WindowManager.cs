namespace misery.utils;

public class WindowManager
{
    private readonly Form _form;
    private readonly Space _space;

    public static void MoveForms(Form from, Form to)
    {
        from.Hide();
        to.Show();
        to.FormClosed += (sender, e) => from.Close();
    }

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

        control.Left = (int)float.Round(_space.GetLowestLeft(a, b));
        control.Top = (int)float.Round(_space.GetLowestTop(a, b));
        control.Width = (int)float.Round(_space.GetWidthBetween(a, b));
        control.Height = (int)float.Round(_space.GetHeightBetween(a, b));
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
                    //label.BackColor = Color.Transparent;
                    break;
                }
            case TextBox textBox:
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.Margin = new Padding(0);
                    break;
                }
                case NumericUpDown numericUpDown:
                {
                    break;
                }
        }
    }
    public void PlaceLabel(string text, int firstRow, int firstColumn, int secondRow, int secondColumn) { 
        Label label = new Label() { Text = text };
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

    public float GetHeightBetween(Tile a, Tile b)
    {
        float lowestTop = GetLowestTop(a, b);
        float highestBottom = Math.Max(a.Top + a.Height, b.Top + b.Height);
        return (int)(highestBottom - lowestTop);
    }

    public float GetWidthBetween(Tile a, Tile b)
    {
        float lowestLeft = GetLowestLeft(a, b);
        float highestRight = Math.Max(a.Left + a.Width, b.Left + b.Width);
        return (int)(highestRight - lowestLeft);
    }

    public float GetLowestTop(Tile a, Tile b)
    {
        return Math.Min(a.Top, b.Top);
    }

    public float GetLowestLeft(Tile a, Tile b)
    {
        return Math.Min(a.Left, b.Left);
    }
}

internal class Tile
{
    public float Top, Left; // upper left corner
    public float Width, Height;

    public Tile(float top, float left, float height, float width)
    {
        Top = top;
        Left = left;
        Width = width;
        Height = height;
    }
}