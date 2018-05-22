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
    }
}
