namespace misery.eng.automaton;

public struct State : IEquatable<State>
{
    public int Value { get; set; }

    public State(int value) => Value = value;
    public State() => Value = 0;

    private int transitions = 0;
    private int lastValueSnapshot = 0;
    public void Older()
    {
        int current = (Value == 0) ? 0 : 1;
        if (current != lastValueSnapshot)
        {
            transitions++;
            lastValueSnapshot = current;
        }
    }
    public int GetAge()
    {
        return transitions;
    }

    public double GetNormalizedIndex(int maxExpectedTransitions = 100)
    {
        if (maxExpectedTransitions <= 0) return 0.0;
        return Math.Min(1.0, (double)transitions / maxExpectedTransitions);
    }

    public bool Equals(State other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is State other && Equals(other);
    public override int GetHashCode() => Value;
    public static bool operator ==(State left, State right) => left.Equals(right);
    public static bool operator !=(State left, State right) => !left.Equals(right);
}