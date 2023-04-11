using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public class SplineDataItem
    {
        public double Coord { get; set; }
        public double Spline { get; set; }
        public double FirstDer { get; set; }
        public double SecondDer { get; set; }

        public string Template
        {
            get
            {
                return this.TemplateToString();
            }
        }

        public string Repr
        {
            get { return this.ToString(); }
        }
        public SplineDataItem(double coord, double spline, double firstDer, double secondDer)
        {
            Coord = coord;
            Spline = spline;
            FirstDer = firstDer;
            SecondDer = secondDer;
        }
        public string ToString(string format="{0:f3}") => $"Point: {string.Format(format, Coord)}" +
            $"\nSpline value: {string.Format(format, Spline)}" +
            $"\nFirst Derivative: {string.Format(format, FirstDer)}" +
            $"\nSecond Derivative: {string.Format(format, SecondDer)}\n";

        /*
        public override string ToString() => $"Point: {Coord}\nSpline value: {Spline}" +
            $"\nFirst Derivative: {FirstDer}\nSecond Derivative: {SecondDer}\n";
        */

        public string TemplateToString(string format = "{0:f3}") => $"Point: {string.Format(format, Coord)}" +
            $"\nSpline value: {string.Format(format, Spline)}" +
            $"\nSecond Derivative: {string.Format(format, SecondDer)}\n";

        public override string ToString() => this.ToString();
    }
}
