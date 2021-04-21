using System.Collections.Generic;
using System.Windows.Controls;

namespace FullFactorExperiment
{
    /// <summary>
    /// Логика взаимодействия для FullFactorExperimentElement.xaml
    /// </summary>
    public partial class FullFactorExperimentElement : UserControl
    {
        private List<double> minValues;
        private List<double> maxValues;
        private int count;

        public FullFactorExperimentElement()
        {
            InitializeComponent();
        }

        public void UpdateValues(List<double> minValues, List<double> maxValues, int count)
        {
            this.minValues = minValues;
            this.maxValues = maxValues;
            this.count = count;

            ListView_TableParameters.Col
        }
    }
}
