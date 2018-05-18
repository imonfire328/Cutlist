using System;
using System.Collections.Generic;
using CLGenerator.MD;
using CLGenerator.ST;

namespace CLGenerator.BD
{
    /// <summary>
    /// build a cutlist from list of dimensions
    /// </summary>
    public class Operator 
    {
        MdList _cutList;
        Queue<MdDimension> _dimensions;
        IAlignDecorator _alignDec;

        public Operator(IStrategy<List<MdDimension>> preOrganize, MdList cutList, IAlignDecorator alignDec)
        {
            _dimensions = new Queue<MdDimension>(preOrganize.Implement());
            _cutList = cutList;
            _alignDec = alignDec;
        }

        public MdList Build()
        {
            var c = 0; 
            while(_dimensions.Count != 0)
            {
                var currentDim = new MdDimension(++c, _dimensions.Dequeue());
                MdPiece fit = null;
                while(fit == null)
                {
                    var alignment = new Align(_alignDec);
                    fit = alignment.Implement(_cutList.GetCurrentBoard(), currentDim);
                    if (fit == null)
                        _cutList.CycleBoard();
                    else
                        _cutList.AddAlignment(alignment);
                }
            }

            return _cutList;
        }
    }
}
