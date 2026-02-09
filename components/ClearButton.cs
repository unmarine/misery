using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using misery.Eng;

namespace misery.components;


internal class ClearButton: Button
{
    private Automaton _automaton;

    public ClearButton(Automaton automaton)
    {
        _automaton = automaton;
        Text = "Clear";
    }

    protected override void OnClick(EventArgs e)
    {
        _automaton.Clear();
        Settings.DisplayedGrid.Invalidate();
    }
}
