using Lab01.Distributions;
using System.Collections.Generic;

namespace Lab01.QueuingSystem
{
    public class Operator : IBlock
    {
        private readonly IDisctribution _disctribution;

        private readonly uint _maxQueue;

        private uint _queue;

        private readonly Queue<uint> _queueTimes;

        public uint Queue => _queue;

        private uint _next;

        public uint Next
        {
            get => _next;
            set => _next = value > 0 ? value : 0;
        }

        public Operator(IDisctribution disctribution, uint maxQueue = 0)
        {
            _disctribution = disctribution;
            _maxQueue = maxQueue;
            _queue = 0;
            _next = 0;
            _queueTimes = new Queue<uint>();
        }

        public bool ResieveRequest(uint currentTime)
        {
            if (_maxQueue == 0 || _maxQueue > _queue)
            {
                _queue++;
                _queueTimes.Enqueue(currentTime);
                return true;
            }

            return false;
        }

        public uint ProcessRequest()
        {
            if (_queue > 0)
            {
                _queue--;
                return _queueTimes.Dequeue();
            }

            return 0;
        }

        public uint Delay() => _disctribution.Generate();
    }
}
