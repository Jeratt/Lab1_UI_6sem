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
        RawData rawData;
        public OxyPlotModel(SplineData data, RawData rawData)
        {
            this.data = data;
            this.rawData = rawData;
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

// this.plotModel.Series.Clear();
            Legend legend_rd = new Legend();
            LineSeries lineSeries_rd = new LineSeries();
            for (int js = 0; js < rawData.NodeCnt; js++)
            {
                OxyColor color = (js == 0) ? OxyColors.Red : OxyColors.Black;
                lineSeries_rd.Points.Add(new DataPoint(rawData.Grid[js], rawData.Field[js]));
                lineSeries_rd.Color = color;

                lineSeries_rd.MarkerType = MarkerType.Circle;
                lineSeries_rd.MarkerSize = 4;
                lineSeries_rd.MarkerStroke = color;
                lineSeries_rd.MarkerFill = color;
                lineSeries_rd.Title = "Original function";
            }
            plotModel.Legends.Add(legend_rd);
            this.plotModel.Series.Add(lineSeries_rd);
        }
    }
}
