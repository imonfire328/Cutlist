using System;
using System.Collections.Generic;

namespace CLGenerator.MD.MdStructure
{
    public interface IWall : IStructure
    {
    }


    public class HorizontalWall : IWall
    {
        MdDimension _dim;
        
        public HorizontalWall(IBaseStructure struc)
        {
            _dim = new MdDimension(
                new MdRectangle(
                    struc.InsideWidth(), 
                    struc.Depth.GetMeasure()
                ), 
                struc.Dim.Name + ":wall", 
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


    public class VerticalWall : IWall
    {
        MdDimension _dim;
        public VerticalWall(IBaseStructure struc)
        {
            _dim = new MdDimension(
                new MdRectangle(
                    struc.Dim.Height, 
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
