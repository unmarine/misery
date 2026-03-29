using misery.eng.automaton;
using misery.utils;

namespace misery.windows;

public class Overview : Form
{
        private readonly SimulationManager _simulationManager;
        private readonly Button _buttonSelectSimulation;


        private readonly ComboBox _comboboxListOfSimulations;

        public Overview(SimulationManager simulationManager)
        {
                _simulationManager = simulationManager;

                ClientSize = new Size(600, 100);
                
                Text = @"Overview of simulations";
                DoubleBuffered = true;
                
                var windowManager = new WindowManager(this, 3, 2);

                _buttonSelectSimulation = new Button();
                _buttonSelectSimulation.Text = @"Select";
                _buttonSelectSimulation.Click += OpenSimulation;


                _comboboxListOfSimulations = new ComboBox();
                _comboboxListOfSimulations.Text = @"Choose simulation";

                _comboboxListOfSimulations.Items.Clear();
                if (simulationManager.Simulations.Count > 0)
                        foreach (var t in _simulationManager.Simulations)
                                _comboboxListOfSimulations.Items.Add(t);


                var create = new Button { Text = @"Add simulation" };
                create.Click += (_, _) =>
                {
                        var setupForm = new Setup(_simulationManager);
                        WindowManager.MoveForms(this, setupForm);
                };

                windowManager.PlaceControl(_comboboxListOfSimulations, 0, 0, 0, 1);
                windowManager.PlaceControl(_buttonSelectSimulation, 1, 0, 2, 0);
                windowManager.PlaceControl(create, 1, 1, 2, 1);
        }


        private void OpenSimulation(object? sender, EventArgs e)
        {
                var selected = _comboboxListOfSimulations.SelectedItem;
                if (selected == null) return;

                var selectedSimulation = selected as Automaton;
                WindowManager.MoveForms(this, new Display(selectedSimulation!, _simulationManager));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
                //_windowManager.Debug(e.Graphics);
        }
}