using misery.eng.neighborhoods;

namespace misery.eng.automaton;

public class Simulation
{
    private readonly RuleSet? _ruleSet;
    private INeighborhood _neighborhood;
    public int Generation { get ; private set; }

    public Simulation(RuleSet ruleSet, INeighborhood neighborhood)
    {
        _ruleSet = ruleSet;
        _neighborhood = neighborhood;
    }

    public void Advance(Grid readFrom, Grid writeTo, int rows, int columns)
    {
        Generation++;
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < columns; c++)
            {
                var dest = writeTo.ReadState(r, c);
                dest.Value = 0;
                writeTo.SetState(r, c, dest);
            }

        //ClearBuffer(writeTo, rows, columns);
        ApplyRules(readFrom, writeTo, rows, columns);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                var s = writeTo.ReadState(r, c);
                s.Older();
                writeTo.SetState(r, c, s);
            }
        }
    }


    private void ClearBuffer(Grid grid, int rows, int columns)
    {
        for (int row = 0; row < rows; row++) 
        {
            for (int column = 0; column < columns; column++)
            {
                var existing = grid.ReadState(row, column);
                existing.Value = 0;
                grid.SetState(row, column, existing);
            }
        }
                    //grid.SetState(row, column, new State(0));
    }

    private void ApplyRules(Grid readFrom, Grid writeTo, int rows, int columns)
    {
        Parallel.For(0, rows, row =>
        {
            for (int column = 0; column < columns; column++)
            {
                var current = readFrom.ReadState(row, column);
                if (_ruleSet == null) continue;

                List<Condition>? conditions = _ruleSet.GetConditionsForState(current);
                if (conditions == null) continue;

                State destination = writeTo.ReadState(row, column);

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
    public void ChangeNeighborhood(INeighborhood neighborhood) => _neighborhood = neighborhood;
}

