using System;
using System.Collections.Generic;
using System.Linq;
using CLGenerator.MD;

namespace CLGenerator.ST
{
    public interface IDcDimRestriction
    {
        bool Restricted(MdBoard board, MdPoint _selectedEdge, MdDimension dim);
    }



    /// <summary>
    /// Restrict pieces that are larger than board with kerf
    /// </summary>
    public class DcKerfRestriction : IDcDimRestriction
    {
        IDcDimRestriction _dec;
        double _kerfAllowance;

        public DcKerfRestriction(IDcDimRestriction dec, double kerfAllowance)
        {
            _dec = dec;
            _kerfAllowance = kerfAllowance;
        }

        public DcKerfRestriction(double kerfAllowance)
        {
            _kerfAllowance = kerfAllowance;
        }

        public bool Restricted(MdBoard board, MdPoint _selectedEdge, MdDimension dim)
        {
            bool restricted = false;

            if (_dec != null)
                restricted = _dec.Restricted(board, _selectedEdge, dim);

            if (!restricted)
            {
                var tempDim = new MdDimension(new MdRectangle(dim.X + _kerfAllowance, dim.Y + _kerfAllowance), dim);
                if (board.WithinBounds(tempDim, _selectedEdge))
                    return false;
            }
            return true;
        }
    }


    /// <summary>
    /// Restrict pieces that protrude horizontally to peices below
    /// </summary>
    public class DcDimOverhangXRestriction : IDcDimRestriction
    {
        IDcDimRestriction _dec;

        public DcDimOverhangXRestriction(IDcDimRestriction dec){
            _dec = dec;
        }

        public DcDimOverhangXRestriction(){}

        public bool Restricted(MdBoard board, MdPoint _selectedEdge, MdDimension dim){

            bool restricted = false; 

            if (_dec != null)
                restricted = _dec.Restricted(board, _selectedEdge, dim);

            if(!restricted){
                var p = new MdPiece(_selectedEdge, dim);
                var pieceEndPoint = p.Coordinates.Corners[3];

                // piece is on the bottom of board
                if (pieceEndPoint.Y == 0) return false;

                // if the right bottom corner x value  of the dim in question is larger than the furthest x edge, 
                // the peice is hanging over.

                var point = board.AvailablePoints.Where(r => r.Y == pieceEndPoint.Y).OrderByDescending(r => r.X).Select(r => r).ToList();
                if (point.First() == null) return false; 
                if (point.First().X >= pieceEndPoint.X ) return false;
            }
            return true;
        }
    }


    public class DcDimOverhangYRestriction : IDcDimRestriction
    {
        IDcDimRestriction _dec;

        public DcDimOverhangYRestriction(IDcDimRestriction dec)
        {
            _dec = dec;
        }

        public DcDimOverhangYRestriction() { }

        public bool Restricted(MdBoard board, MdPoint _selectedEdge, MdDimension dim)
        {

            bool restricted = false;

            if (_dec != null)
                restricted = _dec.Restricted(board, _selectedEdge, dim);

            if (!restricted)
            {
                var p = new MdPiece(_selectedEdge, dim);

                //top left point
                var pieceEndPoint = p.Coordinates.Corners[1];

                // piece is on the bottom of board
                if (pieceEndPoint.X == 0) return false;

                // if the right bottom corner x value  of the dim in question is larger than the furthest x edge, 
                // the peice is hanging over.

                var point = board.AvailablePoints.Where(r => r.X == pieceEndPoint.X).OrderByDescending(r => r.Y).Select(r => r).ToList();
                if (point.First() == null) return false;
                if (point.First().Y >= pieceEndPoint.Y) return false;
            }
            return true;
        }
    }
}
