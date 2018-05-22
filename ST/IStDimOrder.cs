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
            orderedList = _dimensions.OrderByDescending(r => r.Width).Select(r => r).ToList<MdDimension>();
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

            _dimensions = _dimensions.OrderByDescending(r => r.ToBinary()).Select(r => r).ToList<MdDimension>();
            return _dimensions;
        }
    }

    /// <summary>
    /// Order dimensions in groups based on the boards height
    /// </summary>
    public class DcOrderWithinBoundsY : IStDimOrder
    {
        double _yBound;
        int _reach;
        List<MdDimension> _dimensions;
        IStDimOrder _decorator; 

        public DcOrderWithinBoundsY(List<MdDimension> dim, MdDimension boardDim, int maxReach = 100000){
            _yBound = boardDim.Height;
            _dimensions = dim;
            _reach = maxReach;
        }

        public DcOrderWithinBoundsY(IStDimOrder dec, MdDimension boardDim, int maxReach = 100000){
            _decorator = dec;
            _yBound = boardDim.Height;
            _reach = maxReach;
        }

        public List<MdDimension> Implement()
        {
            if(_decorator != null){
                _dimensions = _decorator.Implement();
            }

            var newDims = new List<MdDimension>();
            for (int i = 0; _dimensions.Count() != 0; i++){
                
                var m = _dimensions[0];
                var totalHeight = m.Height;
                newDims.Add(m);
                _dimensions.Remove(m);

                var matchingWidths = _dimensions.Where(r => r.Width == m.Width).Take(_reach).Select(r => r).ToList();
                foreach(MdDimension dim in matchingWidths){
                    if(totalHeight + dim.Height < _yBound){
                        newDims.Add(dim);
                        _dimensions.Remove(dim);
                        totalHeight += dim.Height;
                    } 
                }
            }
            return newDims;
        }
    }
}
