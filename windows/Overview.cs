using misery.Eng;
using misery.utils;

namespace misery.windows;
public class Overview: Form
{
    WindowManager _windowManager;
    SimulationManager _simulationManager;

    ComboBox _listOfSimulations;
    Button _selectSimulationButton;

    public Overview(SimulationManager simulationManager)
    {
        ClientSize = new Size(600,1000);
        Text = "Overview of simulations";
        DoubleBuffered = true;
        _windowManager = new WindowManager(this, 10, 10);

        _selectSimulationButton = new Button();
        _selectSimulationButton.Text = "Select";
        _selectSimulationButton.Click += OpenSimulation;

        _simulationManager = simulationManager;

        _listOfSimulations = new ComboBox();
        _listOfSimulations.Text = "Choose simulation";

        for (int i = 0; i < _simulationManager.Simulations.Count; i++)
        {
            _listOfSimulations.Items.Add(i);
        }

        _windowManager.PlaceControl(_listOfSimulations, 0, 0, 2, 8);
        _windowManager.PlaceControl(_selectSimulationButton, 0, 8, 1, 9);
    }

    private void OpenSimulation(object? sender, EventArgs e)
    {
        if (sender == null) return;

        var selected = _listOfSimulations.SelectedItem;
        if (selected == null) return;

        int simulationIndex = (int)selected;
        if (simulationIndex < 0 || simulationIndex > _simulationManager.Simulations.Count) return;

        Display display = new Display(_simulationManager.Simulations[simulationIndex]);
        display.Show();
        Hide();
        display.FormClosed += (s, e) => Close();


    }
    protected override void OnPaint(PaintEventArgs e)
    {
        _windowManager.Debug(e.Graphics);
    }
}

