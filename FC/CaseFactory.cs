using System;
using CLGenerator.MD;

namespace CLGenerator.FC
{
    public class CaseFactory : IFactory<IMdDimension>
    {
        public CaseFactory()
        {
        }

        public IMdDimension GetInstance()
        {
            throw new NotImplementedException();
        }
    }
}
