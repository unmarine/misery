namespace misery.eng.automaton;

public struct State(int value) : IEquatable<State>
{
        public int Value { get; set; } = value;

        public State() : this(0)
        {
        }

        private int _lastAlive;
        private double _activity;
        private const double DecayFactor = 0.95;

        public void Older()
        {
                var currentAlive = Value == 0 ? 0 : 1;
                if (currentAlive != _lastAlive)
                {
                        _activity = 1.0;
                        _lastAlive = currentAlive;
                }
                else
                {
                        _activity *= DecayFactor;
                        if (_activity < 1e-6) _activity = 0.0;
                }
        }

        public double GetNormalizedIndex(int _ = 1)
        {
                return _activity;
        }

        public bool Equals(State other)
        {
                return Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
                return obj is State other && Equals(other);
        }

        public override int GetHashCode()
        {
                return Value;
        }

        public static bool operator ==(State left, State right)
        {
                return left.Equals(right);
        }

        public static bool operator !=(State left, State right)
        {
                return !left.Equals(right);
        }
}