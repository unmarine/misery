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
        var timer = new System.Windows.Forms.Timer();
        timer.Interval = 1;

        Application.Run(new Overview(simulation));
    }
}