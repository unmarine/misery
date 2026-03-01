namespace misery.Eng;

public struct Coordinate(int row, int column) : IEquatable<Coordinate>
{
    public int Column { get; set; } = column;
    public int Row { get; set; } = row;

    public bool Equals(Coordinate other) => Column == other.Column && Row == other.Row;
    public override bool Equals(object? obj) => obj is Coordinate other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Column, Row);
    public static bool operator ==(Coordinate left, Coordinate right) => left.Equals(right);
    public static bool operator !=(Coordinate left, Coordinate right) => !left.Equals(right);
}