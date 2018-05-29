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
    public class MdBoard : MdDimension
    {
        public int _id { get; private set; }
        public List<MdPoint> AvailablePoints { get; private set; } = new List<MdPoint>() { new MdPoint(0, 0) };

        List<MdPiece> _pieces { get; set; } = new List<MdPiece>();
        MdMatrix _matrix { get; set; } = new MdMatrix();
        IPointOrderStrategy _edgeOrderStrgy;
        IDcDimRestriction _pieceRestrictions;
        double _availableArea;

        public MdBoard(IPointOrderStrategy edgeOrderStrgy, IDcDimRestriction pieceRestrictions, Material material) : base(material, new MdDimension(material))
        {
            _pieces = new List<MdPiece>();
            _id = Convert.ToInt32(DateTime.Now.ToString("mmssffff"));
            _availableArea = CalcArea();
            _edgeOrderStrgy = edgeOrderStrgy;
            _pieceRestrictions = pieceRestrictions;
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
            if (dim.Material.Thickness != Material.Thickness) return false;
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
            cb.SetColorStroke(new BaseColor(0, 0, 0));
            cb.SetColorFill(new BaseColor(255, 255, 255));

            foreach(MdPiece p in _pieces){
                cb = p.Write(cb);
            }
            return cb;
        }


        public PdfContentByte WriteBoard(PdfContentByte cb)
        {
            new MdPiece(new MdPoint(0, 0), GetBase()).Write(cb);
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
            if (startPoint.X + dim.X > X)
                return false;
            if (startPoint.Y + dim.Y > Y)
                return false;

            return true; 
        }


        public MdPoint SetToHeight(MdPoint p){
            return new MdPoint(p.X, Y);
        }

        public MdPoint SetToWidth(MdPoint p){
            return new MdPoint(X, p.Y);
        }


        double _calcAvailableArea()
        {
            double area = CalcArea(); 
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
