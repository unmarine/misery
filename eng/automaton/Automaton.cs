using misery.eng.neighborhoods;
using misery.eng.pathfinding;
using Timer = System.Windows.Forms.Timer;

namespace misery.eng.automaton;

public class Automaton
{
        public readonly int Columns, Rows;
        private Dictionary<State, int> _quantityOfStates = new();
        public Timer? Clock;

        public readonly DoubleBuffer DoubleBuffer;
        private readonly string _name;
        public Pathfinding PathFinder = new DijkstraSearch();

        public List<Dictionary<State, int>> Records = new();
        private readonly Simulation _simulation;


        public Automaton(INeighborhood neighborhood, int height, int width, RuleSet ruleSet, string name)
        {
                _simulation = new Simulation(ruleSet, neighborhood);
                Columns = width;
                _name = name;
                Rows = height;
                DoubleBuffer = new DoubleBuffer(Rows, Columns);
                _name = name == "" ? ruleSet.ToString() : name;
        }

        public Coordinate PathStart { get; set; } = new(-1, -1);
        public Coordinate PathEnd { get; set; } = new(-1, -1);

        public event Action? GridUpdated;
        public event Action? GridCleared;
        public event Action<int, Dictionary<State, int>>? GenerationAdvanced;

        public override string ToString()
        {
                return _name;
        }

        public void Advance()
        {
                _simulation.Advance(DoubleBuffer.ReadBuffer, DoubleBuffer.WriteBuffer, Rows, Columns);
                DoubleBuffer.Swap();

                var counts = new Dictionary<State, int>();
                for (var row = 0; row < Rows; row++)
                for (var column = 0; column < Columns; column++)
                {
                        var s = DoubleBuffer.WriteBuffer.ReadState(row, column);
                        counts.TryAdd(s, 0);
                        counts[s]++;
                }

                _quantityOfStates = counts;
                Records.Add(counts);

                GridUpdated?.Invoke();
                GenerationAdvanced?.Invoke(_simulation.Generation, _quantityOfStates);
        }

        public void Clear()
        {
                for (var row = 0; row < Rows; row++)
                for (var column = 0; column < Columns; column++)
                        DoubleBuffer.ForceState(row, column, new State(0));
                _quantityOfStates.Clear();

                _simulation.Reset();
                GridCleared?.Invoke();
                GridUpdated?.Invoke();
        }

        public void Randomize(int lowest, int greatest)
        {
                var random = new Random();
                for (var row = 0; row < Rows; row++)
                for (var column = 0; column < Columns; column++)
                {
                        var value = random.Next(lowest, greatest + 1);
                        DoubleBuffer.ForceState(row, column, new State(value));
                        if (_quantityOfStates.ContainsKey(new State(value)))
                                _quantityOfStates[new State(value)]++;
                        else _quantityOfStates.Add(new State(value), 1);
                }

                GridUpdated?.Invoke();
        }
}