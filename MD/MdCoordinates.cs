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


        public MdCoordinates(MdPoint start, MdDimension dim)
        {
            Corners = new MdPoint[]
            {
                start,
                new MdPoint(start.X, dim.Height + start.Y ),
                new MdPoint(start.X + dim.Width, start.Y + dim.Height),
                new MdPoint(start.X + dim.Width, start.Y ),
            };
        }
    }
}
