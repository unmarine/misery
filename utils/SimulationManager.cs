using misery.eng.automaton;

namespace misery.utils;

public class SimulationManager
{
        public readonly List<Automaton> Simulations = [];

        public void AddSimulation(Automaton automaton)
        {
                Simulations.Add(automaton);
        }
}