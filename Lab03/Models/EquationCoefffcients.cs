namespace Lab03.Models
{
    public class EquationCoefffcients
    {
        public double N { get; set; }
        public double X0 { get; set; }
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double X3 { get; set; }
        public double X4 { get; set; }
        public double X12 { get; set; }
        public double X13 { get; set; }
        public double X14 { get; set; }
        public double X23 { get; set; }
        public double X24 { get; set; }
        public double X34 { get; set; }
        public double X123 { get; set; }
        public double X124 { get; set; }
        public double X134 { get; set; }
        public double X234 { get; set; }
        public double X1234 { get; set; }
        public double Y { get; set; }
        public double Yl { get; set; }
        public double Ycn { get; set; }
        public double YmYl { get; set; }
        public double YmYcn { get; set; }

        public EquationCoefffcients(double n, double x0, double x1, double x2, double x3, double x4, double x12, double x13, double x14, double x23, double x24, double x34, double x123, double x124, double x134, double x234, double x1234, double y, double yl, double ycn, double ymYl, double ymYcn)
        {
            N = n;
            X0 = x0;
            X1 = x1;
            X2 = x2;
            X3 = x3;
            X4 = x4;
            X12 = x12;
            X13 = x13;
            X14 = x14;
            X23 = x23;
            X24 = x24;
            X34 = x34;
            X123 = x123;
            X124 = x124;
            X134 = x134;
            X234 = x234;
            X1234 = x1234;
            Y = y;
            Yl = yl;
            Ycn = ycn;
            YmYl = ymYl;
            YmYcn = ymYcn;
        }
    }
}
