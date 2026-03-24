using misery.eng;

namespace misery.components.combiners;

public class StateInputControls
{
    private NumericUpDown updownStateValue, updownSize;
    
    public StateInputControls(NumericUpDown updownStateValue, NumericUpDown updownSize)
    {
        this.updownStateValue = updownStateValue;
        this.updownSize = updownSize;

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

    public void Actualize()
    {
        updownSize.Value = Settings.brushSize;
        updownStateValue.Value = Settings.brushState;
    }
}
