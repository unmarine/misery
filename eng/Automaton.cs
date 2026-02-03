using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using misery.eng;

namespace misery.Eng;

public class Automaton
{
        private RuleSet _ruleSet;
        private INeighborhood _neighborhood;
        public Grid TheGrid { get; private set; }
        
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
                Grid update = new Grid(TheGrid.Rows, TheGrid.Columns);

                for (int row = 0; row < TheGrid.Rows; row++)
                {
                        for (int column = 0; column < TheGrid.Columns; column++)
                        {
                                State current = TheGrid.ReadState(row, column);

                                var conditions = _ruleSet.GetConditionsForState(current);

                                foreach (Condition condition in conditions)
                                {
                                        int neighbors = _neighborhood.Count(TheGrid, condition.Counted,
                                                new Coordinate(row, column), 1);

                                        if (condition.IsUnconditional)
                                        {
                                                update.SetState(row, column, condition.Resulting);
                                        } else if (condition.IsWithin(neighbors))
                                        {
                                                update.SetState(row, column, condition.Resulting);
                                        }
                                }
                        }
                }

                TheGrid = update;
        }

        public void Randomize(int lowest, int greatest)
        {
                Random random = new Random();
                for (int row = 0; row < TheGrid.Rows; row++)
                {
                        for (int column = 0; column < TheGrid.Columns; column++)
                        {
                                int value = random.Next(lowest, greatest + 1);

                                TheGrid.SetState(row, column, new State(value));
                        }
                }
        }

        private static ConsoleColor FromColor(Color color)
        {
                int index = (color.R > 128 | color.G > 128 | color.B > 128) ? 8 : 0; 
                index |= (color.R > 64) ? 4 : 0; 
                index |= (color.G > 64) ? 2 : 0; 
                index |= (color.B > 64) ? 1 : 0; 
                return (ConsoleColor)index;
        }

        public void DebugDisplay()
        {
                for (int row = 0; row < TheGrid.Rows; row++)
                {
                        for (int column = 0; column < TheGrid.Columns; column++)
                        {
                                State currentState = TheGrid.ReadState(row, column);
                                Color currentColor = Settings.GetColorByState(currentState);

                                Console.BackgroundColor = FromColor(currentColor);
                                Console.Write(" ");
                                Console.BackgroundColor = ConsoleColor.Black;
                        }

                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.WriteLine();
                }
        }
}