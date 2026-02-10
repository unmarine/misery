using misery.Eng;

namespace misery.utils;
public class SimulationManager
{
    public readonly List<Automaton> Simulations = [];

    public void AddSimulation(Automaton automaton)
    {
        Simulations.Add(automaton);
    }

    public void AddRandomizedSimulation(Automaton automaton, int lowest, int greatest)
    {
        automaton.Randomize(lowest, greatest);
        Simulations.Add(automaton);
    }
}

