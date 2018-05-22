using System;
using System.Collections.Generic;
using CLGenerator.MD;

namespace CLGenerator.ST
{
    public interface IStDimOrientationStrategy : IStrategy<List<MdDimension>>
    {
        
    }

    public class StOrientToHeight : IStDimOrientationStrategy
    {
        private List<MdDimension> _dims { get; set; } = new List<MdDimension>();
        
        public StOrientToHeight(List<MdDimension> dims){
            _dims = dims;
        }
        
        public List<MdDimension> Implement()
        {
            for (int i = 0; i < _dims.Count; i++)
            {
                var dim = _dims[i];
                if (dim.Width > dim.Height){
                    _dims[i] = _rotate(dim);
                }
            }
            return _dims;
        }

        ///todo: maybe add within dimension
        private MdDimension _rotate(MdDimension dim)
        {
            return new MdDimension(new MdRectangle(dim.Height, dim.Width), dim.Name, dim.ColorFill, dim.Material);
        }
    }
}
