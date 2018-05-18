using System;
namespace CLGenerator.MD
{
    public class MdPoint : MdMetric
    {
        public double X { get; private set; }
        public double Y { get; private set; }

        public MdPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToBinary()
        {
            return base.ToBinary(X, Y);
        }

        public bool Compare(MdPoint p)
        {
            if (p.Y == Y && p.X == X)
                return true;
            return false;
        }
    }
}
