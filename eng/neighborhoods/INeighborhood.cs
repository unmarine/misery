using misery.eng.automaton;

namespace misery.eng.neighborhoods;

public interface INeighborhood
{
        public int Count(Grid grid, State state, Coordinate coordinate, int radius);
}