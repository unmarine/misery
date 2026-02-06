using misery.components;
using misery.Components;
using misery.eng;
using misery.Eng;
using misery.utils;

namespace misery;

public partial class Form1 : Form
{
        private Automaton automaton;
        private VisualGrid visualGrid;
        private System.Windows.Forms.Timer timer;
        private WindowManager wm;

        public Form1()
        {
                ClientSize = new Size(2000, 2000);
                Text = "Automaton Simulation";
                Settings.SetDefaultColorStatePairs();
                Settings.SetColorForState(2, Color.MediumVioletRed);
                Settings.SetColorForState(1, Color.Yellow);
                
                INeighborhood neighborhood = new Moore();
                var dead = new State(0);
                var live = new State(1);
                var dying = new State(2);
                var none = new State(-1);

                var first = new Condition(dead, live, live, 2, 2);
                var second = new Condition(live, none, dying, -1, -1, true);
                var third = new Condition(dying, none, dead, -1, -1, true);

                var BriansBrain = new RuleSet();
                BriansBrain.AddCondition(first);
                BriansBrain.AddCondition(second);
                BriansBrain.AddCondition(third);
                //
                // first = new Condition(live, live, dead, 0, 1);
                // second = new Condition(live, live, live, 2, 3);
                // third = new Condition(live, live, dead, 4, 8);
                // var fourth = new Condition(dead, live, live, 3, 3);
                //
                // RuleSet GameOfLife = new RuleSet();
                // GameOfLife.AddCondition(first);
                // GameOfLife.AddCondition(second);
                // GameOfLife.AddCondition(third);
                // GameOfLife.AddCondition(fourth);
                //
                automaton = new Automaton(neighborhood, 400, 400, BriansBrain);
                
        timer = new System.Windows.Forms.Timer();
                timer.Interval = 1; // Set refresh rate (500 ms
                timer.Tick += Timer_Tick; // Attach tick event
                var b = new RunPauseButton(timer);

                Settings.DisplayedTimer = timer;
                DoubleBuffered = true;
                visualGrid = new VisualGrid(automaton);

                Settings.DisplayedGrid = visualGrid;

                Button randomization = new Button();
                NumericUpDown lower = new NumericUpDown();
                NumericUpDown greater = new NumericUpDown();

                RandomizeControls rdc = new RandomizeControls(automaton, randomization, lower, greater, visualGrid);

                NumericUpDown stateForColor = new NumericUpDown();
                Button buttonForColor = new Button();
                Button submitButton = new Button();
                ListBox dgv = new ListBox();

                ColorPairsController cpc = new ColorPairsController(stateForColor, buttonForColor, submitButton, dgv);

                ClearButton clb = new ClearButton(automaton);
                
                wm = new WindowManager(this, 100, 50);
                wm.PlaceControl(visualGrid, 0, 0, 55, 35);
                wm.PlaceControl(b, 0, 35, 2, 37);
                wm.PlaceControl(randomization, 2, 35, 4, 37);
                wm.PlaceControl(lower, 2, 37, 4, 39);
                wm.PlaceControl(greater, 2, 39, 4, 41);

        wm.PlaceControl(stateForColor, 18, 35, 20, 37);
        wm.PlaceControl(buttonForColor, 18, 37, 20, 39);
        wm.PlaceControl(submitButton, 18, 39,20, 41 );
        wm.PlaceControl(dgv, 20, 35, 55, 41);
        wm.PlaceControl(clb, 4, 35, 6, 37);
    }

        protected override void OnPaint(PaintEventArgs e)
        {
                base.OnPaint(e);

                wm.Debug(e.Graphics);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
                automaton.Advance();
                visualGrid.Invalidate();
        }
}