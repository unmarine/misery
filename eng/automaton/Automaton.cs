using misery.eng.neighborhoods;
using misery.eng.pathfinding;

namespace misery.eng.automaton;

public class Automaton
{
    public string Name;
    public readonly int Columns, Rows;
    
    public List<Dictionary<State, int>> Records = new();
    private Dictionary<State, int> _quantityOfStates = new Dictionary<State, int>();

    public DoubleBuffer doubleBuffer;
    public Simulation simulation;
    public Pathfinding PathFinder = new DijkstraSearch();
    public System.Windows.Forms.Timer? Clock;
    
    public List<Coordinate> Path { get; private set; } = new();
    public Coordinate PathStart { get; set; } = new Coordinate(-1, -1);
    public Coordinate PathEnd { get; set; } = new Coordinate(-1, -1);

    public event Action? GridUpdated;
    public event Action? GridCleared;
    public event Action<int, Dictionary<State, int>>? GenerationAdvanced;


    public Automaton(INeighborhood neighborhood, int height, int width, RuleSet ruleSet, string name)
    {
        simulation = new Simulation(ruleSet, neighborhood);
        Columns = width;
        Name = name;
        Rows = height;
        doubleBuffer = new DoubleBuffer(Rows, Columns);
        Name = name == "" ? ruleSet.ToString() : name;
    }

    public override string ToString() => Name;

    public void Advance()
    {
        simulation.Advance(doubleBuffer.ReadBuffer, doubleBuffer.WriteBuffer, Rows, Columns);
        doubleBuffer.Swap();

        var counts = new Dictionary<State, int>();
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                State s = doubleBuffer.WriteBuffer.ReadState(row, column);
                counts.TryAdd(s, 0);
                counts[s]++;
            }
        }

        _quantityOfStates = counts;
        Records.Add(counts);

        GridUpdated?.Invoke();
        GenerationAdvanced?.Invoke(simulation.Generation, _quantityOfStates);
    }

    public void Clear()
    {
        for (int row = 0; row < Rows; row++)
            for (int column = 0; column < Columns; column++)
                doubleBuffer.ForceState(row, column, new State(0));
                _quantityOfStates.Clear();

        simulation.Reset();
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
                doubleBuffer.ForceState(row, column, new State(value));
                if (_quantityOfStates.ContainsKey(new State(value)))
                    _quantityOfStates[new State(value)]++;
                else _quantityOfStates.Add(new State(value), 1);
            }
        GridUpdated?.Invoke();
    }
}