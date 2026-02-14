using misery.Eng;

namespace misery.components;

public class ColorPairsController
{
    NumericUpDown statePick;
    Button colorPick;
    Button submitButton;
    ListBox showPairs;
    Color pickedColor = Color.White;

    public ColorPairsController(NumericUpDown s, Button b, Button sbm, ListBox p)
    {
        statePick = s;
        colorPick = b;
        showPairs = p;

        submitButton = sbm;
        submitButton.Text = @"Add";
        submitButton.Click += Submit;

        p.IntegralHeight = false;

        colorPick.Text = @"Color";
        colorPick.Click += ColorPick;
        ReloadDisplay();
    }

    private void ColorPick(object? sender, EventArgs e)
    {
        if (sender == null) return;

        using (ColorDialog dlg = new ColorDialog())
        {
            dlg.ShowDialog();
            pickedColor = dlg.Color;
        }
    }

    private void ReloadDisplay()
    {
        showPairs.Items.Clear();

        foreach (var pair in Settings.ColorByStateValue)
        {
            showPairs.Items.Add(pair.Key + " " + pair.Value.Name);
        }
    }

    private void Submit(object? sender, EventArgs e)
    {
        if (sender == null) return;
        Settings.SetColorForState((int)statePick.Value, pickedColor);
        ReloadDisplay();
        Settings.DisplayedGrid.Invalidate();
    }
}
