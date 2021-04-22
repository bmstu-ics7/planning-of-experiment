using Modeling.Distributions;
using System.Collections.Generic;

namespace Modeling.QueuingSystem
{
    public class Operator : IBlock
    {
        private readonly List<IDistribution> _distributions;

        private readonly uint _maxQueue;

        private uint _queue;

        private readonly Queue<Request> _queueRequests;

        public uint Queue => _queue;

        private double _next;

        public double Next
        {
            get => _next;
            set => _next = value > 0 ? value : 0;
        }

        public Operator(List<IDistribution> distributions, uint maxQueue = 0)
        {
            _distributions = distributions;
            _maxQueue = maxQueue;
            _queue = 0;
            _next = 0;
            _queueRequests = new Queue<Request>();
        }

        public bool ResieveRequest(double currentTime, int generatorType)
        {
            if (_maxQueue == 0 || _maxQueue > _queue)
            {
                _queue++;
                _queueRequests.Enqueue(new Request(generatorType, currentTime));
                return true;
            }

            return false;
        }

        public double ProcessRequest()
        {
            if (_queue > 0)
            {
                _queue--;
                return _queueRequests.Dequeue().TimeStart;
            }

            return 0;
        }

        public double Delay() => _distributions[_queueRequests.Peek().GeneratorType].Generate();
    }
}
