using System.Collections.Generic;
using System.Linq;
using System;

namespace Modeling.QueuingSystem
{
    public class Model
    {
        private readonly IList<Generator> _generators;

        private IList<IBlock> _blocks;

        public Model(Generator generator, List<IBlock> blocks)
        {
            _generators = new List<Generator> { generator };
            _blocks = blocks;
        }

        public Model(List<Generator> generators, List<IBlock> blocks)
        {
            _generators = generators;
            _blocks = blocks;
        }

        public ModelResult Generate()
        {
            double time = 0;
            List<int> timesWait = new List<int>();

            while (true)
            {
                bool exit = true;
                foreach (var gen in _generators)
                {
                    if (gen.Count > 0)
                    {
                        exit = false;
                        break;
                    }
                }

                if (exit) break;

                time = _generators[0].Next;
                foreach (var gen in _generators)
                {
                    if (0 < gen.Next && gen.Next < time)
                    {
                        time = gen.Next;
                    }
                }

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
                            double startTime = ((Operator)block).ProcessRequest();
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
