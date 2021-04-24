using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using Lab04.Models;
using Modeling.Distributions;
using Modeling.QueuingSystem;

namespace Lab04
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int countAllExperiments = 16;

        private readonly int countIterationExperiments = 20;

        private readonly int widthXColumns = 70;
        private readonly int widthYColumns = 70;

        private double b0;
        private double b1;
        private double b2;
        private double b3;
        private double b4;
        private double b12;
        private double b13;
        private double b14;
        private double b23;
        private double b24;
        private double b34;
        private double b123;
        private double b124;
        private double b134;
        private double b234;
        private double b1234;
        private double b11;
        private double b22;
        private double b33;
        private double b44;

        private double minLambdaComing1;
        private double maxLambdaComing1;
        private double minLambdaComing2;
        private double maxLambdaComing2;
        private double minLambdaProcessing1;
        private double maxLambdaProcessing1;
        private double minLambdaProcessing2;
        private double maxLambdaProcessing2;
        private int count;

        private double S;
        private double alpha;
        private int n = 0;

        public MainWindow()
        {
            InitializeComponent();

            double N = countAllExperiments + 2 * 4 + 1;
            S = Math.Sqrt(countAllExperiments / N);
            alpha = Math.Sqrt((S * N - countAllExperiments) / 2.0);
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

        private double CalculateB(double min, double max, List<double> x, List<double> y)
        {
            double xy = 0;
            double xx = 0;

            for (int i = 0; i < x.Count; ++i)
            {
                xy += x[i] * y[i];
                xx += x[i] * x[i];
            }

            return xy / xx;
        }

        private double CalculateY(List<double> b, List<double> x)
        {
            double result = 0;
            for (int i = 0; i < b.Count; ++i)
            {
                result += b[i] * x[i];
            }
            return result;
        }

        private void Button_StartExperiment_Click(object sender, RoutedEventArgs e)
        {
            bool parseMinComing1 = double.TryParse(TextBox_MinComing1.Text, out minLambdaComing1);
            bool parseMaxComing1 = double.TryParse(TextBox_MaxComing1.Text, out maxLambdaComing1);
            bool parseMinComing2 = double.TryParse(TextBox_MinComing2.Text, out minLambdaComing2);
            bool parseMaxComing2 = double.TryParse(TextBox_MaxComing2.Text, out maxLambdaComing2);
            bool parseMinProcessing1 = double.TryParse(TextBox_MinProcessing1.Text, out minLambdaProcessing1);
            bool parseMaxProcessing1 = double.TryParse(TextBox_MaxProcessing1.Text, out maxLambdaProcessing1);
            bool parseMinProcessing2 = double.TryParse(TextBox_MinProcessing1.Text, out minLambdaProcessing2);
            bool parseMaxProcessing2 = double.TryParse(TextBox_MaxProcessing1.Text, out maxLambdaProcessing2);
            bool parseCount = int.TryParse(TextBox_Count.Text, out count);

            if (parseMinComing1 && parseMaxComing1 && parseMinComing2 && parseMaxComing2 && parseMinProcessing1 && parseMaxProcessing1 && parseMinProcessing2 && parseMaxProcessing2 && parseCount)
            {
                CalculateModel();
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
            bool parseProcessing1 = double.TryParse(TextBox_PointProcessing1.Text, out double lambdaProcessing1);
            bool parseProcessing2 = double.TryParse(TextBox_PointProcessing1.Text, out double lambdaProcessing2);

            if (parseComing1 && parseComing2 && parseProcessing1 && parseProcessing2)
            {
                AddPoint(lambdaComing1, lambdaComing2, lambdaProcessing1, lambdaProcessing2);
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

        private void CalculateModel()
        {
            ListView_TableParameters.Items.Clear();

            var listX0 = new List<double>();
            var listX1 = new List<double>();
            var listX2 = new List<double>();
            var listX3 = new List<double>();
            var listX4 = new List<double>();
            var listX12 = new List<double>();
            var listX13 = new List<double>();
            var listX14 = new List<double>();
            var listX23 = new List<double>();
            var listX24 = new List<double>();
            var listX34 = new List<double>();
            var listX123 = new List<double>();
            var listX124 = new List<double>();
            var listX134 = new List<double>();
            var listX234 = new List<double>();
            var listX1234 = new List<double>();
            var listX11 = new List<double>();
            var listX22 = new List<double>();
            var listX33 = new List<double>();
            var listX44 = new List<double>();
            var listY = new List<double>();
            var listYn = new List<double>();

            for (int i = 1; i <= countAllExperiments; ++i)
            {
                n = i;
                int x0 = 1;
                listX0.Add(x0);

                int x1 = i % 2 == 0 ? 1 : -1;
                listX1.Add(x1);

                int x2 = (i - 1) / 2 % 2 == 0 ? -1 : 1;
                listX2.Add(x2);

                int x3 = (i - 1) / 4 % 2 == 0 ? -1 : 1;
                listX3.Add(x3);

                int x4 = (i - 1) / 8 % 2 == 0 ? -1 : 1;
                listX4.Add(x4);

                int x12 = x1 * x2;
                listX12.Add(x12);

                int x13 = x1 * x3;
                listX13.Add(x13);

                int x14 = x1 * x4;
                listX14.Add(x14);

                int x23 = x2 * x3;
                listX23.Add(x23);

                int x24 = x2 * x4;
                listX24.Add(x24);

                int x34 = x3 * x4;
                listX34.Add(x34);

                int x123 = x1 * x2 * x3;
                listX123.Add(x123);

                int x124 = x1 * x2 * x4;
                listX124.Add(x124);

                int x134 = x1 * x3 * x4;
                listX134.Add(x134);

                int x234 = x2 * x3 * x4;
                listX234.Add(x234);

                int x1234 = x1 * x2 * x3 * x4;
                listX1234.Add(x1234);

                double x11 = x1 * x1 - S;
                listX11.Add(x11);

                double x22 = x2 * x2 - S;
                listX22.Add(x22);

                double x33 = x3 * x3 - S;
                listX33.Add(x33);

                double x44 = x4 * x4 - S;
                listX44.Add(x44);

                double lambdaComing1 = ConvertFactorToValue(minLambdaComing1, maxLambdaComing1, x1);
                double lambdaComing2 = ConvertFactorToValue(minLambdaComing2, maxLambdaComing2, x2);
                double lambdaProcessing1 = ConvertFactorToValue(minLambdaProcessing1, maxLambdaProcessing1, x3);
                double lambdaProcessing2 = ConvertFactorToValue(minLambdaProcessing2, maxLambdaProcessing2, x4);

                double sigmaComing1 = Rayleigh.ConvertLambdaToSigma(lambdaComing1);
                double sigmaComing2 = Rayleigh.ConvertLambdaToSigma(lambdaComing2);
                double sigmaProcessing1 = Rayleigh.ConvertLambdaToSigma(lambdaProcessing1);
                double sigmaProcessing2 = Rayleigh.ConvertLambdaToSigma(lambdaProcessing2);

                var comingDistribution1 = new Rayleigh(sigmaComing1);
                var comingDistribution2 = new Rayleigh(sigmaComing2);
                var proecssingDisctribution1 = new Rayleigh(sigmaProcessing1);
                var proecssingDisctribution2 = new Rayleigh(sigmaProcessing2);

                double y = 0;
                for (int exp = 0; exp < countIterationExperiments; ++exp)
                {
                    ModelResult result = CalculateModel(comingDistribution1, comingDistribution2, proecssingDisctribution1, proecssingDisctribution2, count);
                    y += result.AverageTime;
                }
                y = Math.Round(y / (double)countIterationExperiments, 5);
                listY.Add(y);
            }

            for (int i = 0; i < 9; ++i)
            {
                n++;

                double x1 = 0;
                double x2 = 0;
                double x3 = 0;
                double x4 = 0;

                switch (i)
                {
                    case 0:
                        x1 = alpha;
                        break;
                    case 1:
                        x1 = -alpha;
                        break;
                    case 2:
                        x2 = alpha;
                        break;
                    case 3:
                        x2 = -alpha;
                        break;
                    case 4:
                        x3 = alpha;
                        break;
                    case 5:
                        x3 = -alpha;
                        break;
                    case 6:
                        x4 = alpha;
                        break;
                    case 7:
                        x4 = -alpha;
                        break;
                    default:
                        break;
                }

                double x0 = 1;

                listX0.Add(x0);
                listX1.Add(x1);
                listX2.Add(x2);
                listX3.Add(x3);
                listX4.Add(x4);

                double x12 = x1 * x2;
                listX12.Add(x12);

                double x13 = x1 * x3;
                listX13.Add(x13);

                double x14 = x1 * x4;
                listX14.Add(x14);

                double x23 = x2 * x3;
                listX23.Add(x23);

                double x24 = x2 * x4;
                listX24.Add(x24);

                double x34 = x3 * x4;
                listX34.Add(x34);

                double x123 = x1 * x2 * x3;
                listX123.Add(x123);

                double x124 = x1 * x2 * x4;
                listX124.Add(x124);

                double x134 = x1 * x3 * x4;
                listX134.Add(x134);

                double x234 = x2 * x3 * x4;
                listX234.Add(x234);

                double x1234 = x1 * x2 * x3 * x4;
                listX1234.Add(x1234);

                double x11 = x1 * x1 - S;
                listX11.Add(x11);

                double x22 = x2 * x2 - S;
                listX22.Add(x22);

                double x33 = x3 * x3 - S;
                listX33.Add(x33);

                double x44 = x4 * x4 - S;
                listX44.Add(x44);

                double lambdaComing1 = ConvertFactorToValue(minLambdaComing1, maxLambdaComing1, x1);
                double lambdaComing2 = ConvertFactorToValue(minLambdaComing2, maxLambdaComing2, x2);
                double lambdaProcessing1 = ConvertFactorToValue(minLambdaProcessing1, maxLambdaProcessing1, x3);
                double lambdaProcessing2 = ConvertFactorToValue(minLambdaProcessing2, maxLambdaProcessing2, x4);

                double sigmaComing1 = Rayleigh.ConvertLambdaToSigma(lambdaComing1);
                double sigmaComing2 = Rayleigh.ConvertLambdaToSigma(lambdaComing2);
                double sigmaProcessing1 = Rayleigh.ConvertLambdaToSigma(lambdaProcessing1);
                double sigmaProcessing2 = Rayleigh.ConvertLambdaToSigma(lambdaProcessing2);

                var comingDistribution1 = new Rayleigh(sigmaComing1);
                var comingDistribution2 = new Rayleigh(sigmaComing2);
                var proecssingDisctribution1 = new Rayleigh(sigmaProcessing1);
                var proecssingDisctribution2 = new Rayleigh(sigmaProcessing2);

                double y = 0;
                for (int exp = 0; exp < countIterationExperiments; ++exp)
                {
                    ModelResult result = CalculateModel(comingDistribution1, comingDistribution2, proecssingDisctribution1, proecssingDisctribution2, count);
                    y += result.AverageTime;
                }
                y = Math.Round(y / (double)countIterationExperiments, 5);
                listY.Add(y);
            }

            double min1 = minLambdaComing1, min2 = minLambdaComing2, min3 = minLambdaProcessing1, min4 = minLambdaProcessing1;
            double max1 = maxLambdaComing1, max2 = maxLambdaComing2, max3 = maxLambdaProcessing1, max4 = maxLambdaProcessing1;

            b0 = CalculateB(0, 1, listX0, listY);
            b1 = CalculateB(min1, max1, listX1, listY);
            b2 = CalculateB(min2, max2, listX2, listY);
            b3 = CalculateB(min3, max3, listX3, listY);
            b4 = CalculateB(min4, max4, listX4, listY);
            b12 = CalculateB(min1 * min2, max1 * max2, listX12, listY);
            b13 = CalculateB(min1 * min3, max1 * max3, listX13, listY);
            b14 = CalculateB(min1 * min4, max1 * max4, listX14, listY);
            b23 = CalculateB(min2 * min3, max2 * max3, listX23, listY);
            b24 = CalculateB(min2 * min4, max2 * max4, listX24, listY);
            b34 = CalculateB(min3 * min4, max3 * max4, listX34, listY);
            b123 = CalculateB(min1 * min2 * min3, max1 * max2 * max3, listX123, listY);
            b124 = CalculateB(min1 * min2 * min4, max1 * max2 * max4, listX124, listY);
            b134 = CalculateB(min1 * min3 * min4, max1 * max3 * max4, listX134, listY);
            b234 = CalculateB(min2 * min3 * min4, max2 * max3 * max4, listX234, listY);
            b1234 = CalculateB(min1 * min2 * min3 * min4, max1 * max2 * max3 * max4, listX1234, listY);
            b11 = CalculateB(min1 * min1, max1 * max1, listX11, listY);
            b22 = CalculateB(min2 * min2, max2 * max2, listX22, listY);
            b33 = CalculateB(min3 * min3, max3 * max3, listX33, listY);
            b44 = CalculateB(min4 * min4, max4 * max4, listX44, listY);
            b0 = b0 - b11 * S - b22 * S - b33 * S - b44 * S;

            for (int i = 0; i < n; ++i)
            {
                List<double> b = new List<double>
                {
                    b0,
                    b1,
                    b2,
                    b3,
                    b4,
                    b12,
                    b13,
                    b14,
                    b23,
                    b24,
                    b34,
                    b123,
                    b124,
                    b134,
                    b234,
                    b1234,
                    b11,
                    b22,
                    b33,
                    b44,
                };

                List<double> x = new List<double>
                {
                    listX0[i],
                    listX1[i],
                    listX2[i],
                    listX3[i],
                    listX4[i],
                    listX12[i],
                    listX13[i],
                    listX14[i],
                    listX23[i],
                    listX24[i],
                    listX34[i],
                    listX123[i],
                    listX124[i],
                    listX134[i],
                    listX234[i],
                    listX1234[i],
                    listX11[i],
                    listX22[i],
                    listX33[i],
                    listX44[i],
                };

                double yn = CalculateY(b, x);

                ListView_TableParameters.Items.Add(new EquationCoefffcients(
                    i + 1,
                    Math.Round(listX0[i], 5),
                    Math.Round(listX1[i], 5),
                    Math.Round(listX2[i], 5),
                    Math.Round(listX3[i], 5),
                    Math.Round(listX4[i], 5),
                    Math.Round(listX12[i], 5),
                    Math.Round(listX13[i], 5),
                    Math.Round(listX14[i], 5),
                    Math.Round(listX23[i], 5),
                    Math.Round(listX24[i], 5),
                    Math.Round(listX34[i], 5),
                    Math.Round(listX123[i], 5),
                    Math.Round(listX124[i], 5),
                    Math.Round(listX134[i], 5),
                    Math.Round(listX234[i], 5),
                    Math.Round(listX1234[i], 5),
                    Math.Round(listX11[i], 5),
                    Math.Round(listX22[i], 5),
                    Math.Round(listX33[i], 5),
                    Math.Round(listX44[i], 5),
                    Math.Round(listY[i], 5),
                    Math.Round(yn, 5),
                    Math.Round(Math.Abs(listY[i] - yn), 5)
                ));
            }

            ListView_TableResults.Items.Clear();
            ListView_TableResults.Items.Add(new EquationResult(
                Math.Round(b0, 5),
                Math.Round(b1, 5),
                Math.Round(b2, 5),
                Math.Round(b3, 5),
                Math.Round(b4, 5),
                Math.Round(b12, 5),
                Math.Round(b13, 5),
                Math.Round(b14, 5),
                Math.Round(b23, 5),
                Math.Round(b24, 5),
                Math.Round(b34, 5),
                Math.Round(b123, 5),
                Math.Round(b124, 5),
                Math.Round(b134, 5),
                Math.Round(b234, 5),
                Math.Round(b1234, 5),
                Math.Round(b11, 5),
                Math.Round(b22, 5),
                Math.Round(b33, 5),
                Math.Round(b44, 5)
            ));
        }

        private void AddPoint(double lambdaComing1, double lambdaComing2, double lambdaProcessing1, double lambdaProcessing2)
        {
            n++;
            double x0 = 1;
            double x1 = ConvertValueToFactor(minLambdaComing1, maxLambdaComing1, lambdaComing1);
            double x2 = ConvertValueToFactor(minLambdaComing2, maxLambdaComing2, lambdaComing2);
            double x3 = ConvertValueToFactor(minLambdaProcessing1, maxLambdaProcessing1, lambdaProcessing1);
            double x4 = ConvertValueToFactor(minLambdaProcessing2, maxLambdaProcessing2, lambdaProcessing2);
            double x12 = x1 * x2;
            double x13 = x1 * x3;
            double x14 = x1 * x4;
            double x23 = x2 * x3;
            double x24 = x2 * x4;
            double x34 = x3 * x4;
            double x123 = x1 * x2 * x3;
            double x124 = x1 * x2 * x4;
            double x134 = x1 * x3 * x4;
            double x234 = x2 * x3 * x4;
            double x1234 = x1 * x2 * x3 * x4;
            double x11 = x1 * x1 - S;
            double x22 = x2 * x2 - S;
            double x33 = x3 * x3 - S;
            double x44 = x4 * x4 - S;

            var comingDistribution1 = new Rayleigh(Rayleigh.ConvertLambdaToSigma(lambdaComing1));
            var comingDistribution2 = new Rayleigh(Rayleigh.ConvertLambdaToSigma(lambdaComing2));
            var proecssingDisctribution1 = new Rayleigh(Rayleigh.ConvertLambdaToSigma(lambdaProcessing1));
            var proecssingDisctribution2 = new Rayleigh(Rayleigh.ConvertLambdaToSigma(lambdaProcessing2));

            double y = 0;
            for (int exp = 0; exp < countIterationExperiments; ++exp)
            {
                ModelResult result = CalculateModel(comingDistribution1, comingDistribution2, proecssingDisctribution1, proecssingDisctribution2, count);
                y += result.AverageTime;
            }
            y = Math.Round(y / (double)countIterationExperiments, 5);

            var x = new List<double> { x0, x1, x2, x3, x4, x12, x13, x14, x23, x24, x34, x123, x124, x134, x234, x1234, x11, x22, x33, x44 };
            var b = new List<double> { b0, b1, b2, b3, b4, b12, b13, b14, b23, b24, b34, b123, b124, b134, b234, b1234, b11, b22, b33, b44 };
            
            double yn = CalculateY(b, x);

            ListView_TableParameters.Items.Add(new EquationCoefffcients(
                n,
                Math.Round(x0, 5),
                Math.Round(x1, 5),
                Math.Round(x2, 5),
                Math.Round(x3, 5),
                Math.Round(x4, 5),
                Math.Round(x12, 5),
                Math.Round(x13, 5),
                Math.Round(x14, 5),
                Math.Round(x23, 5),
                Math.Round(x24, 5),
                Math.Round(x34, 5),
                Math.Round(x123, 5),
                Math.Round(x124, 5),
                Math.Round(x134, 5),
                Math.Round(x234, 5),
                Math.Round(x1234, 5),
                Math.Round(x11, 5),
                Math.Round(x22, 5),
                Math.Round(x33, 5),
                Math.Round(x44, 5),
                Math.Round(y, 5),
                Math.Round(yn, 5),
                Math.Round(Math.Abs(y - yn), 5)
            ));
        }

        private ModelResult CalculateModel(IDistribution generatorDistribution1, IDistribution generatorDistribution2, IDistribution timeDistribution1, IDistribution timeDistribution2, int count)
        {
            var op = new Operator(new List<IDistribution> { timeDistribution1, timeDistribution2 });
            var generator1 = new Generator(generatorDistribution1, new List<Operator> { op }, count / 2, 0);
            var generator2 = new Generator(generatorDistribution2, new List<Operator> { op }, count / 2, 1);
            var model = new Model(new List<Generator> { generator1, generator2 }, new List<IBlock> { generator1, generator2, op });
            return model.Generate();
        }
    }
}
