namespace misery.Eng;

public struct State : IEquatable<State>
{
        public int Value { get; set; }
        public State(int value) => Value = value;
        public State() => Value = 0;

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
