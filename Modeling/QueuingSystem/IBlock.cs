namespace Modeling.QueuingSystem
{
    public interface IBlock
    {
        double Next { get; set; }
        double Delay();
    }
}
