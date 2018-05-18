using System;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CLGenerator.MD
{
    public class MdShapeWriter : MdRectangleWriter
    {
        public MdShapeWriter()
        {
        }

        public double[] Proportions(MdPoint point, MdDimension dim, int zoom = 1)
        {
            return new double[]
            {
                point.X * zoom,
                point.Y * zoom,
                dim.Height * zoom,
                dim.Width * zoom
            };
        }
    }

    public class MdRectangleWriter
    {
        public PdfContentByte DrawRectangle(PdfContentByte cb, double[] proportions){
            cb.Rectangle(proportions[0], proportions[1], proportions[3], proportions[2]);
            cb.FillStroke();
            return cb;
        }
    }
}
