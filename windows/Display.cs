using misery.components;
using misery.components.buttons;
using misery.components.combiners;
using misery.components.grid;
using misery.components.pathfinding;
using misery.eng;
using misery.eng.automaton;
using misery.utils;

namespace misery.windows;

public class Display : Form
{
        public Display(Automaton automaton, SimulationManager simulation)
        {
                ClientSize = Screen.PrimaryScreen!.Bounds.Size;
                
                Text = @"Viewing Simulation";
                BackColor = Color.FromArgb(255, 38, 38, 38);
                DoubleBuffered = true;
                
                Settings.SetDefaultColorStatePairs();
                Settings.SetColorForState(2, Color.Yellow);

                var runPauseButton = new RunPauseButton(automaton);
                runPauseButton.Actualize();

                var windowManager = new WindowManager(this, 40, 40);

                var interactiveGrid = new InteractiveGrid(automaton);

                var buttonRandomize = new Button();
                var updownLowerBound = new NumericUpDown();
                var updownUpperBound = new NumericUpDown();
                _ = new RandomizeControls(automaton, buttonRandomize, updownLowerBound, updownUpperBound);


                var stateForColor = new NumericUpDown();
                var colorSelectionButton = new Button();
                var addColorButton = new Button();
                var colorPairsList = new ListBox();
                _ = new ColorPairsControls(stateForColor, colorSelectionButton, addColorButton, colorPairsList);

                var buttonClear = new ClearButton(automaton);

                var buttonToOverview = new Button { Text = @"Overview" };
                buttonToOverview.Click += (_, _) =>
                {
                        var overview = new Overview(simulation);
                        WindowManager.MoveForms(this, overview);
                };

                var buttonSetStart = new SetStartButton(interactiveGrid);

                var buttonSetEnd = new SetEndButton(interactiveGrid);


                var pc = new PopulationChart(automaton);
                pc.Actualize();

                ComboBox pathfinders = new();
                Button selectPathfinder = new();
                _ = new PathfindingControls(pathfinders, automaton, selectPathfinder);
                windowManager.PlaceControl(buttonToOverview, 0, 38, 1, 39);

                var updownBrushSize = new NumericUpDown();
                var updownBrushState = new NumericUpDown();
                var brushControls = new StateInputControls(updownBrushState, updownBrushSize);
                brushControls.Actualize();

                var sclButton = new SwitchColorView { Text = @"Check activity" };
                windowManager.PlaceControl(sclButton, 0, 34, 1, 35);

                windowManager.PlaceLabel("Brush Size", 22, 33, 22, 34);
                windowManager.PlaceControl(updownBrushSize, 23, 33, 23, 34);

                windowManager.PlaceLabel("Brush State", 22, 31, 22, 32);
                windowManager.PlaceControl(updownBrushState, 23, 31, 23, 32);


                windowManager.PlaceControl(pathfinders, 1, 32, 1, 33);
                windowManager.PlaceControl(selectPathfinder, 0, 32, 0, 33);

                windowManager.PlaceControl(pc, 7, 24, 21, 39);
                windowManager.PlaceControl(buttonSetStart, 0, 30, 0, 31);
                windowManager.PlaceControl(buttonSetEnd, 1, 30, 1, 31);

                windowManager.PlaceControl(interactiveGrid, 0, 0, 37, 23);

                windowManager.PlaceControl(buttonRandomize, 0, 26, 0, 27);
                windowManager.PlaceControl(updownLowerBound, 1, 26, 1, 26);
                windowManager.PlaceControl(updownUpperBound, 1, 27, 1, 27);

                windowManager.PlaceControl(runPauseButton, 0, 24, 1, 25);

                windowManager.PlaceControl(buttonClear, 0, 28, 1, 29);

                windowManager.PlaceControl(stateForColor, 22, 24, 36, 28);
                windowManager.PlaceControl(colorSelectionButton, 22, 29, 22, 29);
                windowManager.PlaceControl(addColorButton, 22, 30, 22, 30);
                windowManager.PlaceControl(colorPairsList, 23, 24, 36, 30);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
                // _windowManager.Debug(e.Graphics);
        }
}