using System;
using System.Collections.Generic;
using System.Linq;
using CLGenerator.MD;

namespace CLGenerator.ST
{
    
    public interface IPointOrderStrategy
    {
        List<MdPoint> Implement(List<MdPoint> points);
    }


    public class OrderPointsByX : IPointOrderStrategy
    {
        public List<MdPoint> Implement(List<MdPoint> points){
            var _points = points.OrderBy(r => r.X).ThenBy(r => r.Y).ToList();
            return _points;
        }
    }


    public class OrderPointsByY : IPointOrderStrategy
    {
        public List<MdPoint> Implement(List<MdPoint> points){
            var _points = points.OrderBy(r => r.Y).ThenBy(r => r.X).ToList();
            return _points;
        }
    }
}
