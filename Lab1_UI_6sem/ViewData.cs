using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DataLibrary;

namespace Lab1_UI_6sem
{
    public class ViewData: IDataErrorInfo
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

        public string Error {
            get { return "Входные данные некорректны!"; }
        }

        public string this[string columnName]
        {
            get {
                string result = string.Empty;
                switch (columnName)
                {
                    case "NodeCnt":
                        if (NodeCnt < 2)
                            result = "Интерполируемая функция должна быть измерена\nхотя бы в двух точках!";
                        break;
                    case "NodeCntSpline":
                        if (NodeCntSpline < 2)
                            result = "Интерполяция должна проводиться\nхотя бы в двух точках!";
                        break;
                    case "Right":
                        if (Right <= Left)
                            result = "Правый конец отрезка должен\nбыть больше левого!";
                        break;
                }
                return result;
            }

        }

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
