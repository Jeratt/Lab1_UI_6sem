using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataLibrary;

namespace ViewModel
{
    public interface IErrorReporter
    {
        void reportError(string message);
    }

    public interface IDialogWindows
    {
        public bool openForLoad(out string filename);
        public bool openForSave(out string filename);
    }

    public interface IRefresher
    {
        public void refreshUi(params ListBox[] lbs);
    }

    public class ViewData : ViewModelBase, IDataErrorInfo
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

        private readonly IErrorReporter errorReporter;
        public readonly IDialogWindows dialogInt;
        //public readonly IRefresher refresher;

        public ICommand ExecuteRawDataFromControlsCommand { get; private set; }
        public ICommand ExecuteRawDataFromFileCommand { get; private set; }
        public ICommand SaveRawDataCommand { get; private set; }

        public string Integral
        {
            get
            {
                return spline?.Integral.ToString() ?? "";
            }
        }

        public string Error
        {
            get { return "Входные данные некорректны!"; }
        }

        public string this[string columnName]
        {
            get
            {
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
        public ObservableCollection<String> RawDataList { get; }
        public ObservableCollection<SplineDataItem> DataItems { get; }

        public OxyPlotModel Plot { get; set; }

        public ViewData(IErrorReporter errorReporter, IDialogWindows dialogInt)
        {
            this.errorReporter = errorReporter;
            this.dialogInt = dialogInt;
            //this.refresher = refresher;
            ExecuteRawDataFromControlsCommand = new RelayCommand(FromCtrls_Click, _ => CanExecuteRawDataFromControlsCommandHandler());
            ExecuteRawDataFromFileCommand = new RelayCommand(FromFile_Click, _ => CanExecuteRawDataFromFileCommandHandler());
            SaveRawDataCommand = new RelayCommand(Save_Click, _ => CanSaveCommandHandler());
            Ders = new double[2];
            RawDataList = new ObservableCollection<string>();
            DataItems = new ObservableCollection<SplineDataItem>();
        }

        public void Save(string filename)
        {
            if (data is null)
            {
                errorReporter.reportError("Nothing to save!");
                return;
            }
            try
            {
                data.Save(filename);
            }
            catch (Exception x)
            {
                errorReporter.reportError("ERROR SAVING FILE ! ! !: {x}");
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
                errorReporter.reportError($"ERROR LOADING FILE ! ! !: {x}");
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

        private bool CanExecuteRawDataFromControlsCommandHandler()
        {
            return String.Empty == this[nameof(Right)] && String.Empty == this[nameof(NodeCnt)] && String.Empty == this[nameof(NodeCntSpline)];
        }
        private bool CanExecuteRawDataFromFileCommandHandler()
        {
            return String.Empty == this[nameof(NodeCntSpline)];
        }
        private bool CanSaveCommandHandler()
        {
            /*
            if (Validation.GetHasError(NodeCnt) || Validation.GetHasError(Right))
            {
                e.CanExecute = false;
            }
            else
                e.CanExecute = true;*/
            return String.Empty == this[nameof(NodeCnt)] && String.Empty == this[nameof(Right)];
        }

        private void FromCtrls_Click(object sender)
        {
            InitRawData();
            InitSpline();
            Interpolate();
            //TextBlockIntegral.Refresh();
            //rawDataLb.Items.Refresh();
            //splineLb.Items.Refresh();
            //exp.UpdateTarget();
            Plot = new OxyPlotModel(spline, data);
            RaisePropertyChanged(nameof(RawDataList));
            RaisePropertyChanged(nameof(DataItems));
            RaisePropertyChanged(nameof(Plot));
            RaisePropertyChanged(nameof(Integral));
            //refresher.refreshUi();
        }

        private void FromFile_Click(object sender)
        {
            string filename;
            if (this.dialogInt.openForLoad(out filename))
            {
                InitRawDataFromFile(filename);
                InitSpline();
                Interpolate();
                Plot = new OxyPlotModel(spline, data);
                RaisePropertyChanged(nameof(RawDataList));
                RaisePropertyChanged(nameof(DataItems));
                RaisePropertyChanged(nameof(Plot));
                RaisePropertyChanged(nameof(Integral));
            }
            else
            {
                errorReporter.reportError(filename);
            }
        }

        private void Save_Click(object sender)
        {
            string filename;
            InitRawData();
            if (dialogInt.openForSave(out filename))
            {
                Save(filename);
            }
            else
            {
                errorReporter.reportError("Error on saving\nraw data!");
            }
        }

        public void RawData_Init()
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
    }
}
