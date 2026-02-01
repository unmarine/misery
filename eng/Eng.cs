 namespace misery.Eng; 
 
 public class Coordinate
 {
     public int Column { get; set; }
     public int Row { get; set; }

     public Coordinate(int row, int column)
     {
         Row = row;
         Column = column;
     }
 }

 public struct State
 {
     public int Value { get; set; }
     public State(int value) => Value = value;
     public State() => Value = 0;

     public override bool Equals(object? obj)
     {
         if (!(obj is State) || obj == null) return false;


         State state = (State)obj;
         return state.Value == Value;
     }
 }

 public class Grid
 {
     public int Rows { get; }
     public int Columns { get; }

     State[,] grid;

     public Grid(int rows, int columns)
     {
         Rows = rows;
         Columns = columns;
         grid = new State[rows, columns];
         for (int row = 0; row < rows; row++)
         for (int column = 0; column < columns; column++)
         {
             grid[row, column] = new State(0);
         }
     }

     public State ReadState(Coordinate coordinate) => grid[coordinate.Row, coordinate.Column];
     public State ReadState(int row, int column) => grid[row, column];

     public void SetState(int row, int column, State state) => grid[row, column] = state;
     public void SetState(Coordinate coordinate, State state) => SetState(coordinate.Row, coordinate.Column, state);

     public bool IsInside(int row, int column) => row < Rows && column < Columns && column >= 0 && row >= 0;
     public bool IsInside(Coordinate coordinate) => IsInside(coordinate.Row, coordinate.Column);
 }

 public interface Neighborhood
 {
     public int Count(Grid grid, State state, Coordinate coordinate, int radius);
     public int GreatestCount(int radius);
 }

 public class Moore : Neighborhood
 {

     public int Count(Grid grid, State state, Coordinate coordinate, int radius = 1)
     {
         int count = 0;

         for (int row = coordinate.Row - radius; row <= coordinate.Row + radius; row++)
         for (int column = coordinate.Column - radius; column <= coordinate.Column + radius; column++)
         {
             if (
                 !grid.IsInside(new Coordinate(row, column))
                 || (row == coordinate.Row && column == coordinate.Column)) continue;
             if (grid.ReadState(row, column).Equals(state)) count++;
         }

         return count;
     }

     public int GreatestCount(int radius)
     {
         return (2 * radius + 1) * (2 * radius + 1) - 1;
     }
 }

 public class Elementary : Neighborhood
 {
     public int Count(Grid grid, State state, Coordinate coordinate, int radius)
     {
         State left = grid.ReadState(coordinate.Row, coordinate.Column - 1);
         State middle = grid.ReadState(coordinate);
         State right = grid.ReadState(coordinate.Row, coordinate.Column + 1);

         return (left.Value << 2) | (middle.Value << 1) | right.Value;
     }

     public int GreatestCount(int radius)
     {
         // no idea. Who cares.
         return 0;
     }
 }

 public class VonNeumann : Neighborhood
 {
     public int Count(Grid grid, State state, Coordinate coordinate, int radius)
     {
         int count = 0;




         return count;
     }

     public int GreatestCount(int radius)
     {
         return 0;
     }
 }

 public struct Condition
 {
     public State Starting { get; init; }
     public State Counted { get; init; }
     public State Resulting { get; init; }
     public int Amount { get; set; }

     public Condition(State starting, State counted, State resulting, int amount)
     {
         Starting = starting;
         Counted = counted;
         Resulting = resulting;
         Amount = amount;
     }

     public Condition(int starting, int counted, int resulting, int amount)
     {
         Starting = new State(starting);
         Counted = new State(counted);
         Resulting = new State(resulting);
         Amount = amount;
     }
 }

 public class Automaton
 {
     private Neighborhood neighborhood;
     public Grid TheGrid { get; set; }

     public Automaton(Neighborhood neighborhood, int height, int width)
     {
         this.neighborhood = neighborhood;
         TheGrid = new Grid(height, width);
     }

     public void Advance()
     {
         Grid update = new Grid(TheGrid.Rows, TheGrid.Columns);

         for (int row = 0; row < TheGrid.Rows; row++)
         {
             for (int column = 0; column < TheGrid.Columns; column++)
             {
                 State current = TheGrid.ReadState(row, column);

                 foreach (Condition condition in Settings.GetConditionsForState(current))
                 {
                     if (condition.Amount != -1)
                     {
                         if (neighborhood.Count(TheGrid, condition.Counted, new Coordinate(row, column), 1) ==
                             condition.Amount)
                         {
                             update.SetState(row, column, condition.Resulting);
                         }
                     }
                     else update.SetState(row, column, condition.Resulting);
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
         int index = (color.R > 128 | color.G > 128 | color.B > 128) ? 8 : 0; // Bright bit
         index |= (color.R > 64) ? 4 : 0; // Red bit
         index |= (color.G > 64) ? 2 : 0; // Green bit
         index |= (color.B > 64) ? 1 : 0; // Blue bit
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

 public static class Settings
 {
     public static bool Wrap = true;
     private static Dictionary<int, Color> ColorByStateValue = new Dictionary<int, Color>();

     private static Dictionary<State, List<Condition>>
         ConditionsForState = new Dictionary<State, List<Condition>>();

     public static void SetColorForState(int state, Color color) => ColorByStateValue[state] = color;
     public static void SetColorForState(State state, Color color) => ColorByStateValue[state.Value] = color;

     public static void SetDefaultColorStatePairs()
     {
         ColorByStateValue.Add(0, Color.Black);
         ColorByStateValue.Add(1, Color.White);
     }

     public static Color GetColorByState(int value)
     {
         return ColorByStateValue[value];
     }

     public static Color GetColorByState(State state)
     {
         return ColorByStateValue[state.Value];
     }

     public static void AddCondition(Condition condition)
     {
         if (ConditionsForState.ContainsKey(condition.Starting))
         {
             ConditionsForState[condition.Starting].Add(condition);
         }
         else
         {
             ConditionsForState.Add(condition.Starting, new List<Condition>());
             AddCondition(condition);
         }
     }

     public static void AddCondition(State starting, State counted, State resulting, int amount)
     {
         AddCondition(new Condition(starting, counted, resulting, amount));
     }


     public static void RemoveCondition(Condition condition) =>
         ConditionsForState[condition.Starting].Remove(condition);

     public static void AddConditionRangedInclusive(State starting, State counted, State resulting, int start,
         int end)
     {
         int up = Math.Max(start, end);
         int down = Math.Min(start, end);
         for (int amount = down; amount <= up; amount++)
             AddCondition(new Condition(starting, counted, resulting, amount));
     }

     public static void AddConditionRangedInclusive(int starting, int counted, int resulting, int start, int end)
     {
         AddConditionRangedInclusive(new State(starting), new State(counted), new State(resulting), start, end);
     }

     public static List<Condition> GetConditionsForState(State state)
     {
         return ConditionsForState[state];
     }

     public static List<Condition> GetConditionsForState(int state)
     {
         return ConditionsForState[new State(state)];
     }
 }
 