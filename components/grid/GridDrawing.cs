using System.Drawing.Imaging;
using misery.eng;
using misery.eng.automaton;

namespace misery.components.grid;

public class GridDrawing(Bitmap canvas, byte[] rgba)
{
        public Bitmap GetCanvas()
        {
                return canvas;
        }

        public byte[] GetRgba()
        {
                return rgba;
        }

        public void UpdateCanvas(Grid grid)
        {
                var data = canvas.LockBits(new Rectangle(0, 0, canvas.Width, canvas.Height),
                        ImageLockMode.WriteOnly, canvas.PixelFormat);

                for (var row = 0; row < grid.Rows; row++)
                for (var column = 0; column < grid.Columns; column++)
                {
                        byte r, g, b, a;
                        var state = grid.ReadState(row, column);

                        if (Settings.IsViewingActivity)
                        {
                                var t = state.GetNormalizedIndex();
                                r = (byte)(255 * t);
                                g = 0;
                                b = (byte)(255 * (1 - t));
                                a = 0xff;
                        }
                        else
                        {
                                var color = Settings.GetColorByValue(state.Value);
                                r = color.R;
                                g = color.G;
                                b = color.B;
                                a = color.A;
                        }

                        var index = row * data.Stride + column * 4;
                        rgba[index] = b;
                        rgba[index + 1] = g;
                        rgba[index + 2] = r;
                        rgba[index + 3] = a;
                }

                canvas.UnlockBits(data);
        }

        public void DrawPathOver(List<Coordinate> coordinates)
        {
                var data = canvas.LockBits(new Rectangle(0, 0, canvas.Width, canvas.Height),
                        ImageLockMode.WriteOnly, canvas.PixelFormat);

                foreach (var c in coordinates)
                {
                        var index = c.Row * data.Stride + c.Column * 4;
                        rgba[index] = 0;
                        rgba[index + 1] = 0xff;
                        rgba[index + 2] = 0;
                        rgba[index + 3] = 0xff;
                }

                canvas.UnlockBits(data);
        }
}