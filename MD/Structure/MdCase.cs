using System;
using System.Collections.Generic;
using CLGenerator.MD.MdMaterial;
using CLGenerator.MD.MdMeasure;
using CLGenerator.MD.MdStructure;

namespace CLGenerator.MD.Structure
{
    /// <summary>
    /// Represents a basic case with two walls, a bottom and a backing
    /// </summary>
    public class MdCase : IMdBaseCabinet
    {
        public MdDimension Dim { get; private set; }
        public MDepth Depth { get; private set; }
        public MWidth Rise { get; private set; }
        public Material BackMaterial { get; private set; }
        public IMdCabinetAccessory Additions { get; private set; }

        public MdCase(IMdBaseCabinet structure)
        {
            Dim = structure.Dim;
            Depth = structure.Depth;
            BackMaterial = structure.BackMaterial;
            Rise = structure.Rise;
            Additions = structure.Additions;
        }

        public MdCase(MdDimension dim, MDepth depth, Material backMaterial, MWidth rise, IMdCabinetAccessory addition)
        {
            Dim = dim;
            Depth = depth;
            BackMaterial = backMaterial;
            Rise = rise;
            Additions = addition;
        }

        public double InsideWidth()
        {
            return Dim.X - Dim.Material.Thickness.GetMeasure() * 2;
        }

        public double InsideHeight()
        {
            return Dim.Y - Dim.Material.Thickness.GetMeasure() * 2;
        }

        public double InsideDepth()
        {
            return Depth.GetMeasure() - BackMaterial.Thickness.GetMeasure();
        }

        public double InsideRiseHeight()
        {
            return Dim.Y - Rise.GetMeasure();
        }

        public virtual List<MdDimension> PlaceDimension(List<MdDimension> dims)
        {
            dims.Add(new MdDimension(new MdRectangle(Depth.GetMeasure(), Dim.Y), Dim.Name + ":L wall", Dim.ColorFill, Dim.Material));
            dims.Add(new MdDimension(new MdRectangle(Dim.Y, Depth.GetMeasure()), Dim.Name + ":R wall", Dim.ColorFill, Dim.Material));
            dims.Add(new MdDimension(new MdRectangle(InsideWidth(), Depth.GetMeasure()), Dim.Name + ": bottom", Dim.ColorFill, Dim.Material));
            dims.Add(new MdDimension(new MdRectangle(Dim.X - 1, Dim.Y), Dim.Name + ": back", Dim.ColorFill, BackMaterial));

            if (Rise.GetMeasure() != 0)
            {
                dims.Add(new MdDimension(new MdRectangle(InsideWidth(), Rise.GetMeasure()), Dim.Name + ":bottom rail", Dim.ColorFill, Dim.Material));
                dims.Add(new MdDimension(new MdRectangle(InsideWidth(), Rise.GetMeasure()), Dim.Name + ":bottom rail", Dim.ColorFill, Dim.Material));
            }

            if(Additions != null){
                dims = Additions.PlaceAccessory(dims, this);
            }

            return dims;
        }
    }

}
