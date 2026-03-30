using misery.components.grid;

namespace misery.components.pathfinding;

public class SetEndButton : Button
{
        private readonly InteractiveGrid _interactiveGrid;

        public SetEndButton(InteractiveGrid interactiveGrid)
        {
                _interactiveGrid = interactiveGrid;
                Text = @"Set End";
        }

        protected override void OnClick(EventArgs e)
        {
                _interactiveGrid.CurrentMode = InteractiveGridMode.SetEnd;
                _interactiveGrid.Invalidate();
        }
}