using System;
using System.Collections.Generic;
using System.Linq;
using CLGenerator.MD;

namespace CLGenerator.ST
{
    public interface IStBoardOrder 
    {
        List<MdBoard> Implement(List<MdBoard> boards);
    }

    public class ThickDescending : IStBoardOrder
    {
        public List<MdBoard> Implement(List<MdBoard> boards){
            boards = boards.OrderByDescending(r => r.Material.Thickness).Select(r => r).ToList();
            return boards;
        }
    }

    public class ThickAscending : IStBoardOrder{

        public List<MdBoard> Implement(List<MdBoard> boards){
            boards = boards.OrderBy(r => r.Material.Thickness.GetMeasure()).Select(r => r).ToList();
            return boards;
        }
    }
}
