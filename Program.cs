using System;
using System.Collections.Generic;
using CLGenerator.BD;
using CLGenerator.FC;
using CLGenerator.MD;
using CLGenerator.MD.MdMaterial;
using CLGenerator.MD.MdMeasure;
using CLGenerator.MD.Structure;
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
            var doorBack = new Ma12Mdf();
            var doorstyles = new Ma14MDF();
            var shelfInset = new MDepth(.5);
            var rise = new MWidth(3.5);


            var dimList = new MdDimList
            (
                new List<IMdDimension>(){

                    new MdDimension(
                        new MdRectangle(
                            23.75, 82
                        ),
                        "cent tabletop",
                        StaticHelpers.ColorRange(new Random()),
                        boardMaterial
                    ),

                    new MdStructureCopier(
                        new MMultiple(2),
                        new MdDimension(
                            new MdRectangle(32, 21),
                            "out tabletop",
                            StaticHelpers.ColorRange(new Random()),
                            boardMaterial
                        )
                    ),

                    new MdStructureCopier(
                        new MMultiple(2),
                        new MdExposedTopCase(
                            new MdCase(
                                new MdDimension(
                                    new MdRectangle(32, 32),
                                    "out cab",
                                    StaticHelpers.ColorRange(new Random()),
                                    boardMaterial
                                ),
                                new MDepth(20),
                                backingMaterial,
                                rise,
                                new MdAccessoryCopier(
                                    new MMultiple(2),
                                    new MdShelfAccessory(
                                        shelfInset,
                                        new MdDoubleDoorAccessory(
                                            new MdShakerDoorAccessory(
                                                new MWidth(3), 
                                                doorstyles
                                            ),
                                            doorBack
                                        )
                                    )
                                )
                            )
                        )    
                    ),
                   
                    new MdStructureCopier(
                        new MMultiple(2),
                        new MdExposedTopCase(
                            new MdCase(
                                new MdDimension(
                                    new MdRectangle(32, 32),
                                    "in cab",
                                    StaticHelpers.ColorRange(new Random()),
                                    boardMaterial
                                ),
                                new MDepth(23),
                                backingMaterial,
                                rise,
                                new MdAccessoryCopier(
                                    new MMultiple(2),
                                    new MdShelfAccessory(
                                        shelfInset,
                                        new MdDoubleDoorAccessory(
                                            new MdShakerDoorAccessory(
                                                new MWidth(3),
                                                doorstyles
                                            ),
                                            doorBack
                                        )
                                    )
                                )
                            )
                        )
                    ),

                    // bookcases
                    new MdStructureCopier(
                        new MMultiple(2),
                        new MdClosedTopCase(
                            new MdCase(
                                new MdDimension(
                                    new MdRectangle(40, 72),
                                    "book",
                                    StaticHelpers.ColorRange(new Random()),
                                    boardMaterial
                                ),
                                new MDepth(11.5), 
                                backingMaterial, 
                                rise,
                                new MdAccessoryCopier(
                                    new MMultiple(4),
                                    new MdShelfAccessory(
                                        shelfInset,
                                        null
                                    )
                                )
                            )
                        )
                    ),

                    new MdExposedTopCase(
                        new MdCase(
                            new MdDimension(
                                new MdRectangle(
                                    18, 32
                                ),
                                "cenCab",
                                StaticHelpers.ColorRange(new Random()),
                                boardMaterial
                            ),
                            new MDepth(23),
                            backingMaterial,
                            rise,
                            new MdShelfAccessory(
                                new MDepth(.75),
                                new MdAccessoryCopier(
                                    new MMultiple(3),
                                    new MdDrawerAccessory(
                                        new MHeight(6),
                                        new MHeight(7.75),
                                        new MWidth(.5),
                                        new Ma12BirchPly(),
                                        new MdDoorAccessory(
                                            new MdShakerDoorAccessory(
                                                new MWidth(3),
                                                doorstyles
                                            ),
                                            doorBack,
                                            null
                                        )
                                    )
                                )
                            )
                        )
                    ),

                    new MdClosedTopCase(
                        new MdCase(
                            new MdDimension(
                                new MdRectangle(66, 6),
                                "topBrg",
                                StaticHelpers.ColorRange(new Random()),
                                boardMaterial
                            ),
                            new MDepth(6),
                            boardMaterial,
                            new MWidth(0),
                            null
                        )
                    )
                }
            );

            var kerf = 1;

            var operations = new OperationTypeFactory(
                    "CrossCut",
                    dimList,
                    kerf
                ).GetInstance().Build();

            new Reader(
                operations
            ).CreatePdf();
        }
    }
}
