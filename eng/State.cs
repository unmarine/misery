namespace misery.Eng;

public struct State : IEquatable<State>
{
    public int Value { get; set; }

    public State(int value) => Value = value;
    public State() => Value = 0;
    
    public bool Equals(State other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is State other && Equals(other);
    public override int GetHashCode() => Value;
    public static bool operator ==(State left, State right) => left.Equals(right);
    public static bool operator !=(State left, State right) => !left.Equals(right);
}