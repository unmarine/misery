using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using misery.components;
using misery.Components;
using misery.Eng;
using misery.utils;

namespace misery.windows;
public class Display: Form
{
    Automaton _automaton;

    WindowManager _windowManager;

    System.Windows.Forms.Timer _timer;

    ClearButton _clearButton;
    ColorPairsController _colorPairsController;
    InteractiveGrid _interactiveGrid;
    RandomizeControls _randomizeControls;
    RunPauseButton _runPauseButton;

    public Display(Automaton automaton, SimulationManager simulation)
    {
        _automaton = automaton;
        ClientSize = Screen.PrimaryScreen!.Bounds.Size;
        Text = "Viewing Simulation";
        Settings.SetDefaultColorStatePairs();
        Settings.SetColorForState(2, Color.Yellow);
        DoubleBuffered = true;

        _timer = new System.Windows.Forms.Timer();
        _timer.Interval = 1;
        _timer.Tick += OnTick;
        Settings.DisplayedTimer = _timer;

        _runPauseButton = new RunPauseButton(_timer);

        _windowManager = new WindowManager(this, 40, 40);

        _interactiveGrid = new InteractiveGrid(_automaton);
        Settings.DisplayedGrid = _interactiveGrid;

        Button randomizeButton = new Button();
        NumericUpDown from = new NumericUpDown();
        NumericUpDown to = new NumericUpDown();
        _randomizeControls = new RandomizeControls(_automaton, randomizeButton, from, to, _interactiveGrid);
    

        NumericUpDown stateForColor = new NumericUpDown();
        Button colorSelectionButton = new Button();
        Button addColorButton = new Button();
        ListBox colorPairsList = new ListBox();
        _colorPairsController = new ColorPairsController(stateForColor, colorSelectionButton, addColorButton, colorPairsList);

        ClearButton clearButton = new ClearButton(_automaton);

        ChangeFormButton returnToOverview = new ChangeFormButton(this, new Overview(simulation));

        _windowManager.PlaceControl(_interactiveGrid, 0, 0, 37, 23);

        _windowManager.PlaceControl(randomizeButton, 0, 24, 1, 25);
        _windowManager.PlaceControl(from, 2, 24, 2, 24);
        _windowManager.PlaceControl(to, 2, 25, 2, 25);
        _windowManager.PlaceControl(_runPauseButton, 0, 26, 1, 27);
        _windowManager.PlaceControl(returnToOverview, 0, 38, 1, 39);

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
        //_windowManager.Debug(e.Graphics);
    }
}