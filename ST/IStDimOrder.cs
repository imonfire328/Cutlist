using System;
using System.Collections.Generic;
using System.Linq;
using CLGenerator.MD;

namespace CLGenerator.ST
{
    /// <summary>
    /// Interface for setting the order of dimensions
    /// </summary>
    public interface IStDimOrder : IStrategy<List<MdDimension>>{}


    /// <summary>
    /// Base class for setting dimension order
    /// </summary>
    public class StDimOrder : IStDimOrder
    {
        IStDimOrder _decorator; 

        public StDimOrder(IStDimOrder dimOrder)
        {
            _decorator = dimOrder;
        }

        public List<MdDimension> Implement()
        {
            return _decorator.Implement();
        }
    }


    /// <summary>
    /// Order dimensions by thier area
    /// </summary>
    public class OrderAreaDec : IStDimOrder
    {
        List<MdDimension> _dimensions;
        IStDimOrder _decorator { get; set; } 


        public OrderAreaDec(List<MdDimension> dim)
        {
            _dimensions = dim;
        }

        public OrderAreaDec(IStDimOrder dec)
        {
            _decorator = dec;
        }

        public List<MdDimension> Implement()
        {
            if(_decorator != null)
                _dimensions = _decorator.Implement();

            var orderedList = new List<MdDimension>();
            orderedList = _dimensions.OrderByDescending(r => r.CalcArea()).Select(r => r).ToList<MdDimension>();
            return orderedList;
        }
    }


    /// <summary>
    /// Order dimensions by their width
    /// </summary>
    public class DcOrderWidth : IStDimOrder
    {
        List<MdDimension> _dimensions;
        IStDimOrder _decorator;


        public DcOrderWidth(List<MdDimension> dim)
        {
            _dimensions = dim;
        }

        public DcOrderWidth(IStDimOrder dec)
        {
            _decorator = dec;
        }

        public List<MdDimension> Implement()
        {
            if(_decorator != null)
                _dimensions = _decorator.Implement();

            var orderedList = new List<MdDimension>();
            orderedList = _dimensions.OrderByDescending(r => r.X).Select(r => r).ToList<MdDimension>();
            return orderedList;
        }
    }

    public class DcOrderHeight : IStDimOrder
    {
        List<MdDimension> _dimensions;
        IStDimOrder _decorator;


        public DcOrderHeight(List<MdDimension> dim)
        {
            _dimensions = dim;
        }

        public DcOrderHeight(IStDimOrder dec)
        {
            _decorator = dec;
        }

        public List<MdDimension> Implement()
        {
            if (_decorator != null)
                _dimensions = _decorator.Implement();

            var orderedList = new List<MdDimension>();
            orderedList = _dimensions.OrderByDescending(r => r.Y).Select(r => r).ToList<MdDimension>();
            return orderedList;
        }

    }


    /// <summary>
    /// Order dimesions by descening binary amount
    /// </summary>
    public class DcOrderDimension : IStDimOrder
    {
        List<MdDimension> _dimensions;
        IStDimOrder _decorator;

        public DcOrderDimension(List<MdDimension> dim)
        {
            _dimensions = dim;
        }

        public DcOrderDimension(IStDimOrder dec)
        {
            _decorator = dec;
        }

        public List<MdDimension> Implement()
        {  
            if (_decorator != null)
                _dimensions = _decorator.Implement();

            return _dimensions;
        }
    }


    public abstract class OrderWithinBounds : IStDimOrder{

        public double Bound { get; private set; }
        public int Reach { get; private set;  }
        public List<MdDimension> Dimensions { get; private set; }

        public OrderWithinBounds(List<MdDimension> dims, double bound, int maxReach = 100000){
            Reach = maxReach;
            Bound = bound;
            Dimensions = dims;
        }

        public abstract List<MdDimension> Implement();
    }


    /// <summary>
    /// Order dimensions in groups based on the boards width
    /// </summary>
    public class DcOrderWithinBoundsX : OrderWithinBounds
    {
        public DcOrderWithinBoundsX(List<MdDimension> dim, MdRectangle boardDim, int maxReach = 100000) : base(
            dim, 
            boardDim.X,
            maxReach
        ){ }

        public DcOrderWithinBoundsX(IStDimOrder dec, MdRectangle boardDim, int maxReach = 100000): base(
            dec.Implement(), 
            boardDim.X, 
            maxReach
        ){ } 

        public override List<MdDimension> Implement()
        {
            var newDims = new List<MdDimension>();
            for (int i = 0; Dimensions.Count() != 0; i++)
            {
                var m = Dimensions[0];
                var totalWidth = m.X;
                newDims.Add(m);
                Dimensions.Remove(m);

                var matchingWidths = Dimensions.Where(r => r.Y == m.Y).Take(Reach).Select(r => r).ToList();
                foreach (MdDimension dim in matchingWidths)
                {
                    if (totalWidth + dim.X < Bound)
                    {
                        newDims.Add(dim);
                        Dimensions.Remove(dim);
                        totalWidth += dim.X;
                    }
                }
            }
            return newDims;
        }
    }
 

    /// <summary>
    /// Order dimensions in groups based on the boards height
    /// </summary>
    public class DcOrderWithinBoundsY : OrderWithinBounds
    {
        public DcOrderWithinBoundsY(List<MdDimension> dim, MdRectangle boardDim, int maxReach = 100000) : base(
            dim, 
            boardDim.Y,
            maxReach
        ){}

        public DcOrderWithinBoundsY(IStDimOrder dec, MdRectangle boardDim, int maxReach = 100000): base(
            dec.Implement(), 
            boardDim.Y, 
            maxReach
        ){}

        public override List<MdDimension> Implement()
        {
            var newDims = new List<MdDimension>();
            for (int i = 0; Dimensions.Count() != 0; i++){
                
                var m = Dimensions[0];
                var totalHeight = m.Y;
                newDims.Add(m);
                Dimensions.Remove(m);

                var matchingWidths = Dimensions.Where(r => r.X == m.X).Take(Reach).Select(r => r).ToList();
                foreach(MdDimension dim in matchingWidths){
                    if(totalHeight + dim.Y < Bound){
                        newDims.Add(dim);
                        Dimensions.Remove(dim);
                        totalHeight += dim.Y;
                    } 
                }
            }
            return newDims;
        }
    }
}
