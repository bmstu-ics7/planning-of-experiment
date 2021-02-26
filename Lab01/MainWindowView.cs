using System.Collections.Generic;
using OxyPlot;

namespace Lab01
{
    public class MainWindowView
    {
        public MainWindowView()
        {
            this.Title = "Test";
            this.Points = new List<DataPoint>();

            for (double x = -10; x <= 10; x += 0.1)
            {
                Points.Add(new DataPoint(x, x * x));
            }
        }

        public string Title { get; set; }

        public IList<DataPoint> Points { get; set; }
    }
}
