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
        private readonly Automaton _automaton;

        private readonly InteractiveGrid _interactiveGrid;

        private readonly WindowManager _windowManager;

        public Display(Automaton automaton, SimulationManager simulation)
        {
                _automaton = automaton;
                ClientSize = Screen.PrimaryScreen!.Bounds.Size;
                
                Text = @"Viewing Simulation";
                BackColor = Color.FromArgb(255, 38, 38, 38);
                DoubleBuffered = true;
                
                Settings.SetDefaultColorStatePairs();
                Settings.SetColorForState(2, Color.Yellow);

                var runPauseButton = new RunPauseButton(_automaton);
                runPauseButton.Actualize();

                _windowManager = new WindowManager(this, 40, 40);

                _interactiveGrid = new InteractiveGrid(_automaton);

                var buttonRandomize = new Button();
                var updownLowerBound = new NumericUpDown();
                var updownUpperBound = new NumericUpDown();
                _ = new RandomizeControls(_automaton, buttonRandomize, updownLowerBound, updownUpperBound);


                var stateForColor = new NumericUpDown();
                var colorSelectionButton = new Button();
                var addColorButton = new Button();
                var colorPairsList = new ListBox();
                _ = new ColorPairsControls(stateForColor, colorSelectionButton, addColorButton, colorPairsList);

                var buttonClear = new ClearButton(_automaton);

                var buttonToOverview = new Button { Text = @"Overview" };
                buttonToOverview.Click += (_, _) =>
                {
                        var overview = new Overview(simulation);
                        WindowManager.MoveForms(this, overview);
                };

                var buttonSetStart = new SetStartButton(_interactiveGrid);

                var buttonSetEnd = new SetEndButton(_interactiveGrid);


                var pc = new PopulationChart(automaton);
                pc.Actualize();

                ComboBox pathfinders = new();
                Button selectPathfinder = new();
                var pfc = new PathfindingControls(pathfinders, automaton, selectPathfinder);
                _windowManager.PlaceControl(buttonToOverview, 0, 38, 1, 39);

                var updownBrushSize = new NumericUpDown();
                var updownBrushState = new NumericUpDown();
                var brushControls = new StateInputControls(updownBrushState, updownBrushSize);
                brushControls.Actualize();

                var sclButton = new SwitchColorView { Text = @"Check activity" };
                _windowManager.PlaceControl(sclButton, 0, 34, 1, 35);

                _windowManager.PlaceLabel("Brush Size", 22, 33, 22, 34);
                _windowManager.PlaceControl(updownBrushSize, 23, 33, 23, 34);

                _windowManager.PlaceLabel("Brush State", 22, 31, 22, 32);
                _windowManager.PlaceControl(updownBrushState, 23, 31, 23, 32);


                _windowManager.PlaceControl(pathfinders, 1, 32, 1, 33);
                _windowManager.PlaceControl(selectPathfinder, 0, 32, 0, 33);

                _windowManager.PlaceControl(pc, 7, 24, 21, 39);
                _windowManager.PlaceControl(buttonSetStart, 0, 30, 0, 31);
                _windowManager.PlaceControl(buttonSetEnd, 1, 30, 1, 31);

                _windowManager.PlaceControl(_interactiveGrid, 0, 0, 37, 23);

                _windowManager.PlaceControl(buttonRandomize, 0, 26, 0, 27);
                _windowManager.PlaceControl(updownLowerBound, 1, 26, 1, 26);
                _windowManager.PlaceControl(updownUpperBound, 1, 27, 1, 27);

                _windowManager.PlaceControl(runPauseButton, 0, 24, 1, 25);

                _windowManager.PlaceControl(buttonClear, 0, 28, 1, 29);

                _windowManager.PlaceControl(stateForColor, 22, 24, 36, 28);
                _windowManager.PlaceControl(colorSelectionButton, 22, 29, 22, 29);
                _windowManager.PlaceControl(addColorButton, 22, 30, 22, 30);
                _windowManager.PlaceControl(colorPairsList, 23, 24, 36, 30);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
                // _windowManager.Debug(e.Graphics);
        }
}