namespace Lab02.Models
{
    public class EquationResult
    {
        public double B0 { get; set; }
        public double B1 { get; set; }
        public double B2 { get; set; }
        public double B12 { get; set; }

        public EquationResult(double b0, double b1, double b2, double b12)
        {
            B0 = b0;
            B1 = b1;
            B2 = b2;
            B12 = b12;
        }
    }
}
