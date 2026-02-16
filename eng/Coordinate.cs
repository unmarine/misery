namespace misery.Eng;

public struct Coordinate(int row, int column) : IEquatable<Coordinate>
{
    public int Column { get; set; } = column;
    public int Row { get; set; } = row;

    public bool Equals(Coordinate other)
    {
        return Column == other.Column && Row == other.Row;
    }

    public override bool Equals(object? obj)
    {
        return obj is Coordinate other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Column, Row);
    }

    public static bool operator ==(Coordinate left, Coordinate right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Coordinate left, Coordinate right)
    {
        return !left.Equals(right);
    }
}