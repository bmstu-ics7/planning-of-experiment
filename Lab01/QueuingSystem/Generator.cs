using System.Collections.Generic;
using Lab01.Distributions;

namespace Lab01.QueuingSystem
{
    public class Generator : IBlock
    {
        private readonly IDisctribution _disctribution;

        private IList<Operator> _recievers;

        private int _count;

        public int Count
        {
            get => _count;
        }

        private uint _next;

        public uint Next
        {
            get => _next;
            set => _next = value > 0 ? value : 0;
        }

        public Generator(IDisctribution disctribution, IList<Operator> recievers, int count)
        {
            _disctribution = disctribution;
            _recievers = recievers;
            _count = count;
            _next = 0;
        }

        public Operator GenerateRequest(uint currentTime)
        {
            _count--;

            foreach (var r in _recievers)
            {
                if (r.ResieveRequest(currentTime))
                {
                    return r;
                }
            }

            return null;
        }

        public uint Delay() => _disctribution.Generate();
    }
}
