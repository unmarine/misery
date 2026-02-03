using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace misery.components;
public class ColorKeyInputDisplay
{
    int top, left, width, height;

    NumericUpDown stateInput;
    TextBox colorInput;


    public ColorKeyInputDisplay(int top, int left, int width, int height)
    {
        stateInput = new NumericUpDown();
        colorInput = new TextBox();

        stateInput.Top = top; colorInput.Top = top;
        stateInput.Left = left; colorInput.Left = (int)(left + 0.5 * width);

        stateInput.Width = (int)(width * 0.5);
        colorInput.Width = (int)(width * 0.5);

    }

    public void Hinzufugen(Form form)
    {
        form.Controls.Add(stateInput);
        form.Controls.Add(colorInput);
    }
}

