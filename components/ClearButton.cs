using misery.Eng;

namespace misery.components;


internal class ClearButton : Button
{
    private Automaton _automaton;

    public ClearButton(Automaton automaton)
    {
        _automaton = automaton;
        Text = @"Clear";
    }

    protected override void OnClick(EventArgs e)
    {
        _automaton.Clear();
    }
}
