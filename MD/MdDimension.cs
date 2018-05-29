using System;
using System.Collections.Generic;
using CLGenerator.MD.MdMaterial;
using CLGenerator.MD.MdMeasure;
using CLGenerator.MD.MdStructure;
using CLGenerator.MD.Structure;
using iTextSharp.text;

namespace CLGenerator.MD
{
    /// <summary>
    /// Represents any area of space with a seperate height and width
    /// </summary>
    public class MdDimension : MdRectangle, IMdDimension
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public Material Material { get; private set;  }

        //[0] dark , [1] light
        public BaseColor[] ColorFill {get; set; }

        public  MdDimension(Material materail) : base(materail.X, materail.Y){
            ColorFill = new BaseColor[]{new BaseColor(0, 0, 0), new BaseColor(240, 240, 240)};
            Material = materail;
            Name = StaticHelpers.GetBoardId() + " " + materail.ToString();
        }

        public MdDimension(int id, MdDimension dim): base(dim.X, dim.Y){
            Id = id;
            Material = dim.Material;
            Name = dim.Name;
            ColorFill = dim.ColorFill;
        }

        public MdDimension(MdRectangle rect, MdDimension dim) : base(rect){
            Id = dim.Id;
            Material = dim.Material;
            Name = dim.Name;
            ColorFill = dim.ColorFill;
        }

        public MdDimension(MdRectangle rect, string name, BaseColor[] colorFill, Material material) : base(rect)
        {
            Name = name;
            Material = material;
            ColorFill = colorFill;
        }

        public MdDimension GetBase(){
            return this;
        }

        public MdDimension GetHalfVertical()
        {
            return new MdDimension(new MdRectangle(X, Y / 2), Name, ColorFill, Material);
        }

        public MdDimension GetHalfHorizontal()
        {
            return new MdDimension(new MdRectangle(X/ 2, Y), Name, ColorFill, Material);

        }

        public bool GreaterThan(MdDimension dim)
        {
            return dim.CalcArea() > dim.CalcArea() ? false : true;
        }

        public List<MdDimension> PlaceDimension(List<MdDimension> dims){
            dims.Add(this);
            return dims;
        }
    }

    /// <summary>
    /// an area that can be broken down to a dimension
    /// </summary>
    public interface IMdDimension{
        List<MdDimension> PlaceDimension(List<MdDimension> dims);
    }
}
