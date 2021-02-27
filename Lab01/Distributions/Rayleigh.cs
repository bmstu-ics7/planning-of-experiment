using System;

namespace Lab01.Distributions
{
    internal class Rayleigh : IDisctribution
    {
        private readonly double _sigma;

        private readonly Random _rnd;

        public Rayleigh(double sigma)
        {
            if (sigma <= 0)
            {
                throw new ArgumentException("Argument sigma should be more or equal that zero.");
            }

            _sigma = sigma;
            _rnd = new Random();
        }

        public uint Generate() => (uint)Density(_rnd.Next(0, 1));

        private double Density(double x) =>
            x / Math.Pow(_sigma, 2) * Math.Exp(-Math.Pow(x, 2) / (2 * Math.Pow(_sigma, 2)));
    }
}
