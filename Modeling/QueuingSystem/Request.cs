namespace Modeling.QueuingSystem
{
    public class Request
    {
        public int GeneratorType;
        public double TimeStart;

        public Request(int generatorType, double timeStart)
        {
            GeneratorType = generatorType;
            TimeStart = timeStart;
        }
    }
}
