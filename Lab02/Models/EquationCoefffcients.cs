namespace Lab02.Models
{
    public class EquationCoefffcients
    {
        public double N { get; set; }
        public double X0 { get; set; }
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double X12 { get; set; }
        public double Y { get; set; }
        public double Yl { get; set; }
        public double Ycn { get; set; }
        public double YmYl { get; set; }
        public double YmYcn { get; set; }

        public EquationCoefffcients(double n, double x0, double x1, double x2, double x12, double y, double yl, double ycn, double ymYl, double ymYcn)
        {
            N = n;
            X0 = x0;
            X1 = x1;
            X2 = x2;
            X12 = x12;
            Y = y;
            Yl = yl;
            Ycn = ycn;
            YmYl = ymYl;
            YmYcn = ymYcn;
        }
    }
}
