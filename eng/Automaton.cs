using misery.eng;

namespace misery.Eng;

public class Automaton
{
        private RuleSet _ruleSet;
        private INeighborhood _neighborhood;
        public Grid TheGrid { get; set; }

        public Automaton(INeighborhood neighborhood, int height, int width, RuleSet ruleSet)
        {
                _neighborhood = neighborhood;
                _ruleSet = ruleSet;
                TheGrid = new Grid(height, width);
        }

        public void ChangeNeighborhood(INeighborhood neighborhood)
        {
                _neighborhood = neighborhood;
        }

        public void Advance()
        {
                var update = new Grid(TheGrid.Rows, TheGrid.Columns);

                for (var row = 0; row < TheGrid.Rows; row++)
                for (var column = 0; column < TheGrid.Columns; column++)
                {
                        var current = TheGrid.ReadState(row, column);

                        var conditions = _ruleSet.GetConditionsForState(current);

                        foreach (var condition in conditions)
                        {
                                var neighbors = _neighborhood.Count(TheGrid, condition.Counted,
                                        new Coordinate(row, column), 1);

                                if (condition.IsUnconditional)
                                        update.SetState(row, column, condition.Resulting);
                                else if (condition.IsWithin(neighbors))
                                        update.SetState(row, column, condition.Resulting);
                        }
                }

                TheGrid = update;
        }

        public void Randomize(int lowest, int greatest)
        {
                var random = new Random();
                for (var row = 0; row < TheGrid.Rows; row++)
                for (var column = 0; column < TheGrid.Columns; column++)
                {
                        var value = random.Next(lowest, greatest + 1);

                        TheGrid.SetState(row, column, new State(value));
                }
        }
}