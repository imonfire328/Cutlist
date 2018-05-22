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


    /// <summary>
    /// Represents a dimension that fits at a certain point on a board
    /// </summary>
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

    /// <summary>
    /// Find a point on a board that fits a dimension
    /// </summary>
    public abstract class AlignDecorator : IAlignDecorator
    {
        public AlignDecorator StAlignDec { get; private set; }
        public abstract MdPiece Implement(MdBoard board, MdDimension dim);

        public AlignDecorator(AlignDecorator strategy)
        {
            StAlignDec = strategy;
        }

        public AlignDecorator() { }

        /// <summary>
        /// Try each point within a boards open edges to place dimension
        /// </summary>
        /// <returns>The align.</returns>
        /// <param name="edges">Edges.</param>
        /// <param name="dim">Dim.</param>
        /// <param name="board">Board.</param>
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


    /// <summary>
    /// Aligns pieces with a vertical priority
    /// </summary>
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
    /// Aligns a piece with a horizontal priority
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
