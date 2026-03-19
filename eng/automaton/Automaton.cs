using misery.eng.neighborhoods;
using misery.eng.pathfinding;
using misery.Eng;

namespace misery.eng.automaton;

public class Automaton
{
    private readonly RuleSet? _ruleSet;
    private INeighborhood _neighborhood;

    private int _generation;

    private Dictionary<State, int> _quantityOfStates = new Dictionary<State, int>();
    public List<Dictionary<State, int>> Records = new();

    public event Action? GridUpdated;
    public event Action? GridCleared;
    public event Action<int, Dictionary<State, int>>? GenerationAdvanced;

    public Pathfinding PathFinder = new DijkstraSearch();
    //public event Action PathChanged;

    public string Name;

    public System.Windows.Forms.Timer? Clock;


    public List<Coordinate> Path { get; private set; } = new();
    public Coordinate PathStart { get; set; } = new Coordinate(-1, -1);
    public Coordinate PathEnd { get; set; } = new Coordinate(-1, -1);

    public readonly int Columns, Rows;

    private bool _isExploitingBufferA = true;
    private readonly Grid _bufferA;
    private readonly Grid _bufferB;


    public Grid GetReadyGrid()
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

    public Automaton(INeighborhood neighborhood, int height, int width, RuleSet? ruleSet, string name)
    {
        _neighborhood = neighborhood;
        _ruleSet = ruleSet;
        Columns = width;
        Name = name;
        Rows = height;
        _bufferA = new Grid(Rows, Columns);
        _bufferB = new Grid(Rows, Columns);
    }

    public void ChangeNeighborhood(INeighborhood neighborhood)
    {
        _neighborhood = neighborhood;
    }

    public override string ToString()
    {
        if (Name == "") return _ruleSet!.ToString();
        else return Name;
    }

    public void Advance()
    {
        _generation++;

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

                if (_ruleSet == null) continue;
                List<Condition>? conditions = _ruleSet.GetConditionsForState(current);
                if (conditions == null) continue;
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

        var counts = new Dictionary<State, int>();
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                State s = writeTo.ReadState(row, column);
                counts.TryAdd(s, 0);
                counts[s]++;
            }
        }

        _quantityOfStates = counts;
        Records.Add(counts);

        _isExploitingBufferA = !_isExploitingBufferA;
        GridUpdated?.Invoke();
        GenerationAdvanced?.Invoke(_generation, _quantityOfStates);
    }

    public void Clear()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                ForceState(row, column, new State(0));
                _quantityOfStates.Clear();
            }
        }

        _generation = 0;
        GridCleared?.Invoke();
        GridUpdated?.Invoke();
    }

    public void Randomize(int lowest, int greatest)
    {
        var random = new Random();
        for (int row = 0; row < Rows; row++)
            for (int column = 0; column < Columns; column++)
            {
                var value = random.Next(lowest, greatest + 1);
                ForceState(row, column, new State(value));
                if (_quantityOfStates.ContainsKey(new State(value)))
                    _quantityOfStates[new State(value)]++;
                else _quantityOfStates.Add(new State(value), 1);
            }
        GridUpdated?.Invoke();
    }
}