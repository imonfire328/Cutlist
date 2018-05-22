using System;
using iTextSharp.text;

namespace CLGenerator
{
    public class StaticHelpers
    {
        public StaticHelpers()
        {
        }

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
    }
}
