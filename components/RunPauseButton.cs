using misery.Eng;

namespace misery.components;

public sealed class RunPauseButton : Button
{
    public RunPauseButton(System.Windows.Forms.Timer timer)
    {
        Text = @"Run";
    }

    protected override void OnClick(EventArgs e)
    {
        if (Settings.DisplayedTimer.Enabled)
        {
            Text = @"Run";
            Settings.DisplayedTimer.Stop();
        }
        else
        {
            Text = @"Pause";
            Settings.DisplayedTimer.Start();
        }
    }
}