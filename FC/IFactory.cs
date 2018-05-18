using System;
namespace CLGenerator.FC
{
    public interface IFactory<T>
    {
        T GetInstance();
    }
}
