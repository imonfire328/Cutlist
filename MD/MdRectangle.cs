using System;
namespace CLGenerator.MD
{
    public class MdRectangle
    {
        public double X { get; private set;  }
        public double Y { get; private set;  }

        public MdRectangle(double x, double y)
        {
            X = x;
            Y = y;
        }

        public MdRectangle(MdDimension dim){
            X = dim.X;
            Y = dim.Y;
        }

        public MdRectangle(MdRectangle rect){
            X = rect.X;
            Y = rect.Y;
        }

        public double CalcArea()
        {
            return X * Y;
        }


        public override string ToString()
        {
            return X + "x" + Y;
        }

    }
}
