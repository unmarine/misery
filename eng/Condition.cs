namespace misery.Eng;

public struct Condition : IEquatable<Condition>
{
        public State Starting { get; init; }
        public State Counted { get; init; }
        public State Resulting { get; init; }

        public int Min { get; set; }
        public int Max { get; set; }

        public bool IsUnconditional { get; set; } = false;

        public bool IsWithin(int n)
        {
                return n >= Min && n <= Max;
        }

        public Condition(State starting, State counted, State resulting, int min, int max, bool isUnconditional = false)
        {
                Starting = starting;
                Counted = counted;
                Resulting = resulting;
                Min = min;
                Max = max;
                IsUnconditional = isUnconditional;
        }

        public Condition(int starting, int counted, int resulting, int min, int max, bool isUnconditional = false)
        {
                Starting = new State(starting);
                Counted = new State(counted);
                Resulting = new State(resulting);
                Min = min;
                Max = max;
                IsUnconditional = isUnconditional;
        }

        public bool Equals(Condition other)
        {
                return Starting.Equals(other.Starting) && Counted.Equals(other.Counted) &&
                       Resulting.Equals(other.Resulting) && Min == other.Min && Max == other.Max &&
                       IsUnconditional == other.IsUnconditional;
        }

        public override bool Equals(object? obj)
        {
                return obj is Condition other && Equals(other);
        }

        public override int GetHashCode()
        {
                return HashCode.Combine(Starting, Counted, Resulting, Min, Max, IsUnconditional);
        }

        public static bool operator ==(Condition left, Condition right)
        {
                return left.Equals(right);
        }

        public static bool operator !=(Condition left, Condition right)
        {
                return !left.Equals(right);
        }
}