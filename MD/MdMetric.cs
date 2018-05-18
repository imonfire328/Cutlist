using System;
namespace CLGenerator.MD
{

    public interface IMetric 
    {
        string ToBinary();
    }

    public abstract class MdMetric : IMetric
    {
        public MdMetric(){}

        public string ToBinary(double i, double k)
        {
            return Convert.ToString(Convert.ToByte(i), 2) + Convert.ToString(Convert.ToByte(i), 2);
        }

        public abstract string ToBinary();
    }
}
