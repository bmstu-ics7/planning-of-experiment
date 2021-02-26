using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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

            this.DataContext = new MainWindowView();
            Oxyplot_Test.InvalidatePlot(true);
        }

        private void Button_Calculate_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindowView)this.DataContext).Title = "Click";
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
