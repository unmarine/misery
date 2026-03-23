using System.Net.Http.Headers;
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
        _windowManager = new WindowManager(this, 12, 9);
        _simulationManager = simulationManager;


        NumericUpDown updownStarting = new NumericUpDown();

        NumericUpDown updownCounted = new NumericUpDown();

        NumericUpDown updownResulting = new NumericUpDown();

        NumericUpDown updownLower = new NumericUpDown();

        NumericUpDown updownUpper = new NumericUpDown();



        CheckBox isUnconditional = new CheckBox();
        Button addRuleButton = new Button();
        ListBox rules = new ListBox();

        _ = new RulesControls(_ruleSet, updownStarting,
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

        _updownWidth = new NumericUpDown();
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
        _windowManager.PlaceControl(_buttonLeave, 11, 8, 11, 8);
        _windowManager.PlaceLabel("Description of simulation", 0, 0, 0, 8);


        _windowManager.PlaceLabel("Name", 1, 0, 1, 1);
        _windowManager.PlaceControl(_textboxName, 2, 0, 2, 1);

        _windowManager.PlaceLabel("Width", 1, 2, 1, 3);
        _windowManager.PlaceControl(_updownWidth, 2, 2, 2, 3);

        _windowManager.PlaceLabel("Height", 1, 4, 1, 5);
        _windowManager.PlaceControl(_updownHeight, 2, 4, 2, 5);

        _windowManager.PlaceLabel("Starting", 7, 2, 7, 2);
        _windowManager.PlaceControl(updownStarting, 8, 2, 8, 2);

        _windowManager.PlaceLabel("Counted", 9, 2, 9, 2);
        _windowManager.PlaceControl(updownCounted, 10, 2, 10, 2);

        _windowManager.PlaceLabel("Resulting", 7, 3, 7, 3);
        _windowManager.PlaceControl(updownResulting, 8, 3, 8, 3);

        _windowManager.PlaceLabel("Lower", 9, 3, 9, 3);
        _windowManager.PlaceControl(updownLower, 10, 3, 10, 3);

        _windowManager.PlaceLabel("Upper", 7, 4, 7, 4);
        _windowManager.PlaceControl(updownUpper, 8, 4, 8, 4);

        _windowManager.PlaceLabel("Awlays", 9, 4, 9, 4);
        _windowManager.PlaceControl(isUnconditional, 10, 4, 10, 4);

        _windowManager.PlaceControl(addRuleButton, 11, 2, 11, 3);

        _windowManager.PlaceControl(rules, 7, 0, 11, 1);

        _windowManager.PlaceControl(_buttonAddSimulation, 11, 4, 11, 4);

        _windowManager.PlaceLabel("Logic of simulation", 6, 0, 6, 8);

        _windowManager.PlaceControl(_buttonUsePreset, 7, 6, 7, 7);
        _windowManager.PlaceControl(_comboboxPresets, 8, 6, 8, 7);
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
        //_windowManager.Debug(e.Graphics);
    }
}