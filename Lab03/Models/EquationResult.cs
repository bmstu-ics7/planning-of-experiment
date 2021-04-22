namespace Lab03.Models
{
    public class EquationResult
    {
        public double B0 { get; set; }
        public double B1 { get; set; }
        public double B2 { get; set; }
        public double B3 { get; set; }
        public double B12 { get; set; }
        public double B13 { get; set; }
        public double B23 { get; set; }
        public double B123 { get; set; }

        public EquationResult(double b0, double b1, double b2, double b3, double b12, double b13, double b23, double b123)
        {
            B0 = b0;
            B1 = b1;
            B2 = b2;
            B3 = b3;
            B12 = b12;
            B13 = b13;
            B23 = b23;
            B123 = b123;
        }
    }
}
