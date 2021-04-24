using System;
using System.Windows;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Wpf;
using Modeling.Distributions;
using Modeling.QueuingSystem;

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

        private void MakeGraph()
        {
            int count = 10000;

            double lambdaProcessing = 10.0;
            double sigmaTime = Rayleigh.ConvertLambdaToSigma(lambdaProcessing);
            var timeDistribytion = new Rayleigh(sigmaTime);

            var points = new List<DataPoint>();

            for (double lambdaComing = 0.1; lambdaComing <= lambdaProcessing;)
            {
                double sumExperiments = 0;
                int countExperiments = 50;

                for (int i = 0; i < countExperiments; ++i)
                {
                    double sigmaGenerator = Rayleigh.ConvertLambdaToSigma(lambdaComing);
                    var generatorDistribution = new Rayleigh(sigmaGenerator);

                    sumExperiments += CalculateModel(generatorDistribution, timeDistribytion, count).AverageTime;
                }

                points.Add(new DataPoint(lambdaComing / lambdaProcessing, sumExperiments / countExperiments));

                if (lambdaComing / lambdaProcessing <= 0.8)
                {
                    lambdaComing += 0.1;
                }
                else
                {
                    lambdaComing += 1;
                }
            }

            LineSeries line = new LineSeries
            {
                ItemsSource = points
            };

            Oxyplot_Output.Series.Clear();
            Oxyplot_Output.Series.Add(line);
            Oxyplot_Output.InvalidatePlot(true);
        }

        private void Button_Calculate_Click(object sender, RoutedEventArgs e)
        {
            bool generatorParsed = double.TryParse(TextBox_SigmaGenerator.Text, out double lambdaComing);
            bool timeParsed = double.TryParse(TextBox_SigmaTime.Text, out double lambdaProcessing);

            if (generatorParsed && timeParsed)
            {
                try
                {
                    int count = 1000;
                    double sigmaGenerator = Rayleigh.ConvertLambdaToSigma(lambdaComing);
                    double sigmaTime = Rayleigh.ConvertLambdaToSigma(lambdaProcessing);

                    double loading = lambdaComing / lambdaProcessing;

                    var generatorDistribution = new Rayleigh(sigmaGenerator);
                    var timeDistribytion = new Rayleigh(sigmaTime);
                    ModelResult result = CalculateModel(generatorDistribution, timeDistribytion, count);

                    double time = result.Time;
                    double avgTime = result.AverageTime;

                    Label_Loading.Content = $"Загрузка системы: {loading}";
                    Label_Time.Content = $"Время работы: {time}";
                    Label_AvgTime.Content = $"Среднее время ожидания: {avgTime}";
                    Label_Count.Content = $"Количество обработанных заявок: {count}";
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

        private ModelResult CalculateModel(IDistribution generatorDistribution, IDistribution timeDistribution, int count)
        {
            var op = new Operator(new List<IDistribution> { timeDistribution });
            var generator = new Generator(generatorDistribution, new List<Operator> { op }, count, 0);
            var model = new Modeling.QueuingSystem.Model(generator, new List<IBlock> { generator, op });
            return model.Generate();
        }

        private void Button_UpdateGraph_Click(object sender, RoutedEventArgs e)
        {
            this.MakeGraph();
        }
    }
}
