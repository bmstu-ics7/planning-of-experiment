using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Wpf;

using Lab01.Distributions;
using Lab01.QueuingSystem;

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
            bool generatorParsed = double.TryParse(TextBox_SigmaGenerator.Text, out double sigmaGenerator);
            bool timeParsed = double.TryParse(TextBox_SigmaTime.Text, out double sigmaTime);

            if (generatorParsed && timeParsed)
            {
                try
                {
                    var generatorDistribution = new Rayleigh(sigmaGenerator);
                    var timeDistribytion = new Rayleigh(sigmaTime);

                    var points = new List<DataPoint>();
                    for (int count = 1; count < 1000; ++count)
                    {
                        double result = CalculateModel(generatorDistribution, timeDistribytion, count);
                        points.Add(new DataPoint(count, result));
                    }

                    LineSeries line = new LineSeries
                    {
                        ItemsSource = points
                    };

                    Oxyplot_Output.Series.Clear();
                    Oxyplot_Output.Series.Add(line);
                    Oxyplot_Output.InvalidatePlot(true);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
            else
            {
                MessageBox.Show(
                    "Введите два вещественных числа с разделителем запятая.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private double CalculateModel(IDisctribution generatorDistribution, IDisctribution timeDistribution, int count)
        {
            var op = new Operator(timeDistribution);
            var generator = new Generator(generatorDistribution, new List<Operator> { op }, count);
            var model = new QueuingSystem.Model(generator, new List<IBlock> { generator, op });
            return model.Generate();
        }
    }
}
