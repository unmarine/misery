using misery.eng.automaton;

namespace misery.eng.neighborhoods;

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
                        || row == coordinate.Row && column == coordinate.Column) continue;
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
    public int Count(Grid grid, State state, Coordinate coordinate, int radius = 1)
    {
        var left = grid.ReadState(coordinate.Row, coordinate.Column - 1);
        var middle = grid.ReadState(coordinate);
        var right = grid.ReadState(coordinate.Row, coordinate.Column + 1);

        return left.Value << 2 | middle.Value << 1 | right.Value;
    }

    public override string ToString()
    {
        return "Elementary";
    }
}

public class VonNeumann : INeighborhood
{
    public int Count(Grid grid, State state, Coordinate coordinate, int radius = 1)
    {
        var count = 0;
        
        void check(int row, int column)
        {
            if ( !grid.IsInside(new Coordinate(row, column))
                 || row == coordinate.Row && column == coordinate.Column) return;
            if (grid.ReadState(row, column).Equals(state)) count++;
        }

        for (int row = coordinate.Row; row < coordinate.Row + radius; row++) check(row, coordinate.Column); // up
        for (int row = coordinate.Row - radius; row < coordinate.Row; row++) check(row, coordinate.Column); // down
        for (int column = coordinate.Column - radius; column < coordinate.Column; column++) check(column, coordinate.Column); // left
        for (int column = coordinate.Column + radius; column > coordinate.Column; column--) check(column, coordinate.Column); // right

        return count;
    }

    public override string ToString()
    {
        return "VonNeumann";
    }
}