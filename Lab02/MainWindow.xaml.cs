using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using Lab02.Models;
using Modeling.Distributions;
using Modeling.QueuingSystem;

namespace Lab02
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int countAllExperiments = 4;

        private readonly int countIterationExperiments = 20;

        private double b0;
        private double b1;
        private double b2;
        private double b12;

        private double minLambdaComing;
        private double maxLambdaComing;
        private double minLambdaProcessing;
        private double maxLambdaProcessing;
        private int count;

        private int n = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;

            double workingWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth;
            double count = gView.Columns.Count;
            int columnWidth = (int)Math.Round(workingWidth / count);

            foreach (var col in gView.Columns)
            {
                col.Width = columnWidth;
            }
        }

        private double ConvertValueToFactor(double min, double max, double value)
            => (value - (max + min) / 2.0) / ((max - min) / 2.0);

        private double ConvertFactorToValue(double min, double max, double factor)
            => factor * ((max - min) / 2.0) + (max + min) / 2.0;

        private double CalculateB(double min, double max, List<int> values, List<double> listY)
        {
            double b = 0;
            for (int i = 0; i < countAllExperiments; ++i)
            {
                b += ConvertValueToFactor(min, max,ConvertFactorToValue(min, max, values[i])) * listY[i];
            }
            b /= (double)countAllExperiments;

            return Math.Round(b, 5);
        }

        private void Button_StartExperiment_Click(object sender, RoutedEventArgs e)
        {
            bool parseMinComing = double.TryParse(TextBox_MinComing.Text, out minLambdaComing);
            bool parseMaxComing = double.TryParse(TextBox_MaxComing.Text, out maxLambdaComing);
            bool parseMinProcessing = double.TryParse(TextBox_MinProcessing.Text, out minLambdaProcessing);
            bool parseMaxProcessing = double.TryParse(TextBox_MaxProcessing.Text, out maxLambdaProcessing);
            bool parseCount = int.TryParse(TextBox_Count.Text, out count);

            if (parseMinComing && parseMaxComing && parseMinProcessing && parseMaxProcessing && parseCount)
            {
                ListView_TableParameters.Items.Clear();

                var listX0 = new List<int>();
                var listX1 = new List<int>();
                var listX2 = new List<int>();
                var listX12 = new List<int>();
                var listY = new List<double>();
                var listYl = new List<double>();
                var listYcn = new List<double>();

                for (int i = 1; i <= countAllExperiments; ++i)
                {
                    n = i;
                    int x0 = 1;
                    listX0.Add(x0);

                    int x1 = i % 2 == 0 ? 1 : -1;
                    listX1.Add(x1);

                    int x2 = (i - 1) / 2 % 2 == 0 ? -1 : 1;
                    listX2.Add(x2);

                    int x12 = x1 * x2;
                    listX12.Add(x12);

                    double lambdaComing = ConvertFactorToValue(minLambdaComing, maxLambdaComing, x1);
                    double lambdaProcessing = ConvertFactorToValue(minLambdaProcessing, maxLambdaProcessing, x2);

                    double sigmaComing = Rayleigh.ConvertLambdaToSigma(lambdaComing);
                    double sigmaProcessing = Rayleigh.ConvertLambdaToSigma(lambdaProcessing);

                    var comingDistribution = new Rayleigh(sigmaComing);
                    var proecssingDisctribution = new Rayleigh(sigmaProcessing);

                    double y = 0;
                    for (int exp = 0; exp < countIterationExperiments; ++exp)
                    {
                        ModelResult result = CalculateModel(comingDistribution, proecssingDisctribution, count);
                        y += result.AverageTime;
                    }
                    y = Math.Round(y / (double)countIterationExperiments, 5);
                    listY.Add(y);
                }

                b0 = CalculateB(0, 1, listX0, listY);
                b1 = CalculateB(minLambdaComing, maxLambdaComing, listX1, listY);
                b2 = CalculateB(minLambdaProcessing, maxLambdaProcessing, listX2, listY);
                b12 = CalculateB(minLambdaComing * minLambdaProcessing, maxLambdaComing * maxLambdaProcessing, listX12, listY);

                for (int i = 0; i < n; ++i)
                {
                    double yl = Math.Round(b0 + listX1[i] * b1 + listX2[i] * b2, 5);
                    double ycn = Math.Round(b0 + listX1[i] * b1 + listX2[i] * b2 + listX12[i] * b12, 5);

                    yl = Math.Abs(yl);
                    ycn = Math.Abs(ycn);

                    ListView_TableParameters.Items.Add(new EquationCoefffcients(
                        i + 1,
                        listX0[i],
                        listX1[i],
                        listX2[i],
                        listX12[i],
                        listY[i],
                        yl,
                        ycn,
                        Math.Round(Math.Abs(listY[i] - yl), 5),
                        Math.Round(Math.Abs(listY[i] - ycn), 5)
                    ));
                }

                ListView_TableResults.Items.Clear();
                ListView_TableResults.Items.Add(new EquationResult(b0, b1, b2, b12));

                Button_AddPoint.IsEnabled = true;
            }
            else
            {
                MessageBox.Show(
                    "Введите вещественные числа с разделителем запятая.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void Button_AddPoint_Click(object sender, RoutedEventArgs e)
        {
            bool parseComing = double.TryParse(TextBox_PointComing.Text, out double lambdaComing);
            bool parseProcessing = double.TryParse(TextBox_PointProcessing.Text, out double lambdaProcessing);

            if (parseComing && parseProcessing)
            {
                n++;
                double x0 = 1;
                double x1 = ConvertValueToFactor(minLambdaComing, maxLambdaComing, lambdaComing);
                double x2 = ConvertValueToFactor(minLambdaProcessing, maxLambdaProcessing, lambdaProcessing);
                double x12 = x1 * x2;

                var comingDistribution = new Rayleigh(Rayleigh.ConvertLambdaToSigma(lambdaComing));
                var proecssingDisctribution = new Rayleigh(Rayleigh.ConvertLambdaToSigma(lambdaProcessing));

                double y = 0;
                for (int exp = 0; exp < countIterationExperiments; ++exp)
                {
                    ModelResult result = CalculateModel(comingDistribution, proecssingDisctribution, count);
                    y += result.AverageTime;
                }
                y = Math.Round(y / (double)countIterationExperiments, 5);

                double yl = Math.Round(b0 + x1 * b1 + x2 * b2, 5);
                double ycn = Math.Round(b0 + x1 * b1 + x2 * b2 + x12 * b12, 5);

                yl = Math.Abs(yl);
                ycn = Math.Abs(ycn);

                ListView_TableParameters.Items.Add(new EquationCoefffcients(
                    n,
                    x0,
                    x1,
                    x2,
                    x12,
                    y,
                    yl,
                    ycn,
                    Math.Round(Math.Abs(y - yl), 5),
                    Math.Round(Math.Abs(y - ycn), 5)
                 ));
            }
            else
            {
                MessageBox.Show(
                    "Введите вещественные числа с разделителем запятая.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private ModelResult CalculateModel(IDistribution generatorDistribution, IDistribution timeDistribution, int count)
        {
            var op = new Operator(timeDistribution);
            var generator = new Generator(generatorDistribution, new List<Operator> { op }, count);
            var model = new Modeling.QueuingSystem.Model(generator, new List<IBlock> { generator, op });
            return model.Generate();
        }
    }
}
