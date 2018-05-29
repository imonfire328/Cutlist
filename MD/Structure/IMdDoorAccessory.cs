using System;
using System.Collections.Generic;
using CLGenerator.BD;
using CLGenerator.MD.MdMaterial;
using CLGenerator.MD.MdMeasure;
using CLGenerator.MD.MdStructure;

namespace CLGenerator.MD.Structure
{

    public interface IMdDoorAccessory : IMdCabinetAccessory{}

    public class MdCrossDoorAccessory : IMdDoorAccessory
    {
        public IMdCabinetAccessory Dec { get; private set;  }
        MWidth _width;
        Material _material; 

        public MdCrossDoorAccessory(MWidth width, Material material, IMdCabinetAccessory dec){
            Dec = dec;
            _width = width;
            _material = material;
        }


        public List<MdDimension> PlaceAccessory(List<MdDimension> dims, IMdBaseCabinet baseStructure)
        {
            new MdShakerDoorAccessory(_width, _material).PlaceAccessory(dims, baseStructure);
            var diagLength = StaticHelpers.GetLengthDiagonal(
                // vertical line
                new MdLine(
                    new MdPoint(
                        _width.GetMeasure(),
                        _width.GetMeasure()
                    ), 
                    new MdPoint(
                        _width.GetMeasure(),
                        baseStructure.Dim.Y - _width.GetMeasure()
                    )
                ),
                // horizontal line
                new MdLine(
                    new MdPoint(
                       _width.GetMeasure(),
                       _width.GetMeasure()
                    ),
                    new MdPoint(
                        baseStructure.Dim.X - _width.GetMeasure(),
                        _width.GetMeasure()
                    )
                )
            );

            // full diagnal piece 
            dims.Add(new MdDimension(new MdRectangle(_width.GetMeasure(), diagLength), "st diag", baseStructure.Dim.ColorFill, _material));
            dims.Add(new MdDimension(new MdRectangle(_width.GetMeasure(), diagLength), "st t diag", baseStructure.Dim.ColorFill, _material));
            return dims;

        }
    }


    public class MdShakerDoorAccessory : IMdDoorAccessory{

        public IMdCabinetAccessory Dec { get; private set;  }
        MWidth _width;
        Material _material; 

        public MdShakerDoorAccessory(MWidth width, Material material)
        {
            _width = width;
            _material = material;
        }

        public List<MdDimension> PlaceAccessory(List<MdDimension> dims, IMdBaseCabinet baseStructure)
        {
            //style risers
            new MdStructureCopier(
                new MMultiple(2),
                new MdDimension(
                    new MdRectangle(
                        _width.GetMeasure(),
                        baseStructure.InsideWidth()
                    ),
                    "door risers",
                    baseStructure.Dim.ColorFill,
                    _material
                )
            ).PlaceDimension(dims);

            //style rails
            new MdStructureCopier(
               new MMultiple(2),
               new MdDimension(
                   new MdRectangle(
                        _width.GetMeasure(),
                        baseStructure.Dim.X - (_width.GetMeasure() * 2)
                   ),
                   "door rails",
                    baseStructure.Dim.ColorFill,
                   _material
               )
           ).PlaceDimension(dims);
            return dims;
        }
    }

    public class MdDoorAccessory : IMdCabinetAccessory
    {
        public IMdCabinetAccessory Dec { get; private set; }
        Material _backMaterial;
        IMdDoorAccessory _style;

        public MdDoorAccessory(IMdDoorAccessory style, Material backMaterial, IMdCabinetAccessory dec)
        {
            Dec = dec;
            _backMaterial = backMaterial;
            _style = style; 
        }

        public List<MdDimension> PlaceAccessory(List<MdDimension> dims, IMdBaseCabinet baseStructure)
        {
            if (Dec != null)
                dims = Dec.PlaceAccessory(dims, baseStructure);
            
            // create shaker horizontal strips;
            dims.Add(new MdDimension(new MdRectangle(baseStructure.Dim.X, baseStructure.InsideRiseHeight()), "door", baseStructure.Dim.ColorFill, _backMaterial));
            dims = _style.PlaceAccessory(dims, baseStructure);
            return dims;
        }
    }

    public class MdDoubleDoorAccessory : IMdCabinetAccessory
    {
        public IMdCabinetAccessory Dec { get; private set; }
        MdDoorAccessory _template;
        IMdDoorAccessory _style;
        Material _backMaterial;

        public MdDoubleDoorAccessory(IMdDoorAccessory style, Material backMaterial)
        {
            _style = style;
            _backMaterial = backMaterial;
        }

        public List<MdDimension> PlaceAccessory(List<MdDimension> dims, IMdBaseCabinet baseStructure)
        {
            if(Dec != null){
                Dec.PlaceAccessory(dims, baseStructure);
            }

            var halvedBaseStruct = new MdCase(
                baseStructure.Dim.GetHalfHorizontal(),
                baseStructure.Depth,
                baseStructure.BackMaterial,
                baseStructure.Rise,
                baseStructure.Additions
            );
                
            _template = new MdDoorAccessory(
               _style,
               _backMaterial, 
                null
            );

            //door one
            _template.PlaceAccessory(dims, halvedBaseStruct);
            //door two
            _template.PlaceAccessory(dims, halvedBaseStruct);
            return dims;
        }
    }
}
