using misery.eng;

namespace misery.components.combiners;

public class StateInputControls
{
    
    public StateInputControls(NumericUpDown updownStateValue, NumericUpDown updownSize)
    {
        updownStateValue.Value = 1;
        updownSize.Value = 1;

        updownStateValue.ValueChanged += (s, e) =>
        {
            Settings.SetState((int)updownStateValue.Value);
        };

        updownSize.ValueChanged += (s, e) =>
        {
            Settings.SetBrushSize((int)updownSize.Value);
        }; 
    }
}
