using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace misery.components;

public class PopulationChart : Chart
{
    public PopulationChart()
    {
        this.Dock = DockStyle.Fill;
        this.BackColor = Color.White;

        ChartArea mainArea = new ChartArea("MainArea");
        
        mainArea.AxisX.Title = "Generation";
        mainArea.AxisX.MajorGrid.LineColor = Color.LightGray;
        mainArea.AxisX.Minimum = 0;

        mainArea.AxisY.MajorGrid.LineColor = Color.LightGray;
        
        this.ChartAreas.Add(mainArea);

        Legend legend = new Legend("States") { Docking = Docking.Bottom };
        this.Legends.Add(legend);
    }

    public void AddState(string stateName, Color color, bool useSecondaryAxis = false)
    {
        if (this.Series.IsUniqueName(stateName))
        {
            Series series = new Series(stateName)
            {
                ChartType = SeriesChartType.StepLine,
                BorderWidth = 2,
                Color = color,
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