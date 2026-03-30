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

        public void Advance(Grid readFrom, Grid writeTo)
        {
                Generation++;
                
                ResetValues(writeTo);   
                ApplyRules(readFrom, writeTo);
                Olden(writeTo);
        }

        private void ResetValues(Grid grid)
        {
                for (var row = 0; row < grid.Rows; row++)
                for (var column = 0; column < grid.Columns; column++)
                {
                    var dest = grid.ReadState(row, column);
                    dest.Value = 0;
                    grid.SetState(row, column, dest);
                }
        }

        private void Olden(Grid grid)
        {
                for (var row = 0; row < grid.Rows; row++)
                for (var column = 0; column < grid.Rows; column++)
                {
                    var s = grid.ReadState(row, column);
                    s.Older();
                    grid.SetState(row, column, s);
                }
        }

        private void ApplyRules(Grid readFrom, Grid writeTo)
        {
                Parallel.For(0, writeTo.Rows, row =>
                {
                        for (var column = 0; column < writeTo.Columns; column++)
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

        public void Reset() => Generation = 0;
}