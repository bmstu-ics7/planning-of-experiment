namespace Lab01.QueuingSystem
{
    public interface IBlock
    {
        uint Next { get; set; }
        uint Delay();
    }
}
