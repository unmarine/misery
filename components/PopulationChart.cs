using System.Windows.Forms.DataVisualization.Charting;
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
        
        this.ChartAreas.Add(mainArea);

        Settings.ColorsChanged += OnColorsChanged;

        _automaton.GridCleared += () =>
        {
            this.BeginInvoke(() =>
            {
                foreach (Series s in this.Series)
                {
                    s.Points.Clear();
                }
            });
        };
    }

    private void OnColorsChanged()
    {
        if (this.IsHandleCreated)
        {
            this.BeginInvoke(() =>
            {
                foreach (Series series in this.Series)
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
        this.BeginInvoke(() =>
        {
            foreach (var pair in states)
            {
                if (pair.Key.Value == 0) continue; 
                if (generation < 20) continue;
                string stateName = $"State {pair.Key.Value}";

                if (this.Series.FindByName(stateName) == null)
                {
                    AddState(stateName, Settings.GetColorByState(pair.Key));
                }
                UpdatePopulation(stateName, generation, pair.Value);
            }
        });
    }
    public void AddState(string stateName, Color color, bool useSecondaryAxis = false)
    {
        if (this.Series.IsUniqueName(stateName))
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
                this.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
            }

            this.Series.Add(series);
        }
    }

    public void UpdatePopulation(string stateName, int generation, double population)
    {
        if (this.Series.IndexOf(stateName) != -1)
        {
            this.Series[stateName].Points.AddXY(generation, population);
        }
    }
}