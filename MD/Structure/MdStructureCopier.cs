using System;
using System.Collections.Generic;
using CLGenerator.MD;
using CLGenerator.MD.MdMeasure;
using CLGenerator.MD.MdStructure;
using CLGenerator.MD.Structure;

namespace CLGenerator.BD
{
    public interface IMdCopier{}

    public class MdAccessoryCopier : IMdCabinetAccessory{

        public IMdCabinetAccessory Dec { get; private set;  }
        MMultiple _copies;
        IMdCabinetAccessory _accessory; 

        public MdAccessoryCopier(MMultiple copies, IMdCabinetAccessory accessory){
            _copies = copies;
            _accessory = accessory;
        }

        public List<MdDimension> PlaceAccessory(List<MdDimension> dims, IMdBaseCabinet baseStructure)
        {
            for (int i = 0; i < _copies.GetQuantity(); i++){
               dims =  _accessory.PlaceAccessory(dims, baseStructure);
            }

            return dims;
        }
    }

    public class MdStructureCopier : IMdDimension
    {
        IMdDimension _dimension;
        MMultiple _copies;
        List<MdDimension> _dims;

        public MdStructureCopier(MMultiple copies, IMdDimension dimension){
            _dimension = dimension;
            _copies = copies;
        }

        public List<MdDimension> PlaceDimension(List<MdDimension> dims)
        {
            for (int i = 0; i < _copies.GetQuantity(); i++){
                dims = _dimension.PlaceDimension(dims);
            }
            return dims;
        }
    }
}
