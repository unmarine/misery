using misery.components.combiners;
using misery.eng;
using misery.eng.automaton;
using misery.utils;

using misery.eng.neighborhoods;

namespace misery.windows;
public sealed class Setup : Form
{
    WindowManager _windowManager;
    SimulationManager _simulationManager;

    RuleSet? _ruleSet = new RuleSet("Unknown");

    Button _addSimulation = new Button();

    ComboBox presets = new ComboBox();
    Button usePreset = new Button();
    
    TextBox textboxName = new TextBox();

    NumericUpDown width, height;
    
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

        _ = new RulesController(_ruleSet, updownStarting, updownCounted, updownResulting, updownLower, updownUpper, isUnconditional, addRuleButton, rules);

        _addSimulation.Click += addSimulation;
        _addSimulation.Text = @"Add Simulation";

        presets = new ComboBox();
        usePreset = new Button();
        usePreset.Text = @"Use Preset";
        usePreset.Click += addPreset;
        
        foreach (var ruleSet in Presets.All)
        {
            presets.Items.Add(ruleSet);
        }
        
        Label widthLabel = new Label {Text = @"Width"};
        width = new NumericUpDown();
        
        Label heightLabel = new Label {Text = @"Height"};
        height = new NumericUpDown();
        
        width.Minimum = 1;
        width.Maximum = 1;
        width.Maximum = Decimal.MaxValue;
        height.Maximum = Decimal.MaxValue;
        width.Value = 300;
        height.Value = 300;

        _windowManager.PlaceControl(textboxName, 0, 9, 0, 11);
        

        _windowManager.PlaceControl(widthLabel, 0, 7, 0 ,7);
        _windowManager.PlaceControl(width, 1, 7, 1, 7);
        _windowManager.PlaceControl(heightLabel, 0, 8, 0, 8);
        _windowManager.PlaceControl(height, 1, 8, 1, 8);
        
        _windowManager.PlaceControl(labelStarting, 0, 0, 0, 0);
        _windowManager.PlaceControl(updownStarting, 1, 0, 1, 0);

        _windowManager.PlaceControl(labelCounted, 0, 1, 0, 1);
        _windowManager.PlaceControl(updownCounted, 1, 1, 1, 1);

        _windowManager.PlaceControl(labelResulting, 0, 2, 0, 2);
        _windowManager.PlaceControl(updownResulting, 1, 2, 1, 2);

        _windowManager.PlaceControl(labelLower, 0, 3, 0, 3);
        _windowManager.PlaceControl(updownLower, 1, 3, 1, 3);

        _windowManager.PlaceControl(labelUpper, 0,4,0,4);
        _windowManager.PlaceControl(updownUpper, 1, 4, 1, 4);

        _windowManager.PlaceControl(isUnconditional, 0, 5, 0, 5);
        _windowManager.PlaceControl(addRuleButton, 1, 5, 1, 5);
        _windowManager.PlaceControl(rules, 2, 0, 11, 5);
      
        _windowManager.PlaceControl(_addSimulation, 0, 12, 0, 15);
        
        _windowManager.PlaceControl(presets, 1, 12, 1, 12);
        _windowManager.PlaceControl(usePreset, 1, 13 , 1, 15);
    }

    private void addPreset(object? sender, EventArgs e)
    {
        RuleSet? selected = presets.SelectedItem as RuleSet;
        if (selected == null) return;
        int width = (int)this.width.Value;
        int height = (int)this.height.Value;
        
        string name = textboxName.Text;

        Automaton automaton = new Automaton(new Moore(), height, width, selected, name);

        System.Windows.Forms.Timer timer = new();
        timer.Interval = 1;
        timer.Tick += (s, e) =>
        {
            automaton.Advance();
        };
        automaton.Clock = timer;

        _simulationManager.AddSimulation(automaton);
        var o = new Overview(_simulationManager);
        WindowManager.MoveForms(this, o);
    }
    private void addSimulation(object? sender, EventArgs e)
    {
        int width = (int)this.width.Value;
        int height = (int)this.height.Value;
        string name = textboxName.Text;

        Automaton automaton = new Automaton(new Moore(), height, width, _ruleSet, name);

        System.Windows.Forms.Timer timer = new();
        timer.Interval = 1;
        timer.Tick += (s, e) =>
        {
            automaton.Advance();
        };
        automaton.Clock = timer;

        _simulationManager.AddSimulation(automaton);
        var overview = new Overview(_simulationManager);
        WindowManager.MoveForms(this, overview);
    }



    protected override void OnPaint(PaintEventArgs e)
    {
         _windowManager.Debug(e.Graphics);
    }
}

