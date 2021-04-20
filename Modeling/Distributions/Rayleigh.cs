using System;
using Troschuetz.Random.Distributions.Continuous;

namespace Modeling.Distributions
{
    public class Rayleigh : IDistribution
    {

        private readonly RayleighDistribution rayleighDistribution;

        static public double ConvertLambdaToSigma(double lambda)
            => (1.0 / lambda) * Math.Pow(Math.PI / 2.0, -0.5);

        public Rayleigh(double sigma)
        {
            if (sigma <= 0)
            {
                throw new ArgumentException("Параметр sigma должен быть больше или равен нуля.");
            }

            rayleighDistribution = new RayleighDistribution(sigma);
        }

        public double Generate()
            => rayleighDistribution.NextDouble();
    }
}