using misery.components;
using misery.eng;
using misery.Eng;
using misery.utils;

namespace misery.windows;
public sealed class Setup : Form
{
    WindowManager _windowManager;
    SimulationManager _simulationManager;

    RuleSet? _ruleSet = new RuleSet("Unknown");

    RulesController _rulesController;
    
    Button _addSimulation = new Button();

    ComboBox presets = new ComboBox();
    Button usePreset = new Button();
    
    NumericUpDown width, height;
    
    public Setup(SimulationManager simulationManager)
    {
        Height = 900; Width = 1000;
        Text = @"Create Custom Automaton";
        _windowManager = new WindowManager(this, 12, 16);
        _simulationManager = simulationManager;

        Label b = new Label() {Text = @"Starting"};
        NumericUpDown starting = new NumericUpDown();

        Label c = new Label();
        c.Text = @"Counted";
        NumericUpDown counted = new NumericUpDown();

        Label d = new Label();
        d.Text = @"Resulting";
        NumericUpDown resulting = new NumericUpDown();

        Label e = new Label();
        e.Text = @"Lower";
        NumericUpDown lower = new NumericUpDown();

        Label f = new Label();
        f.Text = @"Upper";
        NumericUpDown upper = new NumericUpDown();
        
        CheckBox isUnconditional = new CheckBox();
        Button addRuleButton = new Button();
        ListBox rules = new ListBox();

        _rulesController = new RulesController(_ruleSet, starting, counted, resulting, lower, upper, isUnconditional, addRuleButton, rules);

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
        
        _windowManager.PlaceControl(widthLabel, 0, 7, 0 ,7);
        _windowManager.PlaceControl(width, 1, 7, 1, 7);
        _windowManager.PlaceControl(heightLabel, 0, 8, 0, 8);
        _windowManager.PlaceControl(height, 1, 8, 1, 8);
        
        _windowManager.PlaceControl(b, 0, 0, 0, 0);
        _windowManager.PlaceControl(starting, 1, 0, 1, 0);

        _windowManager.PlaceControl(c, 0, 1, 0, 1);
        _windowManager.PlaceControl(counted, 1, 1, 1, 1);

        _windowManager.PlaceControl(d, 0, 2, 0, 2);
        _windowManager.PlaceControl(resulting, 1, 2, 1, 2);

        _windowManager.PlaceControl(e, 0, 3, 0, 3);
        _windowManager.PlaceControl(lower, 1, 3, 1, 3);

        _windowManager.PlaceControl(f, 0,4,0,4);
        _windowManager.PlaceControl(upper, 1, 4, 1, 4);

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
        Automaton automaton = new Automaton(new Moore(), height, width, selected);
        _simulationManager.AddSimulation(automaton);
        Hide();
        var o = new Overview(_simulationManager);
        o.Show();
        o.FormClosed += (s, e) => Close();
    }
    private void addSimulation(object? sender, EventArgs e)
    {
        int width = (int)this.width.Value;
        int height = (int)this.height.Value;
        Automaton automaton = new Automaton(new Moore(), height, width, _ruleSet);
        _simulationManager.AddSimulation(automaton);
        Hide();
        var o = new Overview(_simulationManager);
        o.Show();
        o.FormClosed += (s,_) => Close();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        // _windowManager.Debug(e.Graphics);
    }
}

