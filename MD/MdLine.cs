using System;
using System.Collections.Generic;
using System.Diagnostics;
using CLGenerator.MD.MdMeasure;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CLGenerator.MD
{
    /// <summary>
    /// Represents a line between two parallel pieces
    /// </summary>
    public interface IMdLine
    {
        MdPoint Start { get; } 
        MdPoint End { get; }
        bool Equals(MdLine line);
        string id { get;} 
        PdfContentByte Write(PdfContentByte cb, int zoom);
    }

    public interface IMdLineDirection : IMdLine
    {
        void AddComposite(IMdLine line);
        bool IsAffectedByKerf(IMdLine line);
    }

    /// <summary>
    /// A line that accounts for added kerf of additional cuts 
    /// </summary>
    public class CutLine : IMdLine
    {
        public MdPoint Start { get; private set; }
        public MdPoint End { get; private set; }
        public string id { get; private set; }
        IMdLine _lineDirection; 

        public CutLine(IMdLineDirection lineDirection, List<IMdLineDirection> matchingLineTypes, MWidth kerf){
            _lineDirection = lineDirection;
            int ammnt = 0;

            foreach(IMdLineDirection line in matchingLineTypes){
                if (line.Equals(_lineDirection))
                    continue;
                if (lineDirection.IsAffectedByKerf(line))
                    ammnt++;
            }
        }




        public bool Equals(MdLine line)
        {
            throw new NotImplementedException();
        }

        public PdfContentByte Write(PdfContentByte cb, int zoom)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// Horizontal line
    /// </summary>
    public class MdHorLine : MdLine, IMdLineDirection{
        
        public MdHorLine(MdPoint start, MdPoint end) : base(start, end) { }

        public bool Intersects(MdVertLine vline){

            // if a horizontal line is intersected by a vertical line, 
            // the y value for the horizontal line will be smaller than that
            // of the y value for the end point of the vertical line, and both x 
            // values for the end points will match

            if ((vline.End.X == this.End.X && vline.End.Y > this.End.Y) || this.Start.X != 0)
                return true;
            return false;
        }

        public bool IsAffectedByKerf(IMdLine line)
        {
            if (line.GetType() != this.GetType()) return false;
            if (line.Start.Y < Start.Y && line.Start.X < End.X && line.Start.X > Start.X) return true;
            else
                return false;
        }

        public PdfContentByte Write(PdfContentByte cb, int zoom){

            var color = new BaseColor(200, 140, 34);
            cb.SetColorStroke(color);
            cb.MoveTo(Start.X * zoom, Start.Y * zoom);
            cb.LineTo(End.X * zoom, End.Y * zoom);
            cb.Stroke();

            //get center coordinates within rectangle
            float textX = (float)End.X * zoom + 40;
            //account for height of text by subtracting
            float textY = (float)End.Y * zoom;

            // draw text
            cb.BeginText();
            cb.SetColorFill(color);
            cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, false), 7);
            cb.ShowTextAligned(Element.ALIGN_CENTER, "Y: " + End.Y, textX, textY, 0f);
            cb.EndText();
            return cb;
        }
    }


    /// <summary>
    /// Vertical line
    /// </summary>
    public class MdVertLine : MdLine, IMdLineDirection
    {
        public MdVertLine(MdPoint start, MdPoint end): base(start, end){}
        public bool Intersects(MdHorLine hLine)
        {
            // if a vertical line is intersected by a horizontal line, 
            // the x value for the end point of the vert line will be shorter
            // than the x value of the horizontal line, and both y values for the 
            // end points will match

            if ((this.End.Y == hLine.End.Y && this.End.X < hLine.End.X)|| this.Start.Y != 0)
                return true;
            return false;
        }

        public bool IsAffectedByKerf(IMdLine line)
        {
            if (line.GetType() != this.GetType()) return false;
            if (line.Start.X > Start.X && line.Start.Y < End.Y && line.Start.Y > Start.Y) return true;
            else
                return false;
        }

        public PdfContentByte Write(PdfContentByte cb, int zoom)
        {
            var color = new BaseColor(85, 200, 100);
            cb.SetColorStroke(color);
            cb.MoveTo(Start.X * zoom, Start.Y * zoom);
            cb.LineTo(End.X * zoom, End.Y * zoom);
            cb.Stroke();

            //get center coordinates within rectangle
            float textX = (float)End.X * zoom; 
            //account for height of text by subtracting
            float textY = (float)End.Y * zoom + 40;

            // draw text
            cb.BeginText();
            cb.SetColorFill(color);
            cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, false), 7);
            cb.ShowTextAligned(Element.ALIGN_CENTER, "X: " + End.X, textX, textY, 0f);
            cb.EndText();
            return cb;
        }
    }


    /// <summary>
    /// Base class for unspecified line
    /// </summary>
    public class MdLine 
    {
        public MdPoint Start { get; private set; }
        public MdPoint End { get; private set; }
        public MdRectangleWriter PdfWriter { get; private set; }
        public string id { get; set;  } 

        public MdLine(MdPoint start, MdPoint end)
        {
            id = Constants.counter().ToString();
            PdfWriter = new MdRectangleWriter();
            Start = start;
            End = end;
            Debug.WriteLine("Line Created: " + id + " (" + Start.X + " x " + Start.Y + "), to (" + End.X + " x " + End.Y + ")");
        }

        /// <summary>
        /// Extend a line to new point
        /// </summary>
        /// <param name="line">Line.</param>
        public void AddComposite(IMdLine line){
            if (!line.Start.Equals(End))
                throw new Exception("the lines cannot be combined because they do not match");
            End = line.End;
            Debug.WriteLine("Merge: " + line.id + " into " + id + " (" + Start.X + " x " + Start.Y + "), to (" + End.X + " x " + End.Y + ")");
        }


        public bool Equals(MdLine line){
            if (Start.Equals(line.Start) && End.Equals(line.End))
                return true;
            return false; 
        }
    }
}
