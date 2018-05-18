using System;
namespace CLGenerator.MD
{
    /// <summary>
    /// Represents any area of space with a seperate height and width
    /// </summary>
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

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:CLGenerator.MD.MdDimension"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:CLGenerator.MD.MdDimension"/>.</returns>
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
