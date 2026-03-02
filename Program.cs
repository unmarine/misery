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

        SimulationManager simulation = new SimulationManager();

        Application.Run(new Overview(simulation));
    }
}