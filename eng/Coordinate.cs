namespace misery.Eng;

public struct Coordinate
{
    public int Column { get; set; }
    public int Row { get; set; }

    public Coordinate(int row, int column)
    {
        Row = row;
        Column = column;
    }
}