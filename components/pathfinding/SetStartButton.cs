namespace misery.components.pathfinding;

public class SetStartButton : Button
{
    private readonly InteractiveGrid interactiveGrid;

    public SetStartButton(InteractiveGrid interactiveGrid)
    {
        this.interactiveGrid = interactiveGrid;
        Text = "Set Start";
    }

    protected override void OnClick(EventArgs e)
    =>
        interactiveGrid.CurrentMode = InteractiveGridMode.SetStart;
}
