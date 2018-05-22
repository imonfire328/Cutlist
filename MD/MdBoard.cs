using System;
using System.Collections.Generic;
using System.Linq;
using CLGenerator.BD;
using CLGenerator.MD.MdMaterial;
using CLGenerator.ST;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CLGenerator.MD
{
    public class MdBoard 
    {
        public int _id { get; private set; }
        public List<MdPoint> AvailablePoints { get; private set; } = new List<MdPoint>() { new MdPoint(0, 0) };

        List<MdPiece> _pieces { get; set; } = new List<MdPiece>();
        MdMatrix _matrix { get; set; } = new MdMatrix();
        MdDimension _dim;
        IMaterial _material;
        IPointOrderStrategy _edgeOrderStrgy;
        IDcDimRestriction _pieceRestrictions;
        double _availableArea;


        public MdBoard(MdDimension dim, IPointOrderStrategy edgeOrderStrgy, IDcDimRestriction pieceRestrictions)
        {
            _dim = dim;
            _id = Convert.ToInt32(DateTime.Now.ToString("mmssffff"));
            _availableArea = _dim.CalcArea();
            _edgeOrderStrgy = edgeOrderStrgy;
            _pieceRestrictions = pieceRestrictions;
            _material = _dim.Material;
        }


        public MdBoard(MdDimension dim, IPointOrderStrategy edgeOrderStrgy, IDcDimRestriction pieceRestrictions, IMaterial material)
        {
            _pieces = new List<MdPiece>();
            _dim = dim;
            _id = Convert.ToInt32(DateTime.Now.ToString("mmssffff"));
            _availableArea = _dim.CalcArea();
            _edgeOrderStrgy = edgeOrderStrgy;
            _pieceRestrictions = pieceRestrictions;
            _material = material;
        }


        /// <summary>
        /// Add a new piece to the board
        /// </summary>
        /// <param name="strgy">Strgy.</param>
        public void AddAlignment(Align confirmedAlignment)
        {
            _pieces.Add(confirmedAlignment.Alignment);
            _availableArea = _calcAvailableArea();
            _matrix.UpdateMatrix(confirmedAlignment);
            _calcAvailablePoints(confirmedAlignment.Alignment);
        }


        /// <summary>
        /// determine 
        /// </summary>
        /// <returns><c>true</c>, if alignment was hased, <c>false</c> otherwise.</returns>
        /// <param name="dim">Dim.</param>
        public bool TryAlignment(MdDimension dim, MdPoint point)
        {
            if (dim.Material.Thickness != _material.Thickness) return false;
            if (_pieces.Count() == 0) return true; 
            if (!WithinBounds(dim, point)) return false;
            if (_pieceRestrictions.Restricted(this, point, dim))return false;
                
            foreach(MdPiece piece in _pieces){
                if (piece.Collides(dim, point))
                    return false;
            }
            return true; 
        }


        /// <summary>
        /// Writes the peices to the pdfContentyByte stream
        /// </summary>
        /// <returns>The pieces.</returns>
        /// <param name="cb">Cb.</param>
        public PdfContentByte WritePieces(PdfContentByte cb)
        {
            // draw text
            cb.BeginText();
            cb.SetColorFill(_dim.ColorFill[0]);
            cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, false), 12);
            cb.ShowTextAligned(Element.ALIGN_CENTER, "Thickness: " + _material.Thickness.GetMeasure().ToString(), cb.PdfDocument.Right - 100, 20, 0f);
            cb.EndText();

            cb.SetColorStroke(new BaseColor(0, 0, 0));
            cb.SetColorFill(new BaseColor(255, 255, 255));

            foreach(MdPiece p in _pieces){
                cb = p.Write(cb);
            }
            return cb;
        }


        /// <summary>
        /// Writes the cutlines to the pdfContentByte stream.
        /// </summary>
        /// <returns>The cutlines.</returns>
        /// <param name="cb">Cb.</param>
        public PdfContentByte WriteCutlines(PdfContentByte cb){
            var cutLines = _matrix.GetCutLines(this);
                foreach(IMdLine line in cutLines){
                    line.Write(cb, Constants.PdfZoom);
                }
            return cb;
        }


        /// <summary>
        /// determine whether the board has enough area to fit a piece
        /// </summary>
        /// <returns><c>true</c>, if fit was caned, <c>false</c> otherwise.</returns>
        /// <param name="dim">Dim.</param>
        public bool WithinBounds(MdDimension dim, MdPoint startPoint)
        {
            if (dim.CalcArea() > _availableArea)
                return false;
            if (startPoint.X + dim.Width > _dim.Width)
                return false;
            if (startPoint.Y + dim.Height > _dim.Height)
                return false;

            return true; 
        }


        public MdPoint SetToHeight(MdPoint p){
            return new MdPoint(p.X, _dim.Height);
        }

        public MdPoint SetToWidth(MdPoint p){
            return new MdPoint(_dim.Width, p.Y);
        }


        double _calcAvailableArea()
        {
            double area = _dim.CalcArea(); 
            foreach(MdPiece p in _pieces){
                area -= p.Area;
            }
            return area;
        }


        // maybe create a new interface for this
        void _calcAvailablePoints(MdPiece alignment)
        {
            foreach(MdPoint p in alignment.Coordinates.Corners){
                var edge = AvailablePoints.FirstOrDefault(r => r.X == p.X && r.Y == p.Y);
                if (edge != null){
                    AvailablePoints.Remove(edge);
                }
                else{
                    AvailablePoints.Add(p);
                }
            }

            AvailablePoints = _edgeOrderStrgy.Implement(AvailablePoints);
        }
    }
}
