using misery.eng;

namespace misery.components.combiners;

public class StateInputControls
{
        private readonly NumericUpDown _updownSize;
        private readonly NumericUpDown _updownStateValue;

        public StateInputControls(NumericUpDown updownStateValue, NumericUpDown updownSize)
        {
                _updownStateValue = updownStateValue;
                _updownSize = updownSize;

                updownStateValue.Value = 1;
                updownSize.Value = 1;

                updownStateValue.ValueChanged += (_, _) => { 
                    Settings.SetState((int)updownStateValue.Value); 
                };

                updownSize.ValueChanged += (_, _) => { 
                    Settings.SetBrushSize((int)updownSize.Value); 
                };
        }

        public void Actualize()
        {
                _updownSize.Value = Settings.BrushSize;
                _updownStateValue.Value = Settings.BrushState;
        }
}