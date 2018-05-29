using System;
using CLGenerator.MD.MdStructure;

namespace CLGenerator.MD.Structure
{
    // a case that contains a base structure
    public interface IMdCase
    {
        IMdBaseCabinet BaseStructure { get; }
    }
}
