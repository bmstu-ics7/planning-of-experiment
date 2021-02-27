using System.Collections.Generic;
using System.Linq;

namespace Lab01.QueuingSystem
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

        public double Generate()
        {
            uint time = 0;
            List<int> timesWait = new List<int>();

            while (_generator.Count >= 0)
            {
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

                            block.Next = time + block.Delay();
                        }
                        else
                        {
                            uint startTime = ((Operator)block).ProcessRequest();

                            if (startTime > 0)
                            {
                                timesWait.Add((int)(time - startTime));
                            }

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

            return timesWait.Average();
        }
    }
}
