 namespace misery.Eng;

 public static class Settings
 {
     public static bool Wrap = true;
     private static readonly Dictionary<int, Color> ColorByStateValue = new Dictionary<int, Color>();

     private static readonly Dictionary<State, List<Condition>>
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
         if (!ConditionsForState.ContainsKey((condition.Starting)))
         {
             ConditionsForState.Add(condition.Starting, new List<Condition>());
         }
         ConditionsForState[condition.Starting].Add(condition);
     }

     public static void AddCondition(State starting, State counted, State resulting, int amount)
     {
         AddCondition(new Condition(starting, counted, resulting, amount, amount));
     }


     public static void RemoveCondition(Condition condition) =>
         ConditionsForState[condition.Starting].Remove(condition);

     public static void AddConditionRangedInclusive(State starting, State counted, State resulting, int start,
         int end)
     {
         int up = Math.Max(start, end);
         int down = Math.Min(start, end);
        AddCondition(new Condition(starting, counted, resulting, down, up));
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
 