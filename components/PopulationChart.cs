using System.Windows.Forms.DataVisualization.Charting;
using misery.eng.automaton;
using misery.Eng;

namespace misery.components;

public class PopulationChart : Chart
{
    private readonly Automaton _automaton;

    public PopulationChart(Automaton automaton)
    {
        _automaton = automaton;
        _automaton.GenerationAdvanced += OnGenerationAdvanced;
        BackColor = Color.Black;

        ChartArea mainArea = new ChartArea("MainArea");

        mainArea.AxisX.Title = "Generation";
        mainArea.AxisX.TitleForeColor = Color.White;
        mainArea.AxisX.Minimum = 0;

        mainArea.BackColor = Color.Black;

        mainArea.Position = new ElementPosition(0, 0, 100, 100);
        mainArea.InnerPlotPosition = new ElementPosition(0, 0, 100, 100);

        mainArea.AxisX.IsMarginVisible = false;
        mainArea.AxisY.IsMarginVisible = false;

        mainArea.AxisX.LabelStyle.IsEndLabelVisible = true;
        mainArea.AxisX.LabelStyle.Angle = 0;
        mainArea.AxisY.LabelStyle.Angle = 0;

        mainArea.AxisY.MajorGrid.LineColor = Color.LightGray;

        ChartAreas.Add(mainArea);

        Settings.ColorsChanged += OnColorsChanged;

        _automaton.GridCleared += () =>
        {
            BeginInvoke(() =>
            {
                foreach (Series s in Series)
                {
                    s.Points.Clear();
                }
            });
        };
    }

    private void OnColorsChanged()
    {
        if (IsHandleCreated)
        {
            BeginInvoke(() =>
            {
                foreach (Series series in Series)
                {
                    if (int.TryParse(series.Name.Replace("State ", ""), out int stateValue))
                    {
                        series.Color = Settings.GetColorByState(stateValue);
                    }
                }
            });
        }
    }
    private void OnGenerationAdvanced(int generation, Dictionary<State, int> states)
    {
        BeginInvoke(() =>
        {
            foreach (var pair in states)
            {
                if (pair.Key.Value == 0) continue;
                if (generation < 20) continue;
                string stateName = $"State {pair.Key.Value}";

                if (Series.FindByName(stateName) == null)
                {
                    AddState(stateName, Settings.GetColorByState(pair.Key));
                }
                UpdatePopulation(stateName, generation, pair.Value);
            }
        });
    }

    public void Actualize()
    {
        int i = 0;
        foreach (var record in _automaton.Records)
        {
            foreach (var pair in record)
            {
                if (pair.Key.Value == 0) continue;
                string stateName = $"State {pair.Key.Value}";

                if (Series.FindByName(stateName) == null)
                {
                    AddState(stateName, Settings.GetColorByState(pair.Key));
                }
                UpdatePopulation(stateName, i, pair.Value);
                i++;
            }
        }
    }

    public void AddState(string stateName, Color color, bool useSecondaryAxis = false)
    {
        if (Series.IsUniqueName(stateName))
        {
            Series series = new Series(stateName)
            {
                ChartType = SeriesChartType.FastLine,
                BorderWidth = 2,
                Color = color,
                IsXValueIndexed = true,
                XValueType = ChartValueType.Int32
            };

            if (useSecondaryAxis)
            {
                series.YAxisType = AxisType.Secondary;
                ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
            }

            Series.Add(series);
        }
    }

    public void UpdatePopulation(string stateName, int generation, double population)
    {
        if (Series.IndexOf(stateName) != -1)
        {
            Series[stateName].Points.AddXY(generation, population);
        }
    }
}