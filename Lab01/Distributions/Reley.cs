using System;

namespace Lab01.Distributions
{
    public class Reley
    {
        private readonly double _sigma;

        public Reley(double sigma)
        {
            if (sigma < 0)
            {
                throw new ArgumentException("Parametr sigma should be more or equal that zero.");
            }

            _sigma = sigma;
        }

        public double Density(double x) =>
            x / Math.Pow(_sigma, 2) * Math.Exp(-Math.Pow(x, 2) / (2 * Math.Pow(_sigma, 2)));
    }
}
