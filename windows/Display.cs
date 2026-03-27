using misery.components;
using misery.components.buttons;
using misery.components.combiners;
using misery.eng.automaton;
using misery.eng;
using misery.utils;
using misery.components.pathfinding;
using misery.components.grid;

namespace misery.windows;
public class Display : Form
{
    Automaton _automaton;

    WindowManager _windowManager;

    InteractiveGrid _interactiveGrid;

    public Display(Automaton automaton, SimulationManager simulation)
    {
        _automaton = automaton;
        ClientSize = Screen.PrimaryScreen!.Bounds.Size;
        Text = "Viewing Simulation";
        BackColor = Color.FromArgb(255, 38, 38, 38);
        Settings.SetDefaultColorStatePairs();
        Settings.SetColorForState(2, Color.Yellow);
        DoubleBuffered = true;

        RunPauseButton _runPauseButton = new RunPauseButton(_automaton);
        _runPauseButton.Actualize();

        _windowManager = new WindowManager(this, 40, 40);

        _interactiveGrid = new InteractiveGrid(_automaton);

        Button buttonRandomize = new Button();
        NumericUpDown updownLowerBound = new NumericUpDown();
        NumericUpDown updownUpperBound = new NumericUpDown();
        _ = new RandomizeControls(_automaton, buttonRandomize, updownLowerBound, updownUpperBound, _interactiveGrid);


        NumericUpDown stateForColor = new NumericUpDown();
        Button colorSelectionButton = new Button();
        Button addColorButton = new Button();
        ListBox colorPairsList = new ListBox();
        _ = new ColorPairsControls(stateForColor, colorSelectionButton, addColorButton, colorPairsList);

        ClearButton buttonClear = new ClearButton(_automaton);

        var buttonToOverview = new Button() { Text = @"Overview" };
        buttonToOverview.Click += (sender, e) =>
        {
            var overview = new Overview(simulation);
            WindowManager.MoveForms(this, overview);
        };

        SetStartButton buttonSetStart = new SetStartButton(_interactiveGrid);

        SetEndButton buttonSetEnd = new SetEndButton(_interactiveGrid);


        PopulationChart pc = new PopulationChart(automaton);
        pc.Actualize();

        ComboBox pathfinders = new();
        Button selectPathfinder = new();
        PathfindingControls pfc = new PathfindingControls(pathfinders, automaton, selectPathfinder);
        _windowManager.PlaceControl(buttonToOverview, 0, 38, 1, 39);

        NumericUpDown updownBrushSize = new NumericUpDown();
        NumericUpDown updownBrushState = new NumericUpDown();
        var brushControls = new StateInputControls(updownBrushState, updownBrushSize);
        brushControls.Actualize();

        SwitchColorView sclButton = new SwitchColorView() { Text = "Check activity" };
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

        _windowManager.PlaceControl(_runPauseButton, 0, 24, 1, 25);

        _windowManager.PlaceControl(buttonClear, 0, 28, 1, 29);

        _windowManager.PlaceControl(stateForColor, 22, 24, 36, 28);
        _windowManager.PlaceControl(colorSelectionButton, 22, 29, 22, 29);
        _windowManager.PlaceControl(addColorButton, 22, 30, 22, 30);
        _windowManager.PlaceControl(colorPairsList, 23, 24, 36, 30);

    }

    protected override void OnPaint(PaintEventArgs e)
    {
        _windowManager.Debug(e.Graphics);
    }
}