using misery.components.combiners;
using misery.eng;
using misery.eng.automaton;
using misery.eng.neighborhoods;
using misery.utils;
using Timer = System.Windows.Forms.Timer;

namespace misery.windows;

public sealed class Setup : Form
{
        private readonly Button _buttonAddSimulation = new() { Text = @"Add Simulation" };
        private readonly Button _buttonUsePreset;
        private readonly ComboBox _comboboxPresets;
        private readonly SimulationManager _simulationManager;
        private readonly TextBox _textboxName = new();
        private readonly NumericUpDown _updownHeight;
        private readonly NumericUpDown _updownWidth;


        private RuleSet? _ruleSet = new("Unknown");


        public Setup(SimulationManager simulationManager)
        {
                Height = 900;
                Width = 1000;
                Text = @"Create Custom Automaton";
                var windowManager = new WindowManager(this, 12, 9);
                _simulationManager = simulationManager;


                var updownStarting = new NumericUpDown();

                var updownCounted = new NumericUpDown();

                var updownResulting = new NumericUpDown();

                var updownLower = new NumericUpDown();

                var updownUpper = new NumericUpDown();


                var isUnconditional = new CheckBox();
                var addRuleButton = new Button();
                var rules = new ListBox();

                _ = new RulesControls(_ruleSet, updownStarting,
                        updownCounted, updownResulting,
                        updownLower, updownUpper,
                        isUnconditional, addRuleButton, rules);
                _buttonAddSimulation.Click += AddSimulation;


                _comboboxPresets = new ComboBox();
                _buttonUsePreset = new Button { Text = @"Use Preset" };
                _buttonUsePreset.Click += AddPreset;


                foreach (var ruleSet in Presets.All) _comboboxPresets.Items.Add(ruleSet);

                _updownWidth = new NumericUpDown();
                _updownHeight = new NumericUpDown();

                _updownWidth.Minimum = 1;
                _updownWidth.Maximum = 1;
                _updownWidth.Maximum = decimal.MaxValue;
                _updownHeight.Maximum = decimal.MaxValue;
                _updownWidth.Value = 300;
                _updownHeight.Value = 300;

                var buttonLeave = new Button { Text = "Leave" };
                buttonLeave.Click += (s, e) => { WindowManager.MoveForms(this, new Overview(_simulationManager)); };
                windowManager.PlaceControl(buttonLeave, 11, 8, 11, 8);
                windowManager.PlaceLabel("Description of simulation", 0, 0, 0, 8);


                windowManager.PlaceLabel("Name", 1, 0, 1, 1);
                windowManager.PlaceControl(_textboxName, 2, 0, 2, 1);

                windowManager.PlaceLabel("Width", 1, 2, 1, 3);
                windowManager.PlaceControl(_updownWidth, 2, 2, 2, 3);

                windowManager.PlaceLabel("Height", 1, 4, 1, 5);
                windowManager.PlaceControl(_updownHeight, 2, 4, 2, 5);

                windowManager.PlaceLabel("Starting", 7, 2, 7, 2);
                windowManager.PlaceControl(updownStarting, 8, 2, 8, 2);

                windowManager.PlaceLabel("Counted", 9, 2, 9, 2);
                windowManager.PlaceControl(updownCounted, 10, 2, 10, 2);

                windowManager.PlaceLabel("Resulting", 7, 3, 7, 3);
                windowManager.PlaceControl(updownResulting, 8, 3, 8, 3);

                windowManager.PlaceLabel("Lower", 9, 3, 9, 3);
                windowManager.PlaceControl(updownLower, 10, 3, 10, 3);

                windowManager.PlaceLabel("Upper", 7, 4, 7, 4);
                windowManager.PlaceControl(updownUpper, 8, 4, 8, 4);

                windowManager.PlaceLabel("Awlays", 9, 4, 9, 4);
                windowManager.PlaceControl(isUnconditional, 10, 4, 10, 4);

                windowManager.PlaceControl(addRuleButton, 11, 2, 11, 3);

                windowManager.PlaceControl(rules, 7, 0, 11, 1);

                windowManager.PlaceControl(_buttonAddSimulation, 11, 4, 11, 4);

                windowManager.PlaceLabel("Logic of simulation", 6, 0, 6, 8);

                windowManager.PlaceControl(_buttonUsePreset, 7, 6, 7, 7);
                windowManager.PlaceControl(_comboboxPresets, 8, 6, 8, 7);
        }

        private void AddPreset(object? sender, EventArgs e)
        {
                var selected = _comboboxPresets.SelectedItem as RuleSet;
                if (selected == null) return;
                _ruleSet = selected;
                AddSimulation(sender, e);
        }

        private void AddSimulation(object? sender, EventArgs e)
        {
                var width = (int)_updownWidth.Value;
                var height = (int)_updownHeight.Value;
                var name = _textboxName.Text;

                _ruleSet ??= new RuleSet("Unknown Rule Set");
                var automaton = new Automaton(new Moore(), height, width, _ruleSet, name);

                Timer timer = new();
                timer.Interval = 1;
                timer.Tick += (_, _) => automaton.Advance();
                automaton.Clock = timer;

                _simulationManager.AddSimulation(automaton);
                WindowManager.MoveForms(this, new Overview(_simulationManager));
        }


        protected override void OnPaint(PaintEventArgs e)
        {
                //_windowManager.Debug(e.Graphics);
        }
}