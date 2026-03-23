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
        ClearBuffer(writeTo, rows, columns);
        ApplyRules(readFrom, writeTo, rows, columns);
    }


    private void ClearBuffer(Grid grid, int rows, int columns)
    {
        for (int row = 0; row < rows; row++)
            for (int column = 0; column < columns; column++)
                grid.SetState(row, column, new State(0));
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

                foreach (var condition in conditions)
                {
                    var neighbors = _neighborhood.Count(readFrom, condition.Counted,
                        new Coordinate(row, column), 1);

                    if (condition.IsUnconditional || condition.IsWithin(neighbors))
                        writeTo.SetState(row, column, condition.Resulting);
                }
            }
        });
    }

    public void Reset() => Generation = 0;
    public void ChangeNeighborhood(INeighborhood neighborhood) => _neighborhood = neighborhood;
}

