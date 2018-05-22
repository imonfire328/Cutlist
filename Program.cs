using System;
using System.Collections.Generic;
using CLGenerator.BD;
using CLGenerator.MD;
using CLGenerator.MD.MdMaterial;
using CLGenerator.MD.MdMeasure;
using CLGenerator.ST;
using iTextSharp.text;

namespace CLGenerator
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var boardMaterial = new Ma34MDF();
            var backingMaterial = new Ma14Backing();
            var shelfInset = new MDepth(.5);

            var dimList = new MdDimList
            (
                new List<IDimension>(){

                    new StructureCopier(
                        new MMultiple(2),
                        new CabCase(
                            new Case(
                                new MdDimension(
                                    new MdRectangle(27.5, 31),
                                    "out cab",
                                    StaticHelpers.ColorRange(new Random()),
                                    boardMaterial
                                ),
                                new MDepth(20),
                                backingMaterial
                            ),
                            new MMultiple(2),
                            shelfInset
                        )
                    ),

                    new StructureCopier(
                        new MMultiple(2),
                        new CabCase(
                            new Case(
                                new MdDimension(
                                    new MdRectangle(27, 31.5),
                                    "in cab",
                                    StaticHelpers.ColorRange(new Random()),
                                    boardMaterial
                                ),
                                new MDepth(23.5),
                                backingMaterial
                            ),
                            new MMultiple(2), 
                            shelfInset
                        )
                    ),

                    new StructureCopier(
                        new MMultiple(2),
                        new BookCase(
                            new Case(
                                new MdDimension(
                                    new MdRectangle(48, 62),
                                    "book",
                                    StaticHelpers.ColorRange(new Random()),
                                    boardMaterial
                                ),
                                new MDepth(11.5),
                                backingMaterial
                            ),
                            new MMultiple(4),
                            shelfInset
                        )
                    ),

                    new DrawerCase(
                        new CabCase(
                            new Case(
                                new MdDimension(
                                    new MdRectangle(17.5, 27),
                                    "cent cab",
                                    StaticHelpers.ColorRange(new Random()),
                                    boardMaterial
                                ),
                                new MDepth(23.5),
                                backingMaterial
                            ), 
                            new MMultiple(1), 
                            shelfInset
                        ),
                        new MMultiple(3),
                        new MHeight(7),
                        new MWidth(.5)
                    ),

                    new Case(
                        new MdDimension(
                            new MdRectangle(54, 6), 
                            "cent rail", 
                            StaticHelpers.ColorRange(new Random()),
                            boardMaterial
                        ),
                        new MDepth(10), 
                        backingMaterial
                    )
                }
            );

            var boardTemp = new MdDimension(new MdRectangle(49, 97), "Board Temp", new BaseColor[] { new BaseColor(100, 100, 100), new BaseColor(220, 220, 220) }, new Ma34MDF());
            var kerf = .5;


            new Reader(
                new Operator(
                    new StDimOrder(
                        new DcOrderWithinBoundsY(
                            new OrderAreaDec(
                                new StOrientToHeight(
                                    dimList.GetDimensions()
                                ).Implement()
                            ),
                            boardTemp
                        )
                    ),
                    new MdList(
                        boardTemp, 
                        new OrderPointsByY(),
                        new DcKerfRestriction(
                            new DcDimOverhangXRestriction(
                                
                            ),
                            kerf
                        )
                    ),
                    new StAlignVertical()
                ).Build()
            ).CreatePdf();
        }
    }
}
