using misery.eng.neighborhoods;

namespace misery.eng.automaton;

public class Simulation
{
        private readonly INeighborhood _neighborhood;
        private readonly RuleSet? _ruleSet;

        public Simulation(RuleSet ruleSet, INeighborhood neighborhood)
        {
                _ruleSet = ruleSet;
                _neighborhood = neighborhood;
        }

        public int Generation { get; private set; }

        public void Advance(Grid readFrom, Grid writeTo, int rows, int columns)
        {
                Generation++;
                for (var r = 0; r < rows; r++)
                for (var c = 0; c < columns; c++)
                {
                        var dest = writeTo.ReadState(r, c);
                        dest.Value = 0;
                        writeTo.SetState(r, c, dest);
                }

                ApplyRules(readFrom, writeTo, rows, columns);

                for (var r = 0; r < rows; r++)
                for (var c = 0; c < columns; c++)
                {
                        var s = writeTo.ReadState(r, c);
                        s.Older();
                        writeTo.SetState(r, c, s);
                }
        }

        private void ApplyRules(Grid readFrom, Grid writeTo, int rows, int columns)
        {
                Parallel.For(0, rows, row =>
                {
                        for (var column = 0; column < columns; column++)
                        {
                                var current = readFrom.ReadState(row, column);
                                if (_ruleSet == null) continue;

                                var conditions = _ruleSet.GetConditionsForState(current);

                                var destination = writeTo.ReadState(row, column);

                                foreach (var condition in conditions)
                                {
                                        var neighbors = _neighborhood.Count(readFrom, condition.Counted,
                                                new Coordinate(row, column), 1);

                                        if (condition.IsUnconditional || condition.IsWithin(neighbors))
                                        {
                                                destination.Value = condition.Resulting.Value;
                                                writeTo.SetState(row, column, destination);
                                        }
                                }
                        }
                });
        }

        public void Reset()
        {
                Generation = 0;
        }
}