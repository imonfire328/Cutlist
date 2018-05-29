using System;
using System.Collections.Generic;
using CLGenerator.BD;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CLGenerator.MD
{
    /// <summary>
    /// Represents an area of space occupied on a board
    /// </summary>
    public class MdPiece 
    {
        public int Id { get; private set; }
        public MdCoordinates Coordinates { get; private set; }
        public double Area { get; private set; }

        public MdPoint _start;
        private MdDimension _dim;
        private MdShapeWriter _pdfShapeWriter { get; set; } = new MdShapeWriter(); 

        public MdPiece(MdPoint start, MdDimension dim)
        {
            _start = start;
            _dim = dim;
            Coordinates = new MdCoordinates(_start, _dim);
            Area = dim.CalcArea();
            Id = dim.Id;
        }


        /// <summary>
        /// return true if current placement intersects other pieces
        /// </summary>
        /// <returns>The collides.</returns>
        /// <param name="p1">P1.</param>
        /// <param name="p2">P2.</param>
        public bool Collides(MdDimension pDimension, MdPoint pStartPoint)
        {
            if (
                this._start.X < pStartPoint.X + pDimension.X &&
                this._start.X + this._dim.X > pStartPoint.X &&
                this. _start.Y < pStartPoint.Y + pDimension.Y &&
                this._dim.Y + this._start.Y > pStartPoint.Y 
            ){
                return true;
            }
            return false;
        }


        /// <summary>
        /// [left, bottom, top right]
        /// </summary>
        /// <returns>The lines.</returns>
        public List<IMdLine> ToLines(){
            var list = new List<IMdLine>(){
                new MdVertLine(_start, Coordinates.Corners[1]),
                new MdHorLine(_start, Coordinates.Corners[3]),
                new MdHorLine(Coordinates.Corners[1], Coordinates.Corners[2]),
                new MdVertLine(Coordinates.Corners[3], Coordinates.Corners[2])
            };
            return list;
        }

        /// <summary>
        /// Write the piece to pdfContentByte object
        /// </summary>
        /// <returns>The write.</returns>
        /// <param name="cb">Cb.</param>
        /// <param name="multiplier">Multiplier.</param>
        public PdfContentByte Write(PdfContentByte cb){

            string id = _dim.Id.ToString();

            // set position values for piece
            var pProporitons = _pdfShapeWriter.Proportions(_start, _dim, Constants.PdfZoom);
            cb = _pdfShapeWriter.DrawRectangle(cb, pProporitons, _dim.ColorFill[1]);

            // draw text
            cb.BeginText();
            cb.SetColorFill(_dim.ColorFill[0]);

            StaticHelpers.SetTextWithinCoordinates(this, this.ToString(), cb);

            cb.EndText();
            return cb;
        }

        public override string ToString()
        {
            return _dim.Name + "\n \n" + _dim.ToString();
        }
    }
}
