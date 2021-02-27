using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Wpf;

namespace Lab01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Calculate_Click(object sender, RoutedEventArgs e)
        {
            IList<DataPoint> p = new List<DataPoint>();
            p.Add(new DataPoint(0, 0));
            p.Add(new DataPoint(1, 1));
            p.Add(new DataPoint(2, 2));
            p.Add(new DataPoint(3, 3));
            LineSeries ls = new LineSeries
            {
                ItemsSource = p
            };

            Oxyplot_Output.Series.Clear();
            Oxyplot_Output.Series.Add(ls);
            Oxyplot_Output.InvalidatePlot(true);
        }

        private void ChechIsNumber(TextBox textBox)
        {
            string text = textBox.Text;

            try
            {
                Convert.ToDouble(text);
            }
            catch (FormatException)
            {
                textBox.Text = text.Remove(text.Length - 1, 1);
            }
        }

        private void TextBox_SigmaGenerator_TextChanged(object sender, TextChangedEventArgs e)
        {
            // ChechIsNumber(TextBox_SigmaGenerator);
        }

        private void TextBox_SigmaTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            // ChechIsNumber(TextBox_SigmaTime);
        }
    }
}
