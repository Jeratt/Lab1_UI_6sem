using DataLibrary;
using OxyPlot;
using OxyPlot.Legends;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lab1_UI_6sem
{
    class OxyPlotModel
    {
        public PlotModel plotModel { get; private set; }
        SplineData data;
        public OxyPlotModel(SplineData data)
        {
            this.data = data;
            this.plotModel = new PlotModel { Title = "Spline Interpolation result" };
            AddSeries();
        }
        public void AddSeries()
        {
            this.plotModel.Series.Clear();
            Legend legend = new Legend();
            LineSeries lineSeries = new LineSeries();
            for (int js = 0; js < data.NodeCnt; js++)
            {
                OxyColor color = (js == 0) ? OxyColors.Green : OxyColors.Blue;
                lineSeries.Points.Add(new DataPoint(data.DataItems[js].Coord, data.DataItems[js].Spline));
                lineSeries.Color = color;

                lineSeries.MarkerType = MarkerType.Circle;
                lineSeries.MarkerSize = 4;
                lineSeries.MarkerStroke = color;
                lineSeries.MarkerFill = color;
                lineSeries.Title = "Cubic spline interpolation";
            }
            plotModel.Legends.Add(legend);
            this.plotModel.Series.Add(lineSeries);
        }
    }
}
