using System;
using System.Collections.Generic;
using CLGenerator.BD;
using CLGenerator.MD;
using CLGenerator.ST;

namespace CLGenerator
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var dims = new List<MdDimension>()
            {
                new MdDimension(13, 4),
                new MdDimension(13, 4),
                new MdDimension(13, 4),
                new MdDimension(13, 4),
                new MdDimension(13, 4),
                new MdDimension(13, 4),
                new MdDimension(13, 4),
                new MdDimension(13, 4),
                new MdDimension(13, 4),
                new MdDimension(13, 4),
                new MdDimension(2.5, 2.5),
                new MdDimension(2.5, 2.5),
                new MdDimension(2.5, 2.5),
                new MdDimension(2.5, 2.5),
                new MdDimension(2.5, 2.5),
                new MdDimension(3, 24), 
                new MdDimension(3, 24), 
                new MdDimension(3, 24)


            };

            new Reader(
                new Operator(
                    new StDimOrder(
                        new DcOrderWithinBoundsY(
                            new OrderAreaDec(
                                new StOrientToHeight(
                                    dims
                                ).Implement()
                            ),
                            96
                        )
                    ),
                    new MdList(
                        new MdDimension(96, 48), 
                        new OrderPointsByY(),
                        new DcKerfRestriction(
                            new DcDimOverhangXRestriction(
                                
                            ),
                            .5
                        )
                    ),
                    new StAlignVertical()
                ).Build()
            ).CreatePdf();
        }
    }
}
