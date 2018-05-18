using System;
namespace CLGenerator.MD
{
    public class MdDimension : MdMetric
    {
        public double Width { get; private set; }
        public double Height { get; private set; }
        public int Id { get; private set; }


        public MdDimension(int id, MdDimension dim)
        {
            Id = id;
            Width = dim.Width;
            Height = dim.Height;
        }

        public MdDimension(double height, double width)
        {
            Width = width;
            Height = height;
        }


        public bool GreaterThan(MdDimension dim)
        {
            return dim.CalcArea() > CalcArea() ? false : true;
        }


        public double CalcArea()
        {
            return Width * Height;
        }


        public override string ToString()
        {
            return Width + "x" + Height;
        }


        public override string ToBinary()
        {
            return base.ToBinary(Width, Height);
        }
    }

    public interface IDimension 
    {
        
    }


    public class MdDimComposite : MdDimension
    {
        MdPoint _link;
        public MdDimComposite(double height, double width, MdPoint link) : base(height, width)
        {
            _link = link;
        }
    }
}
