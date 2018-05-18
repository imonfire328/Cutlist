using System;
namespace CLGenerator.ST
{
    public interface IStrategy<T>
    {
        T Implement();
    }
}
