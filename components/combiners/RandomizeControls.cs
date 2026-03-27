using misery.components.grid;
using misery.eng.automaton;

namespace misery.components.combiners;

public class RandomizeControls
{
    public RandomizeControls(Automaton automaton, Button button, NumericUpDown lower, NumericUpDown upper, InteractiveGrid vg)
    {
        button.Click += (s, e) =>
        {
            automaton.Randomize((int)lower.Value, (int)upper.Value);
        };
        button.Text = @"Randomize";
    }
}