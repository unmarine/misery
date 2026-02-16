namespace misery.Eng;

public class Moore : INeighborhood
{
    public int Count(Grid grid, State state, Coordinate coordinate, int radius = 1)
    {
        var count = 0;

        for (var row = coordinate.Row - radius; row <= coordinate.Row + radius; row++)
            for (var column = coordinate.Column - radius; column <= coordinate.Column + radius; column++)
            {
                if (
                        !grid.IsInside(new Coordinate(row, column))
                        || (row == coordinate.Row && column == coordinate.Column)) continue;
                if (grid.ReadState(row, column).Equals(state)) count++;
            }

        return count;
    } 
    
    public override string ToString()
    {
        return "Moore";
    }
}

public class Elementary : INeighborhood
{
    public int Count(Grid grid, State state, Coordinate coordinate, int radius)
    {
        var left = grid.ReadState(coordinate.Row, coordinate.Column - 1);
        var middle = grid.ReadState(coordinate);
        var right = grid.ReadState(coordinate.Row, coordinate.Column + 1);

        return (left.Value << 2) | (middle.Value << 1) | right.Value;
    }

    public override string ToString()
    {
        return "Elementary";
    }
}

public class VonNeumann : INeighborhood
{
    public int Count(Grid grid, State state, Coordinate coordinate, int radius)
    {
        var count = 0;


        return count;
    }

    public override string ToString()
    {
        return "VonNeumann";
    }
}