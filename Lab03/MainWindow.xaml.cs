using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using Lab03.Models;
using Modeling.Distributions;
using Modeling.QueuingSystem;

namespace Lab03
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int countAllExperiments = 8;

        private readonly int countIterationExperiments = 20;

        private double b0;
        private double b1;
        private double b2;
        private double b3;
        private double b12;
        private double b13;
        private double b23;
        private double b123;

        private double minLambdaComing1;
        private double maxLambdaComing1;
        private double minLambdaComing2;
        private double maxLambdaComing2;
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
                b += ConvertValueToFactor(min, max, ConvertFactorToValue(min, max, values[i])) * listY[i];
            }
            b /= (double)countAllExperiments;

            return Math.Round(b, 5);
        }

        private void Button_StartExperiment_Click(object sender, RoutedEventArgs e)
        {
            bool parseMinComing1 = double.TryParse(TextBox_MinComing1.Text, out minLambdaComing1);
            bool parseMaxComing1 = double.TryParse(TextBox_MaxComing1.Text, out maxLambdaComing1);
            bool parseMinComing2 = double.TryParse(TextBox_MinComing2.Text, out minLambdaComing2);
            bool parseMaxComing2 = double.TryParse(TextBox_MaxComing2.Text, out maxLambdaComing2);
            bool parseMinProcessing = double.TryParse(TextBox_MinProcessing.Text, out minLambdaProcessing);
            bool parseMaxProcessing = double.TryParse(TextBox_MaxProcessing.Text, out maxLambdaProcessing);
            bool parseCount = int.TryParse(TextBox_Count.Text, out count);

            if (parseMinComing1 && parseMaxComing1 && parseMinComing2 && parseMaxComing2 && parseMinProcessing && parseMaxProcessing && parseCount)
            {
                ListView_TableParameters.Items.Clear();

                var listX0 = new List<int>();
                var listX1 = new List<int>();
                var listX2 = new List<int>();
                var listX3 = new List<int>();
                var listX12 = new List<int>();
                var listX13 = new List<int>();
                var listX23 = new List<int>();
                var listX123 = new List<int>();
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

                    int x3 = i <= 4 ? -1 : 1;
                    listX3.Add(x3);

                    int x12 = x1 * x2;
                    listX12.Add(x12);

                    int x13 = x1 * x3;
                    listX13.Add(x13);

                    int x23 = x2 * x3;
                    listX23.Add(x23);

                    int x123 = x1 * x2 * x3;
                    listX123.Add(x123);

                    double lambdaComing1 = ConvertFactorToValue(minLambdaComing1, maxLambdaComing1, x1);
                    double lambdaComing2 = ConvertFactorToValue(minLambdaComing2, maxLambdaComing2, x2);
                    double lambdaProcessing = ConvertFactorToValue(minLambdaProcessing, maxLambdaProcessing, x3);

                    double sigmaComing1 = Rayleigh.ConvertLambdaToSigma(lambdaComing1);
                    double sigmaComing2 = Rayleigh.ConvertLambdaToSigma(lambdaComing2);
                    double sigmaProcessing = Rayleigh.ConvertLambdaToSigma(lambdaProcessing);

                    var comingDistribution1 = new Rayleigh(sigmaComing1);
                    var comingDistribution2 = new Rayleigh(sigmaComing2);
                    var proecssingDisctribution = new Rayleigh(sigmaProcessing);

                    double y = 0;
                    for (int exp = 0; exp < countIterationExperiments; ++exp)
                    {
                        ModelResult result = CalculateModel(comingDistribution1, comingDistribution2, proecssingDisctribution, count);
                        y += result.AverageTime;
                    }
                    y = Math.Round(y / (double)countIterationExperiments, 5);
                    listY.Add(y);
                }

                b0 = CalculateB(0, 1, listX0, listY);
                b1 = CalculateB(minLambdaComing1, maxLambdaComing1, listX1, listY);
                b2 = CalculateB(minLambdaComing2, maxLambdaComing2, listX2, listY);
                b3 = CalculateB(minLambdaProcessing, maxLambdaProcessing, listX3, listY);
                b12 = CalculateB(minLambdaComing1 * minLambdaComing2, maxLambdaComing1 * maxLambdaComing2, listX12, listY);
                b13 = CalculateB(minLambdaComing1 * minLambdaProcessing, maxLambdaComing1 * maxLambdaProcessing, listX13, listY);
                b23 = CalculateB(minLambdaComing2 * minLambdaProcessing, maxLambdaComing2 * maxLambdaProcessing, listX23, listY);
                b123 = CalculateB(minLambdaComing1 * minLambdaComing2 * minLambdaProcessing, maxLambdaComing1 * maxLambdaComing2 * maxLambdaProcessing, listX23, listY);

                for (int i = 0; i < n; ++i)
                {
                    double yl = Math.Round(b0 + listX1[i] * b1 + listX2[i] * b2 + listX3[i] * b3, 5);
                    double ycn = Math.Round(b0 + listX1[i] * b1 + listX2[i] * b2 + listX3[i] * b3 + listX12[i] * b12 + listX13[i] * b13 + listX23[i] * b23 + listX123[i] * b123, 5);

                    ListView_TableParameters.Items.Add(new EquationCoefffcients(
                        i + 1,
                        listX0[i],
                        listX1[i],
                        listX2[i],
                        listX3[i],
                        listX12[i],
                        listX13[i],
                        listX23[i],
                        listX123[i],
                        listY[i],
                        yl,
                        ycn,
                        Math.Round(Math.Abs(listY[i] - yl), 5),
                        Math.Round(Math.Abs(listY[i] - ycn), 5)
                    ));
                }

                ListView_TableResults.Items.Clear();
                ListView_TableResults.Items.Add(new EquationResult(b0, b1, b2, b3, b12, b13, b23, b123));

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
            bool parseComing1 = double.TryParse(TextBox_PointComing1.Text, out double lambdaComing1);
            bool parseComing2 = double.TryParse(TextBox_PointComing2.Text, out double lambdaComing2);
            bool parseProcessing = double.TryParse(TextBox_PointProcessing.Text, out double lambdaProcessing);

            if (parseComing1 && parseComing2 && parseProcessing)
            {
                n++;
                double x0 = 1;
                double x1 = ConvertValueToFactor(minLambdaComing1, maxLambdaComing1, lambdaComing1);
                double x2 = ConvertValueToFactor(minLambdaComing2, maxLambdaComing2, lambdaComing2);
                double x3 = ConvertValueToFactor(minLambdaProcessing, maxLambdaProcessing, lambdaProcessing);
                double x12 = x1 * x2;
                double x13 = x1 * x3;
                double x23 = x2 * x3;
                double x123 = x1 * x2 * x3;

                var comingDistribution1 = new Rayleigh(Rayleigh.ConvertLambdaToSigma(lambdaComing1));
                var comingDistribution2 = new Rayleigh(Rayleigh.ConvertLambdaToSigma(lambdaComing2));
                var proecssingDisctribution = new Rayleigh(Rayleigh.ConvertLambdaToSigma(lambdaProcessing));

                double y = 0;
                for (int exp = 0; exp < countIterationExperiments; ++exp)
                {
                    ModelResult result = CalculateModel(comingDistribution1, comingDistribution2, proecssingDisctribution, count);
                    y += result.AverageTime;
                }
                y = Math.Round(y / (double)countIterationExperiments, 5);

                double yl = Math.Round(b0 + x1 * b1 + x2 * b2 + x3 * b3, 5);
                double ycn = Math.Round(b0 + x1 * b1 + x2 * b2 + x3 * b3 + x12 * b12 + x13 * b13 + x23 * b23 + x123 * b123, 5);

                ListView_TableParameters.Items.Add(new EquationCoefffcients(
                    n,
                    x0,
                    x1,
                    x2,
                    x3,
                    x12,
                    x13,
                    x23,
                    x123,
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

        private ModelResult CalculateModel(IDistribution generatorDistribution1, IDistribution generatorDistribution2, IDistribution timeDistribution, int count)
        {
            var op = new Operator(timeDistribution);
            var generator1 = new Generator(generatorDistribution1, new List<Operator> { op }, count / 2);
            var generator2 = new Generator(generatorDistribution2, new List<Operator> { op }, count / 2);
            var model = new Modeling.QueuingSystem.Model(new List<Generator> { generator1, generator2 }, new List<IBlock> { generator1, generator2, op });
            return model.Generate();
        }
    }
}
