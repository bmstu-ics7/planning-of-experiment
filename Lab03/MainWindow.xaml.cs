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
        private readonly int countAllExperiments = 16;

        private readonly int countIterationExperiments = 20;

        private readonly int widthXColumns = 40;
        private readonly int widthYColumns = 70;

        private double b0_PFE;
        private double b1_PFE;
        private double b2_PFE;
        private double b3_PFE;
        private double b4_PFE;
        private double b12_PFE;
        private double b13_PFE;
        private double b14_PFE;
        private double b23_PFE;
        private double b24_PFE;
        private double b34_PFE;
        private double b123_PFE;
        private double b124_PFE;
        private double b134_PFE;
        private double b234_PFE;
        private double b1234_PFE;

        private double b0_DFE;
        private double b1_DFE;
        private double b2_DFE;
        private double b3_DFE;
        private double b4_DFE;
        private double b12_DFE;
        private double b13_DFE;
        private double b14_DFE;
        private double b23_DFE;
        private double b24_DFE;
        private double b34_DFE;
        private double b123_DFE;
        private double b124_DFE;
        private double b134_DFE;
        private double b234_DFE;
        private double b1234_DFE;

        private double minLambdaComing1;
        private double maxLambdaComing1;
        private double minLambdaComing2;
        private double maxLambdaComing2;
        private double minLambdaProcessing1;
        private double maxLambdaProcessing1;
        private double minLambdaProcessing2;
        private double maxLambdaProcessing2;
        private int count;

        private int n_PFE = 0;
        private int n_DFE = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;


            int countX = 0;
            foreach (var col in gView.Columns)
            {
                if (col.Header.ToString()[0] == 'x' || col.Header.ToString()[0] == '№')
                {
                    col.Width = widthXColumns;
                }
                else
                {
                    col.Width = widthYColumns;
                }
            }
        }

        private double ConvertValueToFactor(double min, double max, double value)
            => (value - (max + min) / 2.0) / ((max - min) / 2.0);

        private double ConvertFactorToValue(double min, double max, double factor)
            => factor * ((max - min) / 2.0) + (max + min) / 2.0;

        private double CalculateB_PFE(double min, double max, List<int> values, List<double> listY)
        {
            double b = 0;
            for (int i = 0; i < countAllExperiments; ++i)
            {
                b += ConvertValueToFactor(min, max, ConvertFactorToValue(min, max, values[i])) * listY[i];
            }
            b /= (double)countAllExperiments;

            return Math.Round(b, 5);
        }

        private double CalculateB_DFE(double min, double max, List<int> values, List<double> listY)
        {
            double b = 0;
            for (int i = 0; i < countAllExperiments / 2; ++i)
            {
                b += ConvertValueToFactor(min, max, ConvertFactorToValue(min, max, values[i])) * listY[i];
            }
            b /= ((double)countAllExperiments / 2.0);

            return Math.Round(b, 5);
        }

        private double MultiplyCoefficients(List<double> b, List<double> x)
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
                ModelPFE();
                ModelDFE();

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
                AddPointPFE(lambdaComing1, lambdaComing2, lambdaProcessing1, lambdaProcessing2);
                AddPointDFE(lambdaComing1, lambdaComing2, lambdaProcessing1, lambdaProcessing2);
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

        private void ModelPFE()
        {
            ListView_TableParameters.Items.Clear();

            var listX0 = new List<int>();
            var listX1 = new List<int>();
            var listX2 = new List<int>();
            var listX3 = new List<int>();
            var listX4 = new List<int>();
            var listX12 = new List<int>();
            var listX13 = new List<int>();
            var listX14 = new List<int>();
            var listX23 = new List<int>();
            var listX24 = new List<int>();
            var listX34 = new List<int>();
            var listX123 = new List<int>();
            var listX124 = new List<int>();
            var listX134 = new List<int>();
            var listX234 = new List<int>();
            var listX1234 = new List<int>();
            var listY = new List<double>();
            var listYl = new List<double>();
            var listYcn = new List<double>();

            for (int i = 1; i <= countAllExperiments; ++i)
            {
                n_PFE = i;
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

            b0_PFE = CalculateB_PFE(0, 1, listX0, listY);
            b1_PFE = CalculateB_PFE(min1, max1, listX1, listY);
            b2_PFE = CalculateB_PFE(min2, max2, listX2, listY);
            b3_PFE = CalculateB_PFE(min3, max3, listX3, listY);
            b4_PFE = CalculateB_PFE(min4, max4, listX4, listY);
            b12_PFE = CalculateB_PFE(min1 * min2, max1 * max2, listX12, listY);
            b13_PFE = CalculateB_PFE(min1 * min3, max1 * max3, listX13, listY);
            b14_PFE = CalculateB_PFE(min1 * min4, max1 * max4, listX14, listY);
            b23_PFE = CalculateB_PFE(min2 * min3, max2 * max3, listX23, listY);
            b24_PFE = CalculateB_PFE(min2 * min4, max2 * max4, listX24, listY);
            b34_PFE = CalculateB_PFE(min3 * min4, max3 * max4, listX34, listY);
            b123_PFE = CalculateB_PFE(min1 * min2 * min3, max1 * max2 * max3, listX123, listY);
            b124_PFE = CalculateB_PFE(min1 * min2 * min4, max1 * max2 * max4, listX124, listY);
            b134_PFE = CalculateB_PFE(min1 * min3 * min4, max1 * max3 * max4, listX134, listY);
            b234_PFE = CalculateB_PFE(min2 * min3 * min4, max2 * max3 * max4, listX234, listY);
            b1234_PFE = CalculateB_PFE(min1 * min2 * min3 * min4, max1 * max2 * max3 * max4, listX1234, listY);

            for (int i = 0; i < n_PFE; ++i)
            {
                List<double> bl = new List<double> { b0_PFE, b1_PFE, b2_PFE, b3_PFE, b4_PFE };
                List<double> xl = new List<double> { listX0[i], listX1[i], listX2[i], listX3[i], listX4[i] };
                
                double yl = Math.Round(MultiplyCoefficients(bl, xl), 5);

                List<double> bcn = new List<double>
                {
                    b0_PFE,
                    b1_PFE,
                    b2_PFE,
                    b3_PFE,
                    b4_PFE,
                    b12_PFE,
                    b13_PFE,
                    b14_PFE,
                    b23_PFE,
                    b24_PFE,
                    b34_PFE,
                    b123_PFE,
                    b124_PFE,
                    b134_PFE,
                    b234_PFE,
                    b1234_PFE,
                };

                List<double> xcn = new List<double>
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
                };

                double ycn = Math.Round(MultiplyCoefficients(bcn, xcn), 5);

                yl = Math.Abs(yl);
                ycn = Math.Abs(ycn);

                ListView_TableParameters.Items.Add(new EquationCoefffcients(
                    i + 1,
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
                    listY[i],
                    yl,
                    ycn,
                    Math.Round(Math.Abs(listY[i] - yl), 5),
                    Math.Round(Math.Abs(listY[i] - ycn), 5)
                ));
            }

            ListView_TableResults.Items.Clear();
            ListView_TableResults.Items.Add(new EquationResult(
                b0_PFE,
                b1_PFE,
                b2_PFE,
                b3_PFE,
                b4_PFE,
                b12_PFE,
                b13_PFE,
                b14_PFE,
                b23_PFE,
                b24_PFE,
                b34_PFE,
                b123_PFE,
                b124_PFE,
                b134_PFE,
                b234_PFE,
                b1234_PFE
            ));
        }

        private void ModelDFE()
        {
            ListView_TableParametersDFE.Items.Clear();

            var listX0 = new List<int>();
            var listX1 = new List<int>();
            var listX2 = new List<int>();
            var listX3 = new List<int>();
            var listX4 = new List<int>();
            var listX12 = new List<int>();
            var listX13 = new List<int>();
            var listX14 = new List<int>();
            var listX23 = new List<int>();
            var listX24 = new List<int>();
            var listX34 = new List<int>();
            var listX123 = new List<int>();
            var listX124 = new List<int>();
            var listX134 = new List<int>();
            var listX234 = new List<int>();
            var listX1234 = new List<int>();
            var listY = new List<double>();
            var listYl = new List<double>();
            var listYcn = new List<double>();

            for (int i = 1; i <= countAllExperiments / 2; ++i)
            {
                n_DFE = i;
                int x0 = 1;
                listX0.Add(x0);

                int x1 = i % 2 == 0 ? 1 : -1;
                listX1.Add(x1);

                int x2 = (i - 1) / 2 % 2 == 0 ? -1 : 1;
                listX2.Add(x2);

                int x3 = (i - 1) / 4 % 2 == 0 ? -1 : 1;
                listX3.Add(x3);

                int x4 = x1 * x2 * x3;
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
                for (int exp = 0; exp < countIterationExperiments / 2; ++exp)
                {
                    ModelResult result = CalculateModel(comingDistribution1, comingDistribution2, proecssingDisctribution1, proecssingDisctribution2, count);
                    y += result.AverageTime;
                }
                y = Math.Round(y / (double)countIterationExperiments, 5);
                listY.Add(y);
            }

            double min1 = minLambdaComing1, min2 = minLambdaComing2, min3 = minLambdaProcessing1, min4 = minLambdaProcessing1;
            double max1 = maxLambdaComing1, max2 = maxLambdaComing2, max3 = maxLambdaProcessing1, max4 = maxLambdaProcessing1;

            b0_DFE = CalculateB_DFE(0, 1, listX0, listY);
            b1_DFE = CalculateB_DFE(min1, max1, listX1, listY);
            b2_DFE = CalculateB_DFE(min2, max2, listX2, listY);
            b3_DFE = CalculateB_DFE(min3, max3, listX3, listY);
            b4_DFE = CalculateB_DFE(min4, max4, listX4, listY);
            b12_DFE = CalculateB_DFE(min1 * min2, max1 * max2, listX12, listY);
            b13_DFE = CalculateB_DFE(min1 * min3, max1 * max3, listX13, listY);
            b14_DFE = CalculateB_DFE(min1 * min4, max1 * max4, listX14, listY);
            b23_DFE = CalculateB_DFE(min2 * min3, max2 * max3, listX23, listY);
            b24_DFE = CalculateB_DFE(min2 * min4, max2 * max4, listX24, listY);
            b34_DFE = CalculateB_DFE(min3 * min4, max3 * max4, listX34, listY);
            b123_DFE = CalculateB_DFE(min1 * min2 * min3, max1 * max2 * max3, listX123, listY);
            b124_DFE = CalculateB_DFE(min1 * min2 * min4, max1 * max2 * max4, listX124, listY);
            b134_DFE = CalculateB_DFE(min1 * min3 * min4, max1 * max3 * max4, listX134, listY);
            b234_DFE = CalculateB_DFE(min2 * min3 * min4, max2 * max3 * max4, listX234, listY);
            b1234_DFE = CalculateB_DFE(min1 * min2 * min3 * min4, max1 * max2 * max3 * max4, listX1234, listY);

            for (int i = 0; i < n_DFE; ++i)
            {
                List<double> bl = new List<double> { b0_DFE, b1_DFE, b2_DFE, b3_DFE, b4_DFE };
                List<double> xl = new List<double> { listX0[i], listX1[i], listX2[i], listX3[i], listX4[i] };

                double yl = Math.Round(MultiplyCoefficients(bl, xl), 5);

                List<double> bcn = new List<double>
                {
                    b0_DFE,
                    b1_DFE,
                    b2_DFE,
                    b3_DFE,
                    b4_DFE,
                    b12_DFE,
                    b13_DFE,
                    b14_DFE,
                    b23_DFE,
                    b24_DFE,
                    b34_DFE,
                    b123_DFE,
                    b124_DFE,
                    b134_DFE,
                    b234_DFE,
                    b1234_DFE,
                };

                List<double> xcn = new List<double>
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
                };

                double ycn = Math.Round(MultiplyCoefficients(bcn, xcn), 5);

                yl = Math.Abs(yl);
                ycn = Math.Abs(ycn);

                ListView_TableParametersDFE.Items.Add(new EquationCoefffcients(
                    i + 1,
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
                    listY[i],
                    yl,
                    ycn,
                    Math.Round(Math.Abs(listY[i] - yl), 5),
                    Math.Round(Math.Abs(listY[i] - ycn), 5)
                ));
            }

            ListView_TableResultsDFE.Items.Clear();
            ListView_TableResultsDFE.Items.Add(new EquationResult(
                b0_DFE,
                b1_DFE,
                b2_DFE,
                b3_DFE,
                b4_DFE,
                b13_DFE,
                b12_DFE,
                b14_DFE,
                b23_DFE,
                b24_DFE,
                b34_DFE,
                b123_DFE,
                b124_DFE,
                b134_DFE,
                b234_DFE,
                b1234_DFE
            ));
        }

        private void AddPointPFE(double lambdaComing1, double lambdaComing2, double lambdaProcessing1, double lambdaProcessing2)
        {
            n_PFE++;
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

            List<double> bl = new List<double> { b0_PFE, b1_PFE, b2_PFE, b3_PFE, b4_PFE };
            List<double> xl = new List<double> { x0, x1, x2, x3, x4 };

            double yl = Math.Round(MultiplyCoefficients(bl, xl), 5);

            List<double> bcn = new List<double>
            {
                b0_PFE,
                b1_PFE,
                b2_PFE,
                b3_PFE,
                b4_PFE,
                b12_PFE,
                b13_PFE,
                b14_PFE,
                b23_PFE,
                b24_PFE,
                b34_PFE,
                b123_PFE,
                b124_PFE,
                b134_PFE,
                b234_PFE,
                b1234_PFE,
            };

            List<double> xcn = new List<double>
            {
                x0,
                x1,
                x2,
                x3,
                x4,
                x12,
                x13,
                x14,
                x23,
                x24,
                x34,
                x123,
                x124,
                x134,
                x234,
                x1234,
            };

            double ycn = Math.Round(MultiplyCoefficients(bcn, xcn), 5);

            yl = Math.Abs(yl);
            ycn = Math.Abs(ycn);

            ListView_TableParameters.Items.Add(new EquationCoefffcients(
                n_PFE,
                x0,
                x1,
                x2,
                x3,
                x4,
                x12,
                x13,
                x14,
                x23,
                x24,
                x34,
                x123,
                x124,
                x134,
                x234,
                x1234,
                y,
                yl,
                ycn,
                Math.Round(Math.Abs(y - yl), 5),
                Math.Round(Math.Abs(y - ycn), 5)
             ));
        }

        private void AddPointDFE(double lambdaComing1, double lambdaComing2, double lambdaProcessing1, double lambdaProcessing2)
        {
            n_DFE++;
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

            List<double> bl = new List<double> { b0_DFE, b1_DFE, b2_DFE, b3_DFE, b4_DFE };
            List<double> xl = new List<double> { x0, x1, x2, x3, x4 };

            double yl = Math.Round(MultiplyCoefficients(bl, xl), 5);

            List<double> bcn = new List<double>
            {
                b0_DFE,
                b1_DFE,
                b2_DFE,
                b3_DFE,
                b4_DFE,
                b12_DFE,
                b13_DFE,
                b14_DFE,
                b23_DFE,
                b24_DFE,
                b34_DFE,
                b123_DFE,
                b124_DFE,
                b134_DFE,
                b234_DFE,
                b1234_DFE,
            };

            List<double> xcn = new List<double>
            {
                x0,
                x1,
                x2,
                x3,
                x4,
                x12,
                x13,
                x14,
                x23,
                x24,
                x34,
                x123,
                x124,
                x134,
                x234,
                x1234,
            };

            double ycn = Math.Round(MultiplyCoefficients(bcn, xcn), 5);

            yl = Math.Abs(yl);
            ycn = Math.Abs(ycn);

            ListView_TableParametersDFE.Items.Add(new EquationCoefffcients(
                n_DFE,
                x0,
                x1,
                x2,
                x3,
                x4,
                x12,
                x13,
                x14,
                x23,
                x24,
                x34,
                x123,
                x124,
                x134,
                x234,
                x1234,
                y,
                yl,
                ycn,
                Math.Round(Math.Abs(y - yl), 5),
                Math.Round(Math.Abs(y - ycn), 5)
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
