using System;
using System.Collections.Generic;
using CLGenerator.MD.MdMeasure;
using CLGenerator.MD.MdStructure;

namespace CLGenerator.MD.Structure
{
    /// <summary>
    /// Md shelf addition.
    /// </summary>
    public class MdShelfAccessory : IMdCabinetAccessory
    {
        public IMdCabinetAccessory Dec { get; private set; }
        MDepth _shelfInset;

        public MdShelfAccessory(MDepth shelfInset, IMdCabinetAccessory dec)
        {
            _shelfInset = shelfInset;
            Dec = dec;
        }

        public virtual List<MdDimension> PlaceAccessory(List<MdDimension> dims, IMdBaseCabinet baseStructure)
        {
            if(Dec != null){
                dims = Dec.PlaceAccessory(dims, baseStructure);
            }

            dims = new MdHorizontalWall(baseStructure).PlaceDimension(dims);
            return dims;
        }
    }
}
