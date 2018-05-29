using System;
using System.Collections.Generic;
using CLGenerator.MD.MdStructure;

namespace CLGenerator.MD.Structure
{
    /// <summary>
    /// A fixture requireing the base structure to build from 
    /// </summary>
    public interface IMdCabinetAccessory
    {
        IMdCabinetAccessory Dec { get; }
        List<MdDimension> PlaceAccessory(List<MdDimension> dims, IMdBaseCabinet baseStructure);
    }
}
