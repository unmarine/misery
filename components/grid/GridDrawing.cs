using System.Drawing.Imaging;
using System.IO;
using misery.eng;
using misery.eng.automaton;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace misery.components.grid;

public class GridDrawing
{
    Bitmap _canvas;
    byte[] _rgba;

    public GridDrawing(Bitmap canvas, byte[] rgba)
    {
        _canvas = canvas;
        _rgba = rgba;
    }

    public Bitmap GetCanvas() { 
        return _canvas;
    }
    public byte[] GetRgba()
    {
        return _rgba;
    }

    public void UpdateCanvas(Grid grid)
    {
        BitmapData data = _canvas.LockBits(new Rectangle(0, 0, _canvas.Width, _canvas.Height),
                                 ImageLockMode.WriteOnly, _canvas.PixelFormat);

        for (int row = 0; row < grid.Rows; row++)
        {
            for (int column = 0; column < grid.Columns; column++)
            {
                byte r, g, b, a;
                State state = grid.ReadState(row, column);

                if (Settings.IsViewingActivity)
                {
                    double t = state.GetNormalizedIndex();

                    r = (byte)(255 * (1.0 - t));
                    g = 0;
                    b = (byte)(255 * t);
                    a = 0xff;
                }
                else
                {
                    Color color = Settings.GetColorByValue(state.Value);
                    r = color.R;
                    g = color.G;
                    b = color.B;
                    a = color.A;
                }

                int index = (row * data.Stride) + (column * 4);
                _rgba[index] = b;
                _rgba[index + 1] = g;
                _rgba[index + 2] = r;
                _rgba[index + 3] = a;
            }
        }

        _canvas.UnlockBits(data);
    }

    public void DrawPathOver(List<Coordinate> coordinates)
    {
        BitmapData data = _canvas.LockBits(new Rectangle(0, 0, _canvas.Width, _canvas.Height),
                         ImageLockMode.WriteOnly, _canvas.PixelFormat);

        foreach (Coordinate c in coordinates)
        {
            int index = (c.Row * data.Stride) + (c.Column * 4);
            _rgba[index] = 0;
            _rgba[index + 1] = 0xff;
            _rgba[index + 2] = 0;
            _rgba[index + 3] = 0xff;
        }

        _canvas.UnlockBits(data);
    }
}