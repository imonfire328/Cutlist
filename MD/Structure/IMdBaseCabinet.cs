using System;
using CLGenerator.MD.MdMaterial;
using CLGenerator.MD.MdMeasure;
using CLGenerator.MD.Structure;

namespace CLGenerator.MD.MdStructure
{
    public interface IMdBaseCabinet : IMdDimension
    {
        double InsideWidth();
        double InsideHeight();
        double InsideDepth();
        double InsideRiseHeight();
        MDepth Depth { get; }
        Material BackMaterial { get; }
        MdDimension Dim { get; }
        MWidth Rise { get; }
        IMdCabinetAccessory Additions { get; }


    }

    public interface IMdCabinet : IMdDimension{
    }
}
