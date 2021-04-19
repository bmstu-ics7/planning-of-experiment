namespace Modeling.QueuingSystem
{
    public interface IBlock
    {
        uint Next { get; set; }
        uint Delay();
    }
}
