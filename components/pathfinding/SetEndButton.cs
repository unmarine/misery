namespace misery.components.pathfinding;

public class SetEndButton(InteractiveGrid interactiveGrid) : Button
{
    protected override void OnClick(EventArgs e)
    =>
        interactiveGrid.CurrentMode = InteractiveGridMode.SetEnd;
}
