using misery.components.grid;
using misery.eng.automaton;

namespace misery.eng;

public static class Settings
{
        public static int BrushSize = 1, BrushState = 1;
        public static InteractiveGrid? DisplayedGrid;

        public static bool IsViewingActivity;
        public static readonly Dictionary<int, Color> ColorByStateValue = new();

        public static event Action? ColorsChanged;

        public static void ToggleHeatMapButton()
        {
                IsViewingActivity = !IsViewingActivity;
                ColorsChanged?.Invoke();
        }

        public static void SetColorForState(int state, Color color)
        {
                ColorByStateValue[state] = color;
                ColorsChanged?.Invoke();
        }

        public static void SetDefaultColorStatePairs()
        {
                ColorByStateValue.TryAdd(0, Color.Black);
                ColorByStateValue.TryAdd(1, Color.White);
                ColorByStateValue.TryAdd(2, Color.Red);
                ColorsChanged?.Invoke();
        }

        public static Color GetColorByValue(int value)
        {
                return ColorByStateValue.TryGetValue(value, out var color) ? color : Color.Black;
        }

        public static Color GetColorByState(State state)
        {
                return GetColorByValue(state.Value);
        }

        public static void SetBrushSize(int size)
        {
                BrushSize = size;
        }

        public static void SetState(int state)
        {
                BrushState = state;
        }
}