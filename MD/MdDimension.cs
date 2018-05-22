using System;
using System.Collections.Generic;
using CLGenerator.MD.MdMaterial;
using CLGenerator.MD.MdMeasure;
using CLGenerator.MD.MdStructure;
using iTextSharp.text;

namespace CLGenerator.MD
{
    /// <summary>
    /// Represents any area of space with a seperate height and width
    /// </summary>
    public class MdDimension : MdMetric, IDimension
    {
        public double Width { get; private set; }
        public double Height { get; private set; }
        public int Id { get; private set; }
        public string Name { get; private set; }
        public IMaterial Material { get; private set;  }

        //[0] dark , [1] light
        public BaseColor[] ColorFill {get; set; }

        public MdDimension(int id, MdDimension dim)
        {
            Id = id;
            Width = dim.Width;
            Height = dim.Height;
            Material = dim.Material;
            Name = dim.Name;
            ColorFill = dim.ColorFill;
        }

        public MdDimension(MdRectangle rect, MdDimension dim){
            Id = dim.Id;
            Width = rect.X;
            Height = rect.Y;
            Material = dim.Material;
            Name = dim.Name;
            ColorFill = dim.ColorFill;
        }

        public MdDimension(MdRectangle rect, string name, BaseColor[] colorFill, IMaterial material)
        {
            Width = rect.X;
            Height = rect.Y;
            Name = name;
            Material = material;
            ColorFill = colorFill;
        }


        public bool GreaterThan(MdDimension dim)
        {
            return dim.CalcArea() > CalcArea() ? false : true;
        }


        public List<MdDimension> PlaceDimension(List<MdDimension> dims){
            dims.Add(this);
            return dims;
        }


        public double CalcArea()
        {
            return Width * Height;
        }


        public override string ToString()
        {
            return Width + "x" + Height;
        }


        public override string ToBinary()
        {
            return base.ToBinary(Width, Height);
        }
    }

    public interface IDimension{
        List<MdDimension> PlaceDimension(List<MdDimension> dims);
    }

    public interface ICase{
        Case BaseStructure { get;} 
    }

    public interface ICompositeCase{
        CabCase OuterCase { get; }
    }

    public class DrawerBox : IStructure{
        
        public MdDimension Dim { get; private set; }
        CabCase _enclosure; 

        public DrawerBox(CabCase enclosure, MHeight height, MWidth slideOffset){
            _enclosure = enclosure;
            Dim = _enclosure.BaseStructure.Dim;
        }

        public List<MdDimension> PlaceDimension(List<MdDimension> dims){

            //use the enclosures base structure to add an open box without a top and front
            dims = _enclosure.BaseStructure.PlaceDimension(dims);

            // add a front, because drawers have fronts...
            dims.Add(new MdDimension(new MdRectangle(Dim.Width, Dim.Height), ":drawer front", Dim.ColorFill, Dim.Material));

            return dims;
        }
    }


    /// <summary>
    /// Represents a basic case with two walls, a bottom and a backing
    /// </summary>
    public class Case : IBaseStructure
    {
        public MdDimension Dim { get; private set; }
        public MDepth Depth { get; private set;  }
        public MThick BackThickness { get; private set; }
        private SheetMaterial _backMaterial; 

        public Case(MdDimension dim, MDepth depth, SheetMaterial backMaterial){
            Dim = dim;
            Depth = depth;
            BackThickness = backMaterial.Thickness;
            _backMaterial = backMaterial;
        }

        public double InsideWidth(){
            return Dim.Width - Dim.Material.Thickness.GetMeasure() * 2;
        }

        public double InsideHeight(){
            return Dim.Height - Dim.Material.Thickness.GetMeasure() * 2;
        }

        public double InsideDepth(){
            return Depth.GetMeasure() - BackThickness.GetMeasure();
        }

        public List<MdDimension> PlaceDimension(List<MdDimension> dims){ 
            dims.Add(new MdDimension(new MdRectangle(Depth.GetMeasure(), Dim.Height), Dim.Name + ":L wall", Dim.ColorFill, Dim.Material));
            dims.Add(new MdDimension(new MdRectangle(Dim.Height, Depth.GetMeasure()), Dim.Name + ":R wall", Dim.ColorFill, Dim.Material));
            dims.Add(new MdDimension(new MdRectangle(InsideWidth(), Depth.GetMeasure()), Dim.Name + ": bottom", Dim.ColorFill, Dim.Material));
            dims.Add(new MdDimension(new MdRectangle(Dim.Width - 1, Dim.Height), Dim.Name + ": back", Dim.ColorFill, _backMaterial));
            return dims;
        }
    }



    /// <summary>
    /// Represents a case with multiple shelves and a solid top
    /// </summary>
    public class BookCase : IStructure, ICase
    {
        public Case BaseStructure { get; private set; }
        MMultiple _shelves;
        MDepth _shelfInset;

        public BookCase(Case casse, MMultiple shelvesCount, MDepth shelfInset){
            BaseStructure = casse;
            _shelves = shelvesCount;
            _shelfInset = shelfInset;
        }

        public  List<MdDimension> PlaceDimension(List<MdDimension> dims){
            
            dims = BaseStructure.PlaceDimension(dims);

            // number of shelves
            for (int t = 0; t < _shelves.GetQuantity(); t++){
                dims = new HorizontalWall(BaseStructure).PlaceDimension(dims);
            }
            // place top
            dims = new HorizontalWall(BaseStructure).PlaceDimension(dims);
            return dims;
        }
    }



    /// <summary>
    /// Represents a cabinet case with multiple drawers
    /// </summary>
    public class DrawerCase : IStructure, ICompositeCase
    {
        public CabCase OuterCase { get; private set; }
        Case[] _varDrawerDimensions;
        DrawerBox _boxTemplate; 
        MMultiple _drawerCount; 

        public DrawerCase(CabCase outerCase, MMultiple drawerCount, MHeight drawerHeight, MWidth slideOffsetWidth)
        {
            OuterCase = outerCase;
            _drawerCount = drawerCount;
            _boxTemplate = new DrawerBox(OuterCase, drawerHeight, slideOffsetWidth);
        }

        public List<MdDimension> PlaceDimension(List<MdDimension> dims)
        {
            dims = OuterCase.PlaceDimension(dims);

            for(int i = 0; i < _drawerCount.GetQuantity(); i++){
                dims = _boxTemplate.PlaceDimension(dims);
            }
            return dims;
        }
    }



    /// <summary>
    /// represents a case with 2 horizontal support rails and multiple shelves
    /// </summary>
    public class CabCase : IStructure, ICase
    {
        public MMultiple _shelves;
        public MDepth _shelfInset; 
        public Case BaseStructure { get; private set; } 

        public CabCase(Case casee, MMultiple shelves, MDepth shelfInset)
        {
            BaseStructure = casee;
            _shelves = shelves;
            _shelfInset = shelfInset;
        }

        public List<MdDimension> PlaceDimension(List<MdDimension> dims)
        {
            dims = BaseStructure.PlaceDimension(dims);

            // top rails
            dims.Add(new MdDimension(new MdRectangle(BaseStructure.InsideWidth(), 8), BaseStructure.Dim.Name + ":F rail", BaseStructure.Dim.ColorFill, BaseStructure.Dim.Material));
            dims.Add(new MdDimension(new MdRectangle(BaseStructure.InsideWidth(), 8), BaseStructure.Dim.Name + ":R rail", BaseStructure.Dim.ColorFill, BaseStructure.Dim.Material));
            //shelves
            for (int t = 0; t < _shelves.GetQuantity(); t++)
            {
                dims = new HorizontalWall(BaseStructure).PlaceDimension(dims);
            }
            
            return dims;
        }
    }
}
