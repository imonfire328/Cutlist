using System;
using System.Collections.Generic;

namespace CLGenerator.MD
{
    public class MdDimList : List<IDimension>
    {
        private List<IDimension> _dims { get; set; } 
        
        public MdDimList(List<IDimension> dims)
        {
            _dims = dims;
        }

        public List<MdDimension> GetDimensions()
        {
            var newList = new List<MdDimension>();

            foreach(IDimension dim in _dims){
                newList = dim.PlaceDimension(newList);
            }
            return newList;
        }
    }
}
