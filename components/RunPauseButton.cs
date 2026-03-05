using misery.Eng;

namespace misery.components;

public sealed class RunPauseButton : Button
{
    private Automaton _automaton;

    public RunPauseButton(Automaton automaton)
    {
        _automaton = automaton;
        Text = @"Run";
    }

    protected override void OnClick(EventArgs e)
    {
        if (_automaton.Clock.Enabled)
        {
            Text = @"Run";
            _automaton.Clock.Stop();
        }
        else
        {
            Text = @"Pause";
            _automaton.Clock.Start();
        }
    }

    public void Actualize()
    {
        if (!_automaton.Clock.Enabled)
        {
            Text = @"Run";
        }
        else
        {
            Text = @"Pause";
        }
    }
}