using misery.components;
using misery.eng.automaton;

namespace misery.eng;

public static class Settings
{
    public static int brushSize = 1, brushState = 1;
    public static InteractiveGrid? DisplayedGrid;

    public static event Action? ColorsChanged;
    public static readonly Dictionary<int, Color> ColorByStateValue = new();

    public static void SetColorForState(int state, Color color)
    {
        ColorByStateValue[state] = color;
        ColorsChanged?.Invoke();
    }

    public static void SetColorForState(State state, Color color)
    {
        ColorByStateValue[state.Value] = color;
        ColorsChanged?.Invoke();
    }

    public static void SetDefaultColorStatePairs()
    {
        ColorByStateValue.TryAdd(0, Color.Black);
        ColorByStateValue.TryAdd(1, Color.RebeccaPurple);
        ColorByStateValue.TryAdd(2, Color.Khaki);
        ColorsChanged?.Invoke();
    }

    public static Color GetColorByValue(int value)
    {
        return ColorByStateValue.TryGetValue(value, out var color) ? color : Color.Black;
    }

    public static Color GetColorByState(State state) => GetColorByValue(state.Value);

    public static void SetBrushSize(int size) => brushSize = size;
    public static void SetState(int state) => brushState = state;
    public static void SetState(State state) => brushState = state.Value;

}