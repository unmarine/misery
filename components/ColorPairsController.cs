using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using misery.Eng;

namespace misery.components;

public class ColorPairsController
{
    NumericUpDown statePick;
    Button colorPick;
    Button submitButton;
    DataGridView showPairs;
    Color pickedColor = Color.White;

    public ColorPairsController(NumericUpDown s, Button b, Button sbm, DataGridView p)
    {
        statePick = s;
        colorPick = b;
        showPairs = p;

        showPairs.Columns.Add("State", "State");
        showPairs.Columns.Add("Color", "Color");

        submitButton = sbm;
        submitButton.Text = "Add";
        submitButton.Click += Submit;

        p.ReadOnly = true;

        colorPick.Text = "Color";
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
        showPairs.Rows.Clear();

        foreach (var pair in Settings.ColorByStateValue)
        {
            showPairs.Rows.Add(pair.Key, pair.Value.Name);
        }
    }
    
    private void Submit(object? sender, EventArgs e)
    {
        if (sender == null) return;
        Settings.SetColorForState((int)statePick.Value, pickedColor);
        ReloadDisplay();
    }
}
