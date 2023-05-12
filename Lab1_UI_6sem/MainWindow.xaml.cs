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
using ViewModel;

namespace Lab1_UI_6sem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewData(new MessageBoxErrorReporter(), new DialogWindows());
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }



        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }



        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

    }

    public class MessageBoxErrorReporter : IErrorReporter
    {
        public void reportError(string message)
        {
            MessageBox.Show(message);
        }
    }

    public class DialogWindows: IDialogWindows
    {
        public bool openForSave(out string filename)
        {
            Microsoft.Win32.SaveFileDialog saver = new Microsoft.Win32.SaveFileDialog();
            if ((bool)saver.ShowDialog())
            {
                filename = saver.FileName;
                return true;
            }
            else
            {
                filename = "Error on saving\nraw data!";
                return false;
            }
        }

        public bool openForLoad(out string filename)
        {
            Microsoft.Win32.OpenFileDialog loader = new Microsoft.Win32.OpenFileDialog();
            try
            {
                if ((bool)loader.ShowDialog())
                {
                    filename = loader.FileName;
                    return true;
                }
                else
                {
                    filename = "Error on loading\n raw data!";
                    return false;
                }
            }
            catch (Exception x)
            {
                filename = x.Message;
                return false;
            }
        }
    }

    /*
    public class Refresher : IRefresher
    {
        public void refreshUi(params ListBox[] lbs)
        {
            foreach(ListBox l in lbs)
            {
                l.Items.Refresh();
            }
        }
    }*/
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
