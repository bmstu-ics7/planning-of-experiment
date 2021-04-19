using System;
using System.Windows;
using System.Windows.Controls;
using Lab02.Models;

namespace Lab02
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ListView_TableParameters.Items.Add(new EquationCoefffcients
            {
                N = 1,
                X0 = 2,
                X1 = 3,
                X2 = 4,
                X12 = 5,
                Y = 6,
                Ycn = 7,
                Yl = 8,
                YmYl = 9,
                YmYcn = 10,
            });
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

        private void Button_StartExperiment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_AddPoint_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
