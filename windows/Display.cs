using misery.components;
using misery.Components;
using misery.Eng;
using misery.utils;

namespace misery.windows;
public class Display : Form
{
    Automaton _automaton;

    WindowManager _windowManager;

    System.Windows.Forms.Timer _timer;

    InteractiveGrid _interactiveGrid;

    public Display(Automaton automaton, SimulationManager simulation)
    {
        _automaton = automaton;
        ClientSize = Screen.PrimaryScreen!.Bounds.Size;
        Text = "Viewing Simulation";
        BackColor = Color.Black;
        Settings.SetDefaultColorStatePairs();
        Settings.SetColorForState(2, Color.Yellow);
        DoubleBuffered = true;

        _timer = new System.Windows.Forms.Timer();
        _timer.Interval = 1;
        _timer.Tick += OnTick;
        Settings.DisplayedTimer = _timer;

        RunPauseButton _runPauseButton = new RunPauseButton(_timer);

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

        PathSelectorButton ptb = new PathSelectorButton(_interactiveGrid);

        PopulationChart pc = new PopulationChart(automaton);
        
        _windowManager.PlaceControl(pc, 7, 24, 21, 39);
        _windowManager.PlaceControl(buttonSetStart, 0, 30, 0, 31);
        _windowManager.PlaceControl(buttonSetEnd, 1, 30, 1, 31);
        
        _windowManager.PlaceControl(ptb, 0, 31, 1, 32);
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

    private void OnTick(object? sender, EventArgs e)
    {
        _automaton.Advance();
        _interactiveGrid.Invalidate();
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        // _windowManager.Debug(e.Graphics);
    }
}