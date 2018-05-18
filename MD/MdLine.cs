using System;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CLGenerator.MD
{

    public interface IMdLine
    {
        MdPoint Start { get; } 
        MdPoint End { get; } 
        void AddComposite(IMdLine line);
        bool Equals(MdLine line);
        string id { get;} 
        PdfContentByte Write(PdfContentByte cb, int zoom);
    }


    public class MdHorLine : MdLine, IMdLine{
        
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

        public PdfContentByte Write(PdfContentByte cb, int zoom){
            cb.SetColorStroke(new BaseColor(200, 140, 34));
            cb.MoveTo(Start.X * zoom, Start.Y * zoom);
            cb.LineTo(End.X * zoom, End.Y * zoom);
            cb.Stroke();
            return cb;
        }
    }


    public class MdVertLine : MdLine, IMdLine
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

        public PdfContentByte Write(PdfContentByte cb, int zoom)
        {
            cb.SetColorStroke(new BaseColor(85, 200, 100));
            cb.MoveTo(Start.X * zoom, Start.Y * zoom);
            cb.LineTo(End.X * zoom, End.Y * zoom);
            cb.Stroke();
            return cb;
        }
    }




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

        public void AddComposite(IMdLine line){
            if (!line.Start.Compare(End))
                throw new Exception("the lines cannot be combined because they do not match");
            End = line.End;
            Debug.WriteLine("Merge: " + line.id + " into " + id + " (" + Start.X + " x " + Start.Y + "), to (" + End.X + " x " + End.Y + ")");
        }


        public bool Equals(MdLine line){
            if (Start.Compare(line.Start) && End.Compare(line.End))
                return true;
            return false; 
        }
    }
}
