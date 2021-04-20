using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
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
        private readonly int countExp = 20;

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

        private void Button_StartExperiment_Click(object sender, RoutedEventArgs e)
        {
            bool parseMinComing = double.TryParse(TextBox_MinComing.Text, out double minLambdaComing);
            bool parseMaxComing = double.TryParse(TextBox_MaxComing.Text, out double maxLambdaComing);
            bool parseMinProcessing = double.TryParse(TextBox_MinProcessing.Text, out double minLambdaProcessing);
            bool parseMaxProcessing = double.TryParse(TextBox_MaxProcessing.Text, out double maxLambdaProcessing);
            bool parseCount = int.TryParse(TextBox_Count.Text, out int count);

            if (parseMinComing && parseMaxComing && parseMinProcessing && parseMaxProcessing && parseCount)
            {
                ListView_TableParameters.Items.Clear();

                for (int i = 1; i <= 4; ++i)
                {
                    int n = i;
                    int x0 = 1;
                    int x1 = i % 2 == 0 ? 1 : -1;
                    int x2 = (i - 1) / 2 % 2 == 0 ? -1 : 1;
                    int x12 = x1 * x2;

                    double lambdaComing = ConvertFactorToValue(minLambdaComing, maxLambdaComing, x1);
                    double lambdaProcessing = ConvertFactorToValue(minLambdaProcessing, maxLambdaProcessing, x2);

                    double sigmaComing = Rayleigh.ConvertLambdaToSigma(lambdaComing);
                    double sigmaProcessing = Rayleigh.ConvertLambdaToSigma(lambdaProcessing);

                    var comingDistribution = new Rayleigh(sigmaComing);
                    var proecssingDisctribution = new Rayleigh(sigmaProcessing);

                    double y = 0;
                    for (int exp = 0; exp < countExp; ++exp)
                    {
                        ModelResult result = CalculateModel(comingDistribution, proecssingDisctribution, count);
                        y += result.AverageTime;
                    }
                    y = Math.Round(y / (double)countExp, 5);

                    ListView_TableParameters.Items.Add(new EquationCoefffcients(n, x0, x1, x2, x12, y, 0, 0, 0, 0));
                }
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
