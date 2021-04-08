using System;

namespace Lab01.Distributions
{
    internal class Rayleigh : IDisctribution
    {
        private readonly double _sigma;

        private readonly Random _rnd;

        static public double ConvertLambdaToSigma(double lambda)
            => (1.0 / lambda) / Math.Sqrt(Math.PI / 2.0);

        public Rayleigh(double sigma)
        {
            if (sigma <= 0)
            {
                throw new ArgumentException("Параметр sigma должен быть больше или равен нуля.");
            }

            _sigma = sigma;
            _rnd = new Random();
        }

        public uint Generate()
        {
            double value = _sigma + (Math.Sqrt(-2 * Math.Log(1 - _rnd.NextDouble())) - Math.Sqrt(Math.PI / 2)) * _sigma;
            value = value > 0 ? value : 0;
            return Convert.ToUInt32(value);
        }

        private double Density(double x) =>
            x / Math.Pow(_sigma, 2) * Math.Exp(-Math.Pow(x, 2) / (2 * Math.Pow(_sigma, 2)));
    }
}