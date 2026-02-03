 namespace misery.Eng;

 public static class Settings
 {
     private static readonly Dictionary<int, Color> ColorByStateValue = new Dictionary<int, Color>();


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
 }
 