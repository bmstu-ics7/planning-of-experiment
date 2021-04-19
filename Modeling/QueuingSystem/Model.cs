using System.Collections.Generic;
using System.Linq;
using System;

namespace Modeling.QueuingSystem
{
    public class Model
    {
        private readonly Generator _generator;

        private IList<IBlock> _blocks;

        public Model(Generator generator, List<IBlock> blocks)
        {
            _generator = generator;
            _blocks = blocks;
        }

        public ModelResult Generate()
        {
            uint time = 0;
            List<int> timesWait = new List<int>();

            while (_generator.Count > 0)
            {
                Console.WriteLine(time);
                time = _generator.Next;
                foreach (var block in _blocks)
                {
                    if (0 < block.Next && block.Next < time)
                    {
                        time = block.Next;
                    }
                }

                foreach (var block in _blocks)
                {
                    if (time == block.Next)
                    {
                        if (block.GetType().Equals(typeof(Generator)))
                        {
                            var next = ((Generator)block).GenerateRequest(time);
                            if (next != null)
                            {
                                next.Next = time + next.Delay();
                            }

                            var del = block.Delay();
                            block.Next = time + del;
                        }
                        else
                        {
                            uint startTime = ((Operator)block).ProcessRequest();
                            timesWait.Add((int)(time - startTime));

                            if (((Operator)block).Queue == 0)
                            {
                                block.Next = 0;
                            }
                            else
                            {
                                block.Next = time + block.Delay();
                            }
                        }
                    }
                }
            }

            return new ModelResult
            {
                Time = time,
                AverageTime = timesWait.Count > 0 ? timesWait.Average() : 0,
            };
        }
    }
}
