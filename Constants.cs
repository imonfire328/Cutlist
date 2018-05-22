using System;
namespace CLGenerator
{
    public static class Constants
    {
        public const int PdfZoom = 8;
        public const double DefThick = .75;
        public static double DefBacking = .25;

        public static int count = 0; 

        public static int counter(){
            count++;
            return count;
        }
    }
}
