using misery.eng.automaton;
using misery.utils;
namespace misery.windows;

public class Overview : Form
{
    WindowManager _windowManager;
    SimulationManager _simulationManager;

    ComboBox comboboxListOfSimulations;
    Button buttonSelectSimulation;

    public Overview(SimulationManager simulationManager)
    {
        ClientSize = new Size(600, 200);
        Text = "Overview of simulations";
        DoubleBuffered = true;
        _windowManager = new WindowManager(this, 10, 10);

        buttonSelectSimulation = new Button();
        buttonSelectSimulation.Text = "Select";
        buttonSelectSimulation.Click += OpenSimulation;

        _simulationManager = simulationManager;

        comboboxListOfSimulations = new ComboBox();
        comboboxListOfSimulations.Text = "Choose simulation";

        
        comboboxListOfSimulations.Items.Clear();
        if (simulationManager.Simulations.Count > 0)
        {
            for (int i = 0; i < _simulationManager.Simulations.Count; i++)
            {
                comboboxListOfSimulations.Items.Add(_simulationManager.Simulations[i]);
            }
        }


        Button create = new Button() { Text = @"Add simulation" };
        create.Click += (_, e) =>
        {
            var setupForm = new Setup(_simulationManager);
            WindowManager.MoveForms(this, setupForm);
        };

        _windowManager.PlaceControl(create, 2, 7, 3, 9);

        _windowManager.PlaceControl(comboboxListOfSimulations, 0, 0, 2, 6);
        _windowManager.PlaceControl(buttonSelectSimulation, 0, 7, 1, 9);
    }


    private void OpenSimulation(object? sender, EventArgs e)
    {
        var selected = comboboxListOfSimulations.SelectedItem;
        if (selected == null) return;

        var selectedSimulation = selected as Automaton;
        Display display = new Display(selectedSimulation!, _simulationManager);
        WindowManager.MoveForms(this, display);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
         //_windowManager.Debug(e.Graphics);
    }
}

