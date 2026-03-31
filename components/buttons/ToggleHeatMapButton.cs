using misery.eng;

namespace misery.components.buttons;

internal class ToggleHeatMapButton : Button
{
        protected override void OnClick(EventArgs e)
        {
                Settings.ToggleHeatMapButton();
        }
}