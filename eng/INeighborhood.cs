namespace misery.Eng;

public interface INeighborhood
{
    public int Count(Grid grid, State state, Coordinate coordinate, int radius);
}