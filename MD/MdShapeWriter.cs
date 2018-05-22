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

        /// <summary>
        /// Returns a list of dimensions with zoom applied
        /// </summary>
        /// <returns>The proportions.</returns>
        /// <param name="point">Point.</param>
        /// <param name="dim">Dim.</param>
        /// <param name="zoom">Zoom.</param>
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

    /// <summary>
    /// Draws a rectangle to the given PdfContentByte
    /// </summary>
    public class MdRectangleWriter
    {
        public PdfContentByte DrawRectangle(PdfContentByte cb, double[] proportions, BaseColor color){
            cb.SetColorFill(color);
            cb.Rectangle(proportions[0], proportions[1], proportions[3], proportions[2]);
            cb.FillStroke();
            return cb;
        }
    }
}
