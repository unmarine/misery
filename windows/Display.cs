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

    ClearButton _clearButton;
    ColorPairsController _colorPairsController;
    InteractiveGrid _interactiveGrid;
    RandomizeControls _randomizeControls;
    RunPauseButton _runPauseButton;

    public Display(Automaton _automaton)
    {
        ClientSize = (Screen.PrimaryScreen.Bounds.Size);
        Text = "Viewing Simulation";
        Settings.SetDefaultColorStatePairs();
        DoubleBuffered = true;


        _windowManager = new WindowManager(this, 40, 40);

        _interactiveGrid = new InteractiveGrid(_automaton);
        Settings.DisplayedGrid = _interactiveGrid;

        Button randomizeButton = new Button();
        NumericUpDown from = new NumericUpDown();
        NumericUpDown to = new NumericUpDown();
        RandomizeControls randomizeControls = new RandomizeControls(_automaton, randomizeButton, from, to, _interactiveGrid);
    

        NumericUpDown stateForColor = new NumericUpDown();
        Button colorSelectionButton = new Button();
        Button addColorButton = new Button();
        ListBox colorPairsList = new ListBox();
        ColorPairsController colorPairsController = new ColorPairsController(stateForColor, colorSelectionButton, addColorButton, colorPairsList);

        ClearButton clearButton = new ClearButton(_automaton);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        _windowManager.Debug(e.Graphics);
    }
}




//wm = new WindowManager(this, 100, 50);
//wm.PlaceControl(visualGrid, 0, 0, 55, 35);
//                wm.PlaceControl(b, 0, 35, 2, 37);
//                wm.PlaceControl(randomization, 2, 35, 4, 37);
//                wm.PlaceControl(lower, 2, 37, 4, 39);
//                wm.PlaceControl(greater, 2, 39, 4, 41);

//        wm.PlaceControl(stateForColor, 18, 35, 20, 37);
//        wm.PlaceControl(buttonForColor, 18, 37, 20, 39);
//        wm.PlaceControl(submitButton, 18, 39,20, 41 );
//        wm.PlaceControl(dgv, 20, 35, 55, 41);
//        wm.PlaceControl(clb, 4, 35, 6, 37);
