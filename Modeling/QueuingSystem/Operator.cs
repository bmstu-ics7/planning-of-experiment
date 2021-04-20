using Modeling.Distributions;
using System.Collections.Generic;

namespace Modeling.QueuingSystem
{
    public class Operator : IBlock
    {
        private readonly IDistribution _distribution;

        private readonly uint _maxQueue;

        private uint _queue;

        private readonly Queue<double> _queueTimes;

        public uint Queue => _queue;

        private double _next;

        public double Next
        {
            get => _next;
            set => _next = value > 0 ? value : 0;
        }

        public Operator(IDistribution distribution, uint maxQueue = 0)
        {
            _distribution = distribution;
            _maxQueue = maxQueue;
            _queue = 0;
            _next = 0;
            _queueTimes = new Queue<double>();
        }

        public bool ResieveRequest(double currentTime)
        {
            if (_maxQueue == 0 || _maxQueue > _queue)
            {
                _queue++;
                _queueTimes.Enqueue(currentTime);
                return true;
            }

            return false;
        }

        public double ProcessRequest()
        {
            if (_queue > 0)
            {
                _queue--;
                return _queueTimes.Dequeue();
            }

            return 0;
        }

        public double Delay() => _distribution.Generate();
    }
}
