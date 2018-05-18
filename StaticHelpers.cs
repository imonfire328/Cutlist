using System;
namespace CLGenerator
{
    public class StaticHelpers
    {
        public StaticHelpers()
        {
        }

        public static double GetTextCenterPoint(string text, int textSize){
            double width = 0;
            foreach (char c in text){
                width += textSize;
            }
            return width;
        }
    }
}
