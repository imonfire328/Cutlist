using System;
using CLGenerator.MD;

namespace CLGenerator.ST
{
    public interface IDcDimAllowence 
    {
        MdPiece TryAllowance(IAlignDecorator alignDec, MdBoard board, MdDimension dim);
    }

    public class AllowRotationForFit : IDcDimAllowence
    {
        IDcDimAllowence _dcAllowence; 

        public AllowRotationForFit(IDcDimAllowence dcAllowence){
            _dcAllowence = dcAllowence;
        }

        public AllowRotationForFit(){}

        public MdPiece TryAllowance(IAlignDecorator alignDec, MdBoard board, MdDimension dim)
        {
            MdPiece alignedPiece = null;

            if(_dcAllowence != null){
                alignedPiece = _dcAllowence.TryAllowance(alignDec, board, dim);
            }

            if(alignedPiece == null){
                var rotatedDim = new MdDimension(new MdRectangle(dim.Y, dim.X), dim);
                var result = alignDec.Implement(board, rotatedDim);
                if(result != null){
                    
                }
                return result;
            }
            else{
                return alignedPiece;
            }
        }
    }
}
