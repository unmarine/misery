using misery.Components;
using misery.Eng;

namespace misery.components;

public class RandomizeControls
{
    private readonly Automaton _automaton;

    private readonly Button _button;
    private readonly NumericUpDown _lower;
    private readonly NumericUpDown _upper;

    public RandomizeControls(Automaton automaton, Button button, NumericUpDown lower, NumericUpDown upper, InteractiveGrid vg)
    {
        _automaton = automaton;
        _button = button;
        _lower = lower;
        _upper = upper;

        _button.Click += OnClick;
        _button.Text = @"Randomize";
    }

    private void OnClick(Object? sender, EventArgs e)
    {
        _automaton.Randomize((int)_lower.Value, (int)_upper.Value);
    }
}