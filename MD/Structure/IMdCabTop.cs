using System;
using System.Collections.Generic;
using CLGenerator.MD.MdMeasure;
using CLGenerator.MD.MdStructure;

namespace CLGenerator.MD.Structure
{
    /// <summary>
    /// represents a case with 2 horizontal support rails and multiple shelves
    /// </summary>
    /// 
    public interface IMdCabTop : IMdBaseCabinet {}

    public class MdExposedTopCase : MdCase, IMdCabinet, IMdCabTop
    {
        public MdExposedTopCase(MdCase structure) : base(structure){}

        public override List<MdDimension> PlaceDimension(List<MdDimension> dims)
        {
            dims = base.PlaceDimension(dims);

            // top rails
            dims.Add(new MdDimension(new MdRectangle(InsideWidth(), 6), base.Dim.Name + ":top rail", Dim.ColorFill, Dim.Material));
            dims.Add(new MdDimension(new MdRectangle(InsideWidth(), 6), Dim.Name + ":top rail", Dim.ColorFill, Dim.Material));

            return dims;
        }
    }

    public class MdClosedTopCase : MdCase, IMdCabinet, IMdCabTop
    {
        IMdBaseCabinet _baseStructure;
        public MdClosedTopCase(MdCase structure) : base(structure)
        {
            _baseStructure = structure;
        }

        public override List<MdDimension> PlaceDimension(List<MdDimension> dims)
        {
            dims = base.PlaceDimension(dims);

            dims = new MdHorizontalWall(_baseStructure).PlaceDimension(dims);
            return dims;
        }

    }
}
