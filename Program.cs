using misery.eng;
using misery.Eng;
using misery.utils;
using misery.windows;

namespace misery;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        Eng.INeighborhood neighborhood = new Moore();


        var automaton = new Automaton(neighborhood, 400, 400, Presets.BriansBrain());
        SimulationManager simulation = new SimulationManager();
        automaton.Randomize(0, 1);
        // simulation.AddSimulation(automaton);
        automaton = new Automaton(neighborhood, 400, 400, Presets.GameOfLife());
        automaton.Randomize(0, 1);
        // simulation.AddSimulation (automaton);

        var timer = new System.Windows.Forms.Timer();
        timer.Interval = 1;

        Application.Run(new Overview(simulation));
    }
}