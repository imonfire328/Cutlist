using System;
using System.Collections.Generic;

namespace CLGenerator.MD.MdStructure
{
    public interface IMdWall : IMdCabinet
    {
    }


    public class MdHorizontalWall : IMdWall
    {
        MdDimension _dim;
        
        public MdHorizontalWall(IMdBaseCabinet struc)
        {
            _dim = new MdDimension(
                new MdRectangle(
                    struc.InsideWidth(), 
                    struc.Depth.GetMeasure()
                ), 
                struc.Dim.Name + ":shelf", 
                struc.Dim.ColorFill, 
                struc.Dim.Material
            );
        }
        public List<MdDimension> PlaceDimension(List<MdDimension> dims)
        {
            dims = _dim.PlaceDimension(dims);
            return dims;
        }
    }


    public class MdVerticalWall : IMdWall
    {
        MdDimension _dim;
        public MdVerticalWall(IMdBaseCabinet struc)
        {
            _dim = new MdDimension(
                new MdRectangle(
                    struc.Dim.Y, 
                    struc.Depth.GetMeasure()
                ), 
                _dim.Name + ":wall", 
                _dim.ColorFill, 
                _dim.Material
            );
        }

        public List<MdDimension> PlaceDimension(List<MdDimension> dims)
        {
            dims = _dim.PlaceDimension(dims);
            return dims;
        }
    }
}
