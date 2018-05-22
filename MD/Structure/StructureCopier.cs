using System;
using System.Collections.Generic;
using CLGenerator.MD;
using CLGenerator.MD.MdMeasure;
using CLGenerator.MD.MdStructure;

namespace CLGenerator.BD
{
    public class StructureCopier : IDimension
    {
        IStructure _structure;
        MMultiple _copies;
        List<MdDimension> _dims;


        public StructureCopier(MMultiple copies, IStructure structure){
            _structure = structure;
            _copies = copies;
        }

        public List<MdDimension> PlaceDimension(List<MdDimension> dims)
        {
            for (int i = 0; i < _copies.GetQuantity(); i++){
                dims = _structure.PlaceDimension(dims);
            }
            return dims;
        }
    }
}
