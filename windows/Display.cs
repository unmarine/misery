using misery.components;
using misery.components.buttons;
using misery.components.combiners;
using misery.Components;
using misery.eng.automaton;
using misery.Eng;
using misery.utils;

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
        Settings.DisplayedGrid = _interactiveGrid;

        Button randomizeButton = new Button();
        NumericUpDown from = new NumericUpDown();
        NumericUpDown to = new NumericUpDown();
        _ = new RandomizeControls(_automaton, randomizeButton, from, to, _interactiveGrid);


        NumericUpDown stateForColor = new NumericUpDown();
        Button colorSelectionButton = new Button();
        Button addColorButton = new Button();
        ListBox colorPairsList = new ListBox();
        _ = new ColorPairsController(stateForColor, colorSelectionButton, addColorButton, colorPairsList);

        ClearButton clearButton = new ClearButton(_automaton);

        ChangeFormButton returnToOverview = new ChangeFormButton(this, new Overview(simulation));

        Button buttonSetStart = new Button() {Text = @"Set Start"};
        buttonSetStart.Click += (s, e) =>
        {
            _interactiveGrid.CurrentMode = InteractiveGridMode.SetStart;
        };
            
        Button buttonSetEnd = new Button() {Text = @"Set End"};
        buttonSetEnd.Click += (s, e) =>
        {
            _interactiveGrid.CurrentMode = InteractiveGridMode.SetEnd;
        };


        PopulationChart pc = new PopulationChart(automaton);
        pc.Actualize();

        ComboBox pathfinders = new();
        Button selectPathfinder = new();
        PathfindingControl pfc = new PathfindingControl(pathfinders, automaton, selectPathfinder);


        _windowManager.PlaceControl(pathfinders, 0, 32, 0, 33);
        _windowManager.PlaceControl(selectPathfinder, 1, 32, 1, 33);

        _windowManager.PlaceControl(pc, 7, 24, 21, 39);
        _windowManager.PlaceControl(buttonSetStart, 0, 30, 0, 31);
        _windowManager.PlaceControl(buttonSetEnd, 1, 30, 1, 31);
        
        _windowManager.PlaceControl(_interactiveGrid, 0, 0, 37, 23);

        _windowManager.PlaceControl(randomizeButton, 0, 26, 0, 27);
        _windowManager.PlaceControl(from, 1, 26, 1, 26);
        _windowManager.PlaceControl(to, 1, 27, 1, 27);
        
        _windowManager.PlaceControl(_runPauseButton, 0, 24, 1, 25);
        _windowManager.PlaceControl(returnToOverview, 0, 38, 1, 39);

        _windowManager.PlaceControl(clearButton, 0, 28, 1, 29);

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