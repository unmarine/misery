using misery.eng.automaton;
using misery.eng.pathfinding;

namespace misery.components.combiners
{
    public class PathfindingControls
    {
        public PathfindingControls(ComboBox pathfinders, Automaton automaton, Button selectPathFinder)
        {
            pathfinders.Items.Add(new AStarSearch());
            pathfinders.Items.Add(new DijkstraSearch());

            selectPathFinder.Text = "Select Pathfinder";
            selectPathFinder.Click += (s, e) =>
            {
                Pathfinding? pathfinding = pathfinders.SelectedItem as Pathfinding;
                if (pathfinding == null) return;
                automaton.PathFinder = pathfinding;
            };

        }
    }
}
