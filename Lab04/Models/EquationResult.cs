namespace Lab04.Models
{
    public class EquationResult
    {
        public double B0 { get; set; }
        public double B1 { get; set; }
        public double B2 { get; set; }
        public double B3 { get; set; }
        public double B4 { get; set; }
        public double B12 { get; set; }
        public double B13 { get; set; }
        public double B14 { get; set; }
        public double B23 { get; set; }
        public double B24 { get; set; }
        public double B34 { get; set; }
        public double B123 { get; set; }
        public double B124 { get; set; }
        public double B134 { get; set; }
        public double B234 { get; set; }
        public double B1234 { get; set; }
        public double B11 { get; set; }
        public double B22 { get; set; }
        public double B33 { get; set; }
        public double B44 { get; set; }

        public EquationResult(double b0, double b1, double b2, double b3, double b4, double b12, double b13, double b14, double b23, double b24, double b34, double b123, double b124, double b134, double b234, double b1234, double b11, double b22, double b33, double b44)
        {
            B0 = b0;
            B1 = b1;
            B2 = b2;
            B3 = b3;
            B4 = b4;
            B12 = b12;
            B13 = b13;
            B14 = b14;
            B23 = b23;
            B24 = b24;
            B34 = b34;
            B123 = b123;
            B124 = b124;
            B134 = b134;
            B234 = b234;
            B1234 = b1234;
            B11 = b11;
            B22 = b22;
            B33 = b33;
            B44 = b44;
        }
    }
}
