using misery.Components;

namespace misery.components;

public class PathSelectorButton: Button
{
        private InteractiveGrid _interactiveGrid;
        
        public PathSelectorButton(InteractiveGrid interactiveGrid)
        {
                _interactiveGrid = interactiveGrid;
                Text = @"Path";
        }

        protected override void OnClick(EventArgs e)
        {
              _interactiveGrid.CurrentMode = InteractiveGridMode.SetStart;
        }
}