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
    public class ViewData : IDataErrorInfo
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

        public double Integral {
            get
            {
                if (spline is null)
                    return 0;
                return spline.Integral;
            } 
        }

        /*
        public string? SplineIntegral
        {
            get { return spline?.Integral.ToString(); }
        }*/

        //public string SplineIntegral { get; set; }

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
        public List<String> RawDataList { get; }
        public List<SplineDataItem> DataItems { get; }

        public ViewData() {
            Ders = new double[2];
            RawDataList = new List<string>();
            DataItems = new List<SplineDataItem>();
        }

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
            catch (Exception x)
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
            catch (Exception x)
            {
                MessageBox.Show($"ERROR LOADING FILE ! ! !: {x}");
            }
        }

        public void InitRawData()
        {
            switch (Func)
            {
                case FRawEnum.FRawLinear:
                    data = new RawData(Left, Right, NodeCnt, IsUniform, RawData.FRawLinear);
                    break;
                case FRawEnum.FRawCubic:
                    data = new RawData(Left, Right, NodeCnt, IsUniform, RawData.FRawCubic);
                    break;
                case FRawEnum.FRawRandom:
                    data = new RawData(Left, Right, NodeCnt, IsUniform, RawData.FRawRandom);
                    break;
            }
        }

        public void InitRawDataFromFile(string filename)
        {
            data = new RawData(filename);
        }

        public void InitSpline()
        {
            if (data is null)
                throw new ArgumentNullException();
            spline = new SplineData(data, Ders, NodeCntSpline);
        }

        public void Interpolate()
        {
            if (spline is null || data is null)
                throw new ArgumentNullException();
            spline.Interpolate();
            RawDataList.Clear();
            DataItems.Clear();
            for (int i = 0; i < data.NodeCnt; ++i)
            {
                RawDataList.Add($"Point: {string.Format("{0:f3}", data.Grid[i])};" +
                    $" Value: {string.Format("{0:f3}", data.Field[i])}");
            }
            for (int i = 0; i < spline.NodeCnt; ++i)
            {
                DataItems.Add(spline.DataItems[i]);
            }
        }
    }
}
