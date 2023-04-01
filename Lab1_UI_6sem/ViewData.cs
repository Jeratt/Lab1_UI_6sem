using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DataLibrary;

namespace Lab1_UI_6sem
{
    public class ViewData
    {
        public double Left { get; set; }
        public double Right { get; set; }
        public int NodeCnt { get; set; }
        public bool IsUniform { get; set; }
        public FRawEnum Func { get; set; }
        public double LeftDer { get; set; }
        public double RightDer { get; set; }
        public double[] Ders { get; set; }
        public int NodeCntSpline { get; set; }

        /*
        public string? SplineIntegral
        {
            get { return spline?.Integral.ToString(); }
        }*/
        
        public string SplineIntegral { get; set; }

        public RawData? data;
        public SplineData? spline;
        public ViewData() { }

        public void Save(string filename)
        {
            if (data is null)
            {
                MessageBox.Show("Nothing to save!");
                return;
            }
            try
            {
                data.Save(filename);
            }
            catch(Exception x)
            {
                MessageBox.Show($"ERROR SAVING FILE ! ! !: {x}");
            }
        }

        public void Load(string filename)
        {
            try
            {
                RawData.Load(filename, out data);
            }
            catch(Exception x)
            {
                MessageBox.Show($"ERROR LOADING FILE ! ! !: {x}");
            }
        }
    }
}
