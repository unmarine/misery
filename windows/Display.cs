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

    public Display(Automaton automaton)
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

        _windowManager.PlaceControl(_interactiveGrid, 0, 0, 37, 25);

        _windowManager.PlaceControl(randomizeButton, 0, 25, 1, 27);
        _windowManager.PlaceControl(from, 0, 27, 1, 28);
        _windowManager.PlaceControl(to, 0, 28, 1, 29);
        _windowManager.PlaceControl(_runPauseButton, 1, 25, 2, 27);
    }

    private void OnTick(object? sender, EventArgs e)
    {
        _automaton.Advance();
        _interactiveGrid.Invalidate();
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        _windowManager.Debug(e.Graphics);
    }
}