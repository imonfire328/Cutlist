using System;
using System.Collections.Generic;
using System.Linq;
using CLGenerator.MD;

namespace CLGenerator.ST
{

    public interface IAlignDecorator
    {
        MdPiece Implement(MdBoard board, MdDimension dim);
    }


    public class Align
    {
        public MdPiece Alignment { get; private set; }
        private IAlignDecorator _alignStgy;


        public Align(IAlignDecorator alignStgy)
        {
            _alignStgy = alignStgy;           
        }

        // return an aligned peice
        public MdPiece Implement(MdBoard board, MdDimension dim)
        {
            Alignment = _alignStgy.Implement(board, dim);
            return Alignment;
        }
    }


    public abstract class AlignDecorator : IAlignDecorator
    {
        public AlignDecorator StAlignDec { get; private set; }
        public abstract MdPiece Implement(MdBoard board, MdDimension dim);

        public AlignDecorator(AlignDecorator strategy)
        {
            StAlignDec = strategy;
        }

        public AlignDecorator() { }

        public MdPiece AttemptAlign(List<MdPoint> edges, MdDimension dim, MdBoard board)
        {
            foreach (MdPoint edge in edges)
            {
                if (board.TryAlignment(dim, edge))
                    return new MdPiece(edge, dim);
            }
            return null;
        }
    }


    public class StAlignVertical : AlignDecorator
    {
        public StAlignVertical(AlignDecorator align) : base (align){ }
        public StAlignVertical() { }

        public override MdPiece Implement(MdBoard board, MdDimension dim)
        {
            MdPiece alignment = null;

            // try decorator if available
            if (base.StAlignDec != null)
                alignment = base.StAlignDec.Implement(board, dim);
            
            if (alignment == null){
                // smaller x valued points take priority over other available points.
                // this will cause the order of alignment along the left side, working right.
                var pointsByVert = board.AvailablePoints.OrderBy(r => r.X).Select(r => r).ToList<MdPoint>();
                alignment = base.AttemptAlign(pointsByVert, dim, board);
            }
            return alignment;
        }
    }


    /// <summary>
    /// Strategy to prioritize aligning pieces horizontally.
    /// </summary>
    public class StAlignHorizontal : AlignDecorator
    {
        public StAlignHorizontal(AlignDecorator align) : base (align){}
        public StAlignHorizontal(){}

        public override MdPiece Implement(MdBoard board, MdDimension dim)
        {
            MdPiece alignment = null;
            // try decorator if available
            if(base.StAlignDec != null)
                alignment = base.StAlignDec.Implement(board, dim);

            if(alignment == null){
                // smaller y valued points take priority over other available points.
                // this will cause the order of alignment along the bottom, working up.
                var pointsByVert = board.AvailablePoints.OrderBy(r => r.Y).Select(r => r).ToList<MdPoint>();
                alignment = base.AttemptAlign(pointsByVert, dim, board);
            }
            return alignment;
        }
    }
}
