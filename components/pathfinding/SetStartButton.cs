namespace misery.components.pathfinding;

public class SetStartButton(InteractiveGrid interactiveGrid): Button
{
    protected override void OnClick(EventArgs e)
    =>
        interactiveGrid.CurrentMode = InteractiveGridMode.SetStart;
}
