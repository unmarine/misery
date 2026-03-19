using misery.components.combiners;
using misery.eng;
using misery.eng.automaton;
using misery.eng.neighborhoods;
using misery.utils;

namespace misery.windows;
public sealed class Setup : Form
{
    WindowManager _windowManager;
    SimulationManager _simulationManager;


    RuleSet? _ruleSet = new RuleSet("Unknown");

    Button _buttonAddSimulation = new Button() { Text = @"Add Simulation" };
    ComboBox _comboboxPresets = new ComboBox();
    Button _buttonUsePreset = new Button();
    TextBox _textboxName = new TextBox();
    NumericUpDown _updownWidth, _updownHeight;


    public Setup(SimulationManager simulationManager)
    {
        Height = 900; Width = 1000;
        Text = @"Create Custom Automaton";
        _windowManager = new WindowManager(this, 12, 16);
        _simulationManager = simulationManager;


        Label labelStarting = new Label() { Text = @"Starting" };
        NumericUpDown updownStarting = new NumericUpDown();

        Label labelCounted = new Label() { Text = @"Counted" };
        NumericUpDown updownCounted = new NumericUpDown();

        Label labelResulting = new Label() { Text = @"Resulting" };
        NumericUpDown updownResulting = new NumericUpDown();

        Label labelLower = new Label() { Text = @"Lower" };
        NumericUpDown updownLower = new NumericUpDown();

        Label labelUpper = new Label() { Text = @"Upper" };
        NumericUpDown updownUpper = new NumericUpDown();



        CheckBox isUnconditional = new CheckBox();
        Button addRuleButton = new Button();
        ListBox rules = new ListBox();

        _ = new RulesController(_ruleSet, updownStarting,
            updownCounted, updownResulting,
            updownLower, updownUpper,
            isUnconditional, addRuleButton, rules);
        _buttonAddSimulation.Click += addSimulation;


        _comboboxPresets = new ComboBox();
        _buttonUsePreset = new Button() { Text = @"Use Preset" };
        _buttonUsePreset.Click += addPreset;


        foreach (var ruleSet in Presets.All)
        {
            _comboboxPresets.Items.Add(ruleSet);
        }

        Label widthLabel = new Label { Text = @"Width" };
        _updownWidth = new NumericUpDown();

        Label heightLabel = new Label { Text = @"Height" };
        _updownHeight = new NumericUpDown();

        _updownWidth.Minimum = 1;
        _updownWidth.Maximum = 1;
        _updownWidth.Maximum = decimal.MaxValue;
        _updownHeight.Maximum = decimal.MaxValue;
        _updownWidth.Value = 300;
        _updownHeight.Value = 300;

        Button _buttonLeave = new Button() { Text = "Leave" };
        _buttonLeave.Click += (s, e) =>
        {
            WindowManager.MoveForms(this, new Overview(_simulationManager));
        };
        _windowManager.PlaceControl(_buttonLeave, 11, 13, 11, 15);

        _windowManager.PlaceControl(_textboxName, 0, 9, 0, 11);

        _windowManager.PlaceControl(widthLabel, 0, 7, 0, 7);
        _windowManager.PlaceControl(_updownWidth, 1, 7, 1, 7);
        _windowManager.PlaceControl(heightLabel, 0, 8, 0, 8);
        _windowManager.PlaceControl(_updownHeight, 1, 8, 1, 8);

        _windowManager.PlaceControl(labelStarting, 0, 0, 0, 0);
        _windowManager.PlaceControl(updownStarting, 1, 0, 1, 0);

        _windowManager.PlaceControl(labelCounted, 0, 1, 0, 1);
        _windowManager.PlaceControl(updownCounted, 1, 1, 1, 1);

        _windowManager.PlaceControl(labelResulting, 0, 2, 0, 2);
        _windowManager.PlaceControl(updownResulting, 1, 2, 1, 2);

        _windowManager.PlaceControl(labelLower, 0, 3, 0, 3);
        _windowManager.PlaceControl(updownLower, 1, 3, 1, 3);

        _windowManager.PlaceControl(labelUpper, 0, 4, 0, 4);
        _windowManager.PlaceControl(updownUpper, 1, 4, 1, 4);

        _windowManager.PlaceControl(isUnconditional, 0, 5, 0, 5);
        _windowManager.PlaceControl(addRuleButton, 1, 5, 1, 5);
        _windowManager.PlaceControl(rules, 2, 0, 11, 5);

        _windowManager.PlaceControl(_buttonAddSimulation, 0, 12, 0, 15);

        _windowManager.PlaceControl(_comboboxPresets, 1, 12, 1, 12);
        _windowManager.PlaceControl(_buttonUsePreset, 1, 13, 1, 15);
    }

    private void addPreset(object? sender, EventArgs e)
    {
        RuleSet? selected = _comboboxPresets.SelectedItem as RuleSet;
        if (selected == null) return;
        _ruleSet = selected;
        addSimulation(sender, e);
    }
    private void addSimulation(object? sender, EventArgs e)
    {
        int width = (int)this._updownWidth.Value;
        int height = (int)this._updownHeight.Value;
        string name = _textboxName.Text;

        Automaton automaton = new Automaton(new Moore(), height, width, _ruleSet, name);

        System.Windows.Forms.Timer timer = new();
        timer.Interval = 1;
        timer.Tick += (s, e) => automaton.Advance();
        automaton.Clock = timer;

        _simulationManager.AddSimulation(automaton);
        WindowManager.MoveForms(this, new Overview(_simulationManager));
    }


    protected override void OnPaint(PaintEventArgs e)
    {
        _windowManager.Debug(e.Graphics);
    }
}

