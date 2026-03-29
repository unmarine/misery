using misery.eng;

namespace misery.components.combiners;

public class StateInputControls
{
        private readonly NumericUpDown _updownSize;
        private readonly NumericUpDown _updownStateValue;

        public StateInputControls(NumericUpDown updownStateValue, NumericUpDown updownSize)
        {
                this._updownStateValue = updownStateValue;
                this._updownSize = updownSize;

                updownStateValue.Value = 1;
                updownSize.Value = 1;

                updownStateValue.ValueChanged += (s, e) => { Settings.SetState((int)updownStateValue.Value); };

                updownSize.ValueChanged += (s, e) => { Settings.SetBrushSize((int)updownSize.Value); };
        }

        public void Actualize()
        {
                _updownSize.Value = Settings.BrushSize;
                _updownStateValue.Value = Settings.BrushState;
        }
}