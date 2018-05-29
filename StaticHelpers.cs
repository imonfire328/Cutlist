using System;
using CLGenerator.MD;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CLGenerator
{
    public class StaticHelpers
    {
        public StaticHelpers()
        {
        }

        public static int boardInc = 1;

        public static double GetTextCenterPoint(string text, int textSize){
            double width = 0;
            if(text == null || text.Length == 0){
                return 1;
            }
            foreach (char c in text){
                width += textSize;
            }
            return width;
        }

        public static BaseColor[] ColorRange(Random rand, int limit = 75)
        {
            var r = rand.Next(limit);
            var g = rand.Next(limit);
            var b = rand.Next(limit);

            var dark = new BaseColor(r, g, b);
            var light = new BaseColor(r + 125, g + 125, b + 125);

            return new BaseColor[]{ dark, light };
        }

        public static float ApplyZoom(double val){

            return (float)val * Constants.PdfZoom;
        }

        public static PdfContentByte SetTextWithinCoordinates(MdPiece piece, string text, PdfContentByte cb)
        {
            var upperRight = piece.Coordinates.Corners[2];
            var lowerLeft = piece.Coordinates.Corners[0];
            ColumnText ct = new ColumnText(cb);

            ct.SetSimpleColumn(
                new Phrase(
                    new Chunk(
                        text, 
                        FontFactory.GetFont(
                            FontFactory.HELVETICA, 7, Font.NORMAL
                        )
                    )
                ),
                StaticHelpers.ApplyZoom(lowerLeft.X) + 5, 
                StaticHelpers.ApplyZoom(lowerLeft.Y) + 5, 
                StaticHelpers.ApplyZoom(upperRight.X) - 5, 
                StaticHelpers.ApplyZoom(upperRight.Y) - 5, 
                Element.ALIGN_LEFT | Element.ALIGN_TOP, 100
            );

            ct.Go();
            return cb;
        }

        public static double GetLengthDiagonal(MdLine vertLeg, MdLine horLeg)
        {
            var vertLength = vertLeg.End.Y - vertLeg.Start.Y;
            var horLength = horLeg.End.X - horLeg.Start.X;
            var temp = (vertLength * vertLength) + (horLength * horLength);
            return Math.Sqrt(temp);
        }

        public static string GetBoardId()
        {
            Char c = (Char)(65 + (boardInc - 1));
            boardInc++;
            return c.ToString();
        }
    }
}
