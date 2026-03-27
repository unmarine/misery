using misery.components.grid;

namespace misery.components.pathfinding;

public class SetEndButton : Button
{
    private readonly InteractiveGrid interactiveGrid;

    public SetEndButton(InteractiveGrid interactiveGrid)
    {
        this.interactiveGrid = interactiveGrid;
        Text = "Set End";
    }

    protected override void OnClick(EventArgs e)
    =>
        interactiveGrid.CurrentMode = InteractiveGridMode.SetEnd;
}
