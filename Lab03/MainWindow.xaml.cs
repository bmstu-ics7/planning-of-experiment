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

        private readonly int widthXColumns = 35;

        private double b0_PFE;
        private double b1_PFE;
        private double b2_PFE;
        private double b3_PFE;
        private double b12_PFE;
        private double b13_PFE;
        private double b23_PFE;
        private double b123_PFE;

        private double b0_DFE;
        private double b1_DFE;
        private double b2_DFE;
        private double b3_DFE;
        private double b12_DFE;
        private double b13_DFE;
        private double b23_DFE;
        private double b123_DFE;

        private double minLambdaComing1;
        private double maxLambdaComing1;
        private double minLambdaComing2;
        private double maxLambdaComing2;
        private double minLambdaProcessing;
        private double maxLambdaProcessing;
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
                    countX++;
                    col.Width = widthXColumns;
                }
            }

            double workingWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth - countX * widthXColumns;
            double count = gView.Columns.Count - countX;
            int columnWidth = (int)Math.Round(workingWidth / count);

            foreach (var col in gView.Columns)
            {
                if (col.Header.ToString()[0] != 'x' && col.Header.ToString()[0] != '№')
                {
                    col.Width = columnWidth;
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
            bool parseProcessing = double.TryParse(TextBox_PointProcessing.Text, out double lambdaProcessing);

            if (parseComing1 && parseComing2 && parseProcessing)
            {
                AddPointPFE(lambdaComing1, lambdaComing2, lambdaProcessing);
                AddPointDFE(lambdaComing1, lambdaComing2, lambdaProcessing);
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
            var listX12 = new List<int>();
            var listX13 = new List<int>();
            var listX23 = new List<int>();
            var listX123 = new List<int>();
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

            b0_PFE = CalculateB_PFE(0, 1, listX0, listY);
            b1_PFE = CalculateB_PFE(minLambdaComing1, maxLambdaComing1, listX1, listY);
            b2_PFE = CalculateB_PFE(minLambdaComing2, maxLambdaComing2, listX2, listY);
            b3_PFE = CalculateB_PFE(minLambdaProcessing, maxLambdaProcessing, listX3, listY);
            b12_PFE = CalculateB_PFE(minLambdaComing1 * minLambdaComing2, maxLambdaComing1 * maxLambdaComing2, listX12, listY);
            b13_PFE = CalculateB_PFE(minLambdaComing1 * minLambdaProcessing, maxLambdaComing1 * maxLambdaProcessing, listX13, listY);
            b23_PFE = CalculateB_PFE(minLambdaComing2 * minLambdaProcessing, maxLambdaComing2 * maxLambdaProcessing, listX23, listY);
            b123_PFE = CalculateB_PFE(minLambdaComing1 * minLambdaComing2 * minLambdaProcessing, maxLambdaComing1 * maxLambdaComing2 * maxLambdaProcessing, listX23, listY);

            for (int i = 0; i < n_PFE; ++i)
            {
                double yl = Math.Round(b0_PFE + listX1[i] * b1_PFE + listX2[i] * b2_PFE + listX3[i] * b3_PFE, 5);
                double ycn = Math.Round(b0_PFE + listX1[i] * b1_PFE + listX2[i] * b2_PFE + listX3[i] * b3_PFE + listX12[i] * b12_PFE + listX13[i] * b13_PFE + listX23[i] * b23_PFE + listX123[i] * b123_PFE, 5);

                yl = Math.Abs(yl);
                ycn = Math.Abs(ycn);

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
            ListView_TableResults.Items.Add(new EquationResult(b0_PFE, b1_PFE, b2_PFE, b3_PFE, b12_PFE, b13_PFE, b23_PFE, b123_PFE));
        }

        private void ModelDFE()
        {
            ListView_TableParametersDFE.Items.Clear();

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

            for (int i = 1; i <= countAllExperiments / 2; ++i)
            {
                n_DFE = i;
                int x0 = 1;
                listX0.Add(x0);

                int x1 = i % 2 == 0 ? 1 : -1;
                listX1.Add(x1);

                int x2 = (i - 1) / 2 % 2 == 0 ? -1 : 1;
                listX2.Add(x2);

                int x3 = x1 * x2;
                listX3.Add(x3);

                int x12 = x1 * x2;
                listX12.Add(x12);

                int x13 = x1 * x3;
                listX13.Add(x13);

                int x23 = x2 * x3;
                listX23.Add(x23);

                int x123 = 1;
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

            b0_DFE = CalculateB_DFE(0, 1, listX0, listY);
            b1_DFE = CalculateB_DFE(minLambdaComing1, maxLambdaComing1, listX1, listY);
            b2_DFE = CalculateB_DFE(minLambdaComing2, maxLambdaComing2, listX2, listY);
            b3_DFE = CalculateB_DFE(minLambdaProcessing, maxLambdaProcessing, listX3, listY);
            b12_DFE = CalculateB_DFE(minLambdaComing1 * minLambdaComing2, maxLambdaComing1 * maxLambdaComing2, listX12, listY);
            b13_DFE = CalculateB_DFE(minLambdaComing1 * minLambdaProcessing, maxLambdaComing1 * maxLambdaProcessing, listX13, listY);
            b23_DFE = CalculateB_DFE(minLambdaComing2 * minLambdaProcessing, maxLambdaComing2 * maxLambdaProcessing, listX23, listY);
            b123_DFE = CalculateB_DFE(minLambdaComing1 * minLambdaComing2 * minLambdaProcessing, maxLambdaComing1 * maxLambdaComing2 * maxLambdaProcessing, listX23, listY);

            for (int i = 0; i < n_DFE; ++i)
            {
                double yl = Math.Round(b0_DFE + listX1[i] * b1_DFE + listX2[i] * b2_DFE + listX3[i] * b3_DFE, 5);
                double ycn = Math.Round(b0_DFE + listX1[i] * b1_DFE + listX2[i] * b2_DFE + listX3[i] * b3_DFE + listX12[i] * b12_DFE + listX13[i] * b13_DFE + listX23[i] * b23_DFE + listX123[i] * b123_DFE, 5);

                yl = Math.Abs(yl);
                ycn = Math.Abs(ycn);

                ListView_TableParametersDFE.Items.Add(new EquationCoefffcients(
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

            ListView_TableResultsDFE.Items.Clear();
            ListView_TableResultsDFE.Items.Add(new EquationResult(b0_DFE, b1_DFE, b2_DFE, b3_DFE, b12_DFE, b13_DFE, b23_DFE, b123_DFE));
        }

        private void AddPointPFE(double lambdaComing1, double lambdaComing2, double lambdaProcessing)
        {
            n_PFE++;
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

            double yl = Math.Round(b0_PFE + x1 * b1_PFE + x2 * b2_PFE + x3 * b3_PFE, 5);
            double ycn = Math.Round(b0_PFE + x1 * b1_PFE + x2 * b2_PFE + x3 * b3_PFE + x12 * b12_PFE + x13 * b13_PFE + x23 * b23_PFE + x123 * b123_PFE, 5);

            yl = Math.Abs(yl);
            ycn = Math.Abs(ycn);

            ListView_TableParameters.Items.Add(new EquationCoefffcients(
                n_PFE,
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

        private void AddPointDFE(double lambdaComing1, double lambdaComing2, double lambdaProcessing)
        {
            n_DFE++;
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

            double yl = Math.Round(b0_DFE + x1 * b1_DFE + x2 * b2_DFE + x3 * b3_DFE, 5);
            double ycn = Math.Round(b0_DFE + x1 * b1_DFE + x2 * b2_DFE + x3 * b3_DFE + x12 * b12_DFE + x13 * b13_DFE + x23 * b23_DFE + x123 * b123_DFE, 5);

            yl = Math.Abs(yl);
            ycn = Math.Abs(ycn);

            ListView_TableParametersDFE.Items.Add(new EquationCoefffcients(
                n_DFE,
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

        private ModelResult CalculateModel(IDistribution generatorDistribution1, IDistribution generatorDistribution2, IDistribution timeDistribution, int count)
        {
            var op = new Operator(timeDistribution);
            var generator1 = new Generator(generatorDistribution1, new List<Operator> { op }, count / 2);
            var generator2 = new Generator(generatorDistribution2, new List<Operator> { op }, count / 2);
            var model = new Model(new List<Generator> { generator1, generator2 }, new List<IBlock> { generator1, generator2, op });
            return model.Generate();
        }
    }
}
