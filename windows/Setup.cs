using misery.utils;

namespace misery.windows;
public class Setup : Form
{
    WindowManager _windowManager;
    SimulationManager _simulationManager;

    Button _addSimulation = new Button();

    public Setup(SimulationManager simulationManager)
    {
        Height = 700; Width = 500;
        _windowManager = new WindowManager(this, 12, 24);
        _simulationManager = simulationManager;


    }

    private void addSimulation()
    {

    }
}

