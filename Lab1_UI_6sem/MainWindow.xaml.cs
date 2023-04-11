using DataLibrary;
using Lab1_UI_6sem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab1_UI_6sem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OxyPlotModel plot;
        //public DerConverter derConverter = new DerConverter();
        public ViewData viewData { get; set; }
        public List<String> RawDataList { get; }
        public string IntegralValue { get => viewData?.spline?.Integral.ToString() ?? ""; }
        /*
        public List<SplineDataItem> SplineDataList
        {
            get { return viewData?.spline?.DataItems; }
        }   
        */
        public List<String> SplineDataList
        {
            get;
        }
        public static RoutedCommand ExecuteRawDataFromControlsCommand = new RoutedCommand("ExecuteRawDataFromControlsCommand", typeof(MainWindow));
        public static RoutedCommand ExecuteRawDataFromFileCommand = new RoutedCommand("ExecuteRawDataFromFileCommand", typeof(MainWindow));
        public BindingExpression exp;
        public MainWindow()
        {
            viewData = new ViewData();
            RawDataList = new List<string>();
            SplineDataList = new List<String>();
            InitializeComponent();
            exp = TextBlockIntegral.GetBindingExpression(TextBlock.TextProperty);
            DataContext = viewData;
            this.CommandBindings.Add(new CommandBinding(ExecuteRawDataFromControlsCommand, FromCtrls_Click, CanExecuteRawDataFromControlsCommandHandler));
            this.CommandBindings.Add(new CommandBinding(ExecuteRawDataFromFileCommand, FromFile_Click, CanExecuteRawDataFromFileCommandHandler));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Save_Click, CanSaveCommandHandler));

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            viewData.InitRawData();
            Microsoft.Win32.SaveFileDialog saver = new Microsoft.Win32.SaveFileDialog();
            if ((bool)saver.ShowDialog() && viewData != null)
            {
                viewData.Save(saver.FileName);
                MessageBox.Show("Raw data\n saved successful!");
            }
            else
            {
                MessageBox.Show("Error on saving\nraw data!");
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void FromCtrls_Click(object sender, RoutedEventArgs e)
        {
            viewData.InitRawData();
            viewData.InitSpline();
            viewData.Interpolate();
            //TextBlockIntegral.Refresh();
            rawDataLb.Items.Refresh();
            splineLb.Items.Refresh();
            exp.UpdateTarget();
            plot = new OxyPlotModel(viewData.spline, viewData.data);
            main_plot.Model = plot.plotModel;
        }

        private void FromFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog loader = new Microsoft.Win32.OpenFileDialog();
            try {
                if ((bool)loader.ShowDialog())
                {
                    viewData.InitRawDataFromFile(loader.FileName);
                    viewData.InitSpline();
                    viewData.Interpolate();
                    rawDataLb.Items.Refresh();
                    splineLb.Items.Refresh();
                    exp.UpdateTarget();
                    plot = new OxyPlotModel(viewData.spline, viewData.data);
                    main_plot.Model = plot.plotModel;
                }
                else
                {
                    MessageBox.Show("Error on loading\n raw data!");
                }
            }
            catch(Exception x) {
                MessageBox.Show("Wrong data read\n from file!!!");
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public void RawData_Init()
        {
            switch (viewData.Func) {
                case FRawEnum.FRawLinear:
                    viewData.data = new RawData(viewData.Left, viewData.Right, viewData.NodeCnt, viewData.IsUniform, RawData.FRawLinear);
                    break;
                case FRawEnum.FRawCubic:
                    viewData.data = new RawData(viewData.Left, viewData.Right, viewData.NodeCnt, viewData.IsUniform, RawData.FRawCubic);
                    break;
                case FRawEnum.FRawRandom:
                    viewData.data = new RawData(viewData.Left, viewData.Right, viewData.NodeCnt, viewData.IsUniform, RawData.FRawRandom);
                    break;
            }
        }

        private void CanExecuteRawDataFromControlsCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (Validation.GetHasError(Right) || Validation.GetHasError(NodeCnt) || Validation.GetHasError(NodeCntSpline))
                e.CanExecute = false;
            else
                e.CanExecute = true;
        }
        private void CanExecuteRawDataFromFileCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (Validation.GetHasError(NodeCntSpline))
                e.CanExecute = false;
            else
                e.CanExecute = true;
        }
        private void CanSaveCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (Validation.GetHasError(NodeCnt) || Validation.GetHasError(Right))
            {
                e.CanExecute = false;
            }
            else
                e.CanExecute = true;
        }
    }
}

namespace Converters
{
    public class DerConverter : IMultiValueConverter
    {
        /*
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ViewData)
            {
                return ((ViewData)value).Left.ToString() + "#" + ((ViewData)value).Right.ToString();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        */

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values[0].ToString() + "#" + values[1].ToString();
        }
        /*
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {

            }
            else
            {
                throw new NotImplementedException();
            }
        }
        */
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                if (!((string)value).Contains('#'))
                {
                    return new object[] { 0f, 0f };
                }
                int i = 0;
                double[] vals = new double[2];
                foreach (string x in ((string)value).Split('#'))
                {

                    if(!double.TryParse(x, out vals[i]))
                        vals[i] = 0f;
                    ++i;
                }
                if (i == 2)
                {
                    return new object[] { vals[0], vals[1] };
                }
                else
                {
                    return new object[] { 0f, 0f };
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
    
    public class ComboBoxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((FRawEnum)value)
            {
                case FRawEnum.FRawLinear:
                    return "Linear";
                case FRawEnum.FRawCubic:
                    return "Cube";
                case FRawEnum.FRawRandom:
                    return "Random";
                default:
                    return "Random";
            }
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FRawEnum f = FRawEnum.FRawRandom;
            switch (((System.Windows.Controls.ContentControl)value).Content)
            {
                case "Random":
                    f = FRawEnum.FRawRandom;
                    break;
                case "Cube":
                    f = FRawEnum.FRawCubic;
                    break;
                case "Linear":
                    f = FRawEnum.FRawLinear;
                    break;
            }
            return f;
        }
    }

    public class NullStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return "";
            }
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class DerConverterSingle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double[]))
                throw new NotImplementedException();
            return ((double[])value)[0].ToString() + "#" + ((double[])value)[1].ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!((string)value).Contains('#'))
            {
                return new double[2]{ 0f, 0f };
            }
            int i = 0;
            double[] vals = new double[2];
            foreach (string x in ((string)value).Split('#'))
            {

                if (!double.TryParse(x, out vals[i]))
                    vals[i] = 0f;
                ++i;
            }
            if (i == 2)
            {
                return new double[2] { vals[0], vals[1] };
            }
            else
            {
                return new double[2] { 0f, 0f };
            }
        }
    }
}
