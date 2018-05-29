using System;
using System.Collections.Generic;
using CLGenerator.MD;

namespace CLGenerator.ST
{
    public interface IStDimOrientationStrategy : IStrategy<List<MdDimension>>
    {
    }


    public abstract class StDimOrientation 
    {
        public List<MdDimension> Dims { get; private set; }

        public StDimOrientation(List<MdDimension> dims){
            Dims = dims;
        }

        public MdDimension Rotate(MdDimension dim){
            // if dim exceeds material size return original dim
            if (dim.X > dim.Material.Y || dim.Y > dim.Material.X){
                return dim;
            } 
            var newDim =  new MdDimension(new MdRectangle(dim.Y, dim.X), dim.Name, dim.ColorFill, dim.Material);
            return newDim;
        }
    }
   

    public class StOrientToHeight : StDimOrientation
    {
        public StOrientToHeight(List<MdDimension> dims) : base(dims)
        {
        }
        
        public List<MdDimension> Implement()
        {
            for (int i = 0; i < Dims.Count; i++){
                var dim = Dims[i];
                if(dim.Name == "book:R wall"){
                    
                }

                if (dim.X > dim.Y){
                    Dims[i] = Rotate(dim);
                }
            }
            return Dims;
        }
    }

    public class StOrientToWidth : StDimOrientation
    {
        public StOrientToWidth(List<MdDimension> dims): base(dims)
        {
        }

        public List<MdDimension> Implement()
        {
            for (int i = 0; i < Dims.Count; i++){
                var dim = Dims[i];
                if (dim.Y > dim.X){
                    Rotate(dim);
                }
            }
            return Dims;
        }
    }
}
