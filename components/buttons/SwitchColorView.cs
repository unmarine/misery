using misery.eng;

namespace misery.components.buttons;

internal class SwitchColorView: Button
{
    protected override void OnClick(EventArgs e)
    {
        Settings.IsViewingActivity = !Settings.IsViewingActivity;
    }
}
