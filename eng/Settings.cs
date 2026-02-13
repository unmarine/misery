using misery.Components;

namespace misery.Eng;

public static class Settings
{
    public static InteractiveGrid DisplayedGrid;
    public static System.Windows.Forms.Timer DisplayedTimer;


    public static readonly Dictionary<int, Color> ColorByStateValue = new();


    public static void SetColorForState(int state, Color color)
    {
        ColorByStateValue[state] = color;
    }

    public static void SetColorForState(State state, Color color)
    {
        ColorByStateValue[state.Value] = color;
    }

    public static void SetDefaultColorStatePairs()
    {
        ColorByStateValue.TryAdd(0, Color.Black);
        ColorByStateValue.TryAdd(1, Color.White);
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