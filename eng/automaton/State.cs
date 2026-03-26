namespace misery.eng.automaton;

public struct State : IEquatable<State>
{
    public int Value { get; set; }

    public State(int value) => Value = value;
    public State() => Value = 0;

    private int lastAlive;          // 0/1 snapshot
    private double activity;        // 0..1 activity level

    private const double DecayFactor = 0.95; // per generation

    public void Older()
    {
        int currentAlive = (Value == 0) ? 0 : 1;
        if (currentAlive != lastAlive)
        {
            activity = 1.0;        // spike on transition
            lastAlive = currentAlive;
        }
        else
        {
            activity *= DecayFactor; // decay when no transition
            if (activity < 1e-6) activity = 0.0;
        }
    }

    // expose 0..1 index for coloring
    public double GetNormalizedIndex(int _ = 1) => activity;

    // optional: raw for diagnostics
    public double GetActivity() => activity;

    public bool Equals(State other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is State other && Equals(other);
    public override int GetHashCode() => Value;
    public static bool operator ==(State left, State right) => left.Equals(right);
    public static bool operator !=(State left, State right) => !left.Equals(right);
}