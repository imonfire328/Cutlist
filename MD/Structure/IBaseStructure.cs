using System;
using CLGenerator.MD.MdMeasure;

namespace CLGenerator.MD.MdStructure
{
    public interface IBaseStructure : IDimension
    {
        double InsideWidth();
        double InsideHeight();
        double InsideDepth();
        MDepth Depth { get;}
        MThick BackThickness { get;}
        MdDimension Dim { get; }
    }

    public interface IStructure : IDimension{
    }
}
