using System;
using System.Collections.Generic;

namespace CLGenerator.MD
{
    public class MdDimList : List<IMdDimension>
    {
        private List<IMdDimension> _dims { get; set; } 
        
        public MdDimList(List<IMdDimension> dims)
        {
            _dims = dims;
        }

        public List<MdDimension> GetDimensions()
        {
            var newList = new List<MdDimension>();

            foreach(IMdDimension dim in _dims){
                newList = dim.PlaceDimension(newList);
            }
            return newList;
        }
    }
}
