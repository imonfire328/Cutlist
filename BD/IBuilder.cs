using System;
namespace CLGenerator.BD
{
    public interface IBuilder<T>
    {
        T Build();
    }
}
