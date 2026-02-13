using misery.eng;

namespace misery.Eng;

public class Automaton
{
    private RuleSet _ruleSet;
    private INeighborhood _neighborhood;

    public int Columns, Rows;

    private bool _isExploitingBufferA = true;
    private Grid _bufferA;
    private Grid _bufferB;


    public Grid GetReadyGrid()
    {
        return _isExploitingBufferA ? _bufferA : _bufferB;
    }
    public Grid GetExploitedGrid()
    {
        return _isExploitingBufferA ? _bufferA : _bufferB;
    }

    public void ForceState(int row, int column, State state)
    {
        _bufferA.SetState(row, column, state);
        _bufferB.SetState(row, column, state);
    }
    public Grid GetUnexploitedGrid()
    {
        return _isExploitingBufferA ? _bufferB : _bufferA;
    }

    public Automaton(INeighborhood neighborhood, int height, int width, RuleSet ruleSet)
    {
        _neighborhood = neighborhood;
        _ruleSet = ruleSet;
        Columns = width;
        Rows = height;
        _bufferA = new Grid(Rows, Columns);
        _bufferB = new Grid(Rows, Columns);
    }

    public void ChangeNeighborhood(INeighborhood neighborhood)
    {
        _neighborhood = neighborhood;
    }

    public void Advance()
    {
        var readFrom = _isExploitingBufferA ? _bufferB : _bufferA;
        var writeTo = _isExploitingBufferA ? _bufferA : _bufferB;

        for (int row = 0; row < Rows; row++)
            for (int column = 0; column < Columns; column++)
            {
                writeTo.SetState(row, column, new State(0));
            }

        Parallel.For(0, Rows, row =>
        {
            for (int column = 0; column < Columns; column++)
            {
                var current = readFrom.ReadState(row, column);

                var conditions = _ruleSet.GetConditionsForState(current);

                foreach (var condition in conditions)
                {
                    var neighbors = _neighborhood.Count(readFrom, condition.Counted,
                                    new Coordinate(row, column), 1);

                    if (condition.IsUnconditional)
                        writeTo.SetState(row, column, condition.Resulting);
                    else if (condition.IsWithin(neighbors))
                        writeTo.SetState(row, column, condition.Resulting);
                }
            }
        });

        _isExploitingBufferA = !_isExploitingBufferA;

    }

    public void Clear()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                ForceState(row, column, new State(0));
            }
        }
    }

    public void Randomize(int lowest, int greatest)
    {
        var random = new Random();
        for (int row = 0; row < Rows; row++)
            for (int column = 0; column < Columns; column++)
            {
                var value = random.Next(lowest, greatest + 1);
                ForceState(row, column, new State(value));
            }

    }
}