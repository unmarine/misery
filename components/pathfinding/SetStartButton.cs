using misery.components.grid;

namespace misery.components.pathfinding;

public class SetStartButton : Button
{
        private readonly InteractiveGrid _interactiveGrid;

        public SetStartButton(InteractiveGrid interactiveGrid)
        {
                _interactiveGrid = interactiveGrid;
                Text = @"Set Start";
        }

        protected override void OnClick(EventArgs e)
        {
                _interactiveGrid.CurrentMode = InteractiveGridMode.SetStart;
        }
}