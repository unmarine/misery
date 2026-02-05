using misery.components;
using misery.Components;
using misery.eng;
using misery.Eng;

namespace misery
{
    // AI GENERATED FOR DEBUG
    public partial class Form1 : Form
    {
        private Automaton automaton;
        private VisualGrid visualGrid;
        private System.Windows.Forms.Timer timer;

        public Form1()
        {
            InitializeComponent2(); // Make sure to call this first in the constructor.

            Settings.SetDefaultColorStatePairs(); // Presumed existing method
            Settings.SetColorForState(2, Color.MediumVioletRed); // Set color for state
            Settings.SetColorForState(1, Color.Yellow); // Set color for state

            INeighborhood neighborhood = new Moore(); // Assuming Moore is defined elsewhere

            State dead = new State(0);
            State live = new State(1);
            State dying = new State(2);
            State none = new State(-1);

            Condition first = new Condition(dead, live, live, 2, 2);
            Condition second = new Condition(live, none, dying, -1, -1, true);
            Condition third = new Condition(dying, none, dead, -1, -1, true);

            RuleSet BriansBrain = new RuleSet();
            BriansBrain.AddCondition(first);
            BriansBrain.AddCondition(second);
            BriansBrain.AddCondition(third);

            first = new Condition(live, live, dead, 0, 1);
            second = new Condition(live, live, live, 2, 3);
            third = new Condition(live, live, dead, 4, 8);
            var fourth = new Condition(dead, live, live, 3, 3);

            RuleSet GameOfLife = new RuleSet();
            GameOfLife.AddCondition(first);
            GameOfLife.AddCondition(second);
            GameOfLife.AddCondition(third);
            GameOfLife.AddCondition(fourth);

            automaton = new Automaton(neighborhood, 90, 90, GameOfLife); // Initialize automaton

            //automaton.Randomize(0, 1); // Presumed method to randomize grid
            
            visualGrid = new VisualGrid(automaton.TheGrid); // Presumed class visualization
            visualGrid.Height = 2000;
            visualGrid.Width = 2000;

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1; // Set refresh rate (500 ms
            timer.Tick += Timer_Tick; // Attach tick event
            RunPauseButton b = new RunPauseButton(timer);
            
            this.Controls.Add(b);
            this.Controls.Add(visualGrid); // Add visual grid to form
            this.DoubleBuffered = true; // Reduce flickering


        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            automaton.Advance(); // Move automaton forward
            visualGrid.ReplaceGrid(automaton.TheGrid); // Update the visual grid
        }


        private void InitializeComponent2()
        {
            // Add standard initialization code here, e.g.:
            this.ClientSize = new Size(2000, 2000);
            this.Text = "Automaton Simulation";
        }
    }
}