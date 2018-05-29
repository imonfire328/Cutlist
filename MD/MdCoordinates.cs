using System;
namespace CLGenerator.MD
{
    /// <summary>
    /// [bottomL, topL, topR, bottomR]
    /// </summary>
    public class MdCoordinates
    {
        public MdPoint[] Corners { get; private set; }


        public MdCoordinates(MdPoint r, MdPoint t, MdPoint l, MdPoint b)
        {
            Corners = new MdPoint[] { r, t, l, b };
        }

        /// <summary>
        /// [bottom right, bottom top, tono
        /// </summary>
        /// <param name="start">Start.</param>
        /// <param name="dim">Dim.</param>
        public MdCoordinates(MdPoint start, MdDimension dim)
        {
            Corners = new MdPoint[]
            {
                start,
                new MdPoint(start.X, dim.Y + start.Y ),
                new MdPoint(start.X + dim.X, start.Y + dim.Y),
                new MdPoint(start.X + dim.X, start.Y ),
            };
        }


    }
}
