using System;
using System.Collections.Generic;
using CLGenerator.MD.MdMaterial;
using CLGenerator.MD.MdMeasure;
using CLGenerator.MD.MdStructure;

namespace CLGenerator.MD.Structure
{
    // a box that fits inside of a drawer
    public class MdDrawerAccessory : IMdCabinetAccessory
    {
        public IMdCabinetAccessory Dec { get; private set; }
        MWidth _slideOffset;
        MHeight _drawerHeight;
        MHeight _boxHeight;
        Material _material;
        MdDoorAccessory _drawerFront;

        public MdDrawerAccessory(MHeight boxHeight, MHeight drawerHeight, MWidth slideOffset, Material material, MdDoorAccessory drawerFront)
        {
            _drawerHeight = drawerHeight;
            _boxHeight = boxHeight;
            _slideOffset = slideOffset;
            _material = material;
            _drawerFront = drawerFront;
        }

        public List<MdDimension> PlaceAccessory(List<MdDimension> dims, IMdBaseCabinet baseStructure)
        {
            if (Dec != null)
                dims = Dec.PlaceAccessory(dims, baseStructure);

            // add drawer front door style
            dims = _drawerFront.PlaceAccessory(
                dims,
                new MdCase(
                    new MdDimension(
                        new MdRectangle(
                            baseStructure.Dim.X,
                            _drawerHeight.GetMeasure()
                        ),
                        "drFrnt",
                        baseStructure.Dim.ColorFill,
                        _material
                    ),
                    baseStructure.Depth,
                    new Ma14Backing(),
                    baseStructure.Rise,
                    null
                )
            );


            // add drawer box
            dims = new MdClosedTopCase(
                new MdCase(
                    new MdDimension(
                        new MdRectangle(
                            baseStructure.InsideWidth() - _slideOffset.GetMeasure() * 2,
                            baseStructure.Depth.GetMeasure()
                        ),
                        "drbx",
                        baseStructure.Dim.ColorFill,
                        _material
                    ),
                    new MDepth(_boxHeight.GetMeasure()),
                    _material,
                    new MWidth(0),
                    null
                )
            ).PlaceDimension(dims);

            return dims;
        }
    }
}
