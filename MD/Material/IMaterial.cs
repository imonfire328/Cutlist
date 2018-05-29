using System;
using CLGenerator.MD.MdMeasure;

namespace CLGenerator.MD.MdMaterial
{
    public interface IMaterial
    {
        MThick Thickness { get; }
        MCost Cost { get; }
        MSqFt SqFt { get; }
    }

    public abstract class Material : MdRectangle, IMaterial
    {
        public MThick Thickness { get; private set; }
        public MCost Cost { get; private set; }
        public MSqFt SqFt { get; private set; }
        public string Name { get; private set; }

        public Material(MThick thickness, MCost cost, MdRectangle rect, string name) : base(rect.X, rect.Y)
        {
            Thickness = thickness;
            Cost = cost;
            SqFt = new MSqFt(rect.X, rect.Y);
            Name = name;
        }

        public override string ToString()
        {
            return string.Format("Thickness={0}, Cost={1}", Thickness.GetMeasure(), Cost.GetMeasure(), SqFt.GetMeasure());
        }
    }

    public abstract class SheetMaterial : Material{
        public SheetMaterial(MThick thickness, MCost cost, MdRectangle rect, string name): base(
            thickness, 
            cost, 
            rect,
            name
        ){}
    }


    public abstract class BoardMaterial : Material{
        public BoardMaterial(MThick thickness, MCost cost, MdRectangle rect, string name): base(
            thickness, 
            new MCost(
                cost.GetMeasure() * rect.CalcArea() / 144 
            ), 
            rect, 
            name 
        ){}
    }


    public class Ma34MDF : SheetMaterial
    {
        public Ma34MDF() : base(
            new MThick(.75), 
            new MCost(27.50), 
            new MdRectangle(49, 97),
            "Mdf"
        ){}
    }

    public class Ma14MDF : SheetMaterial{
        public Ma14MDF() : base(
            new MThick(.25),
            new MCost(8.00),
            new MdRectangle(24, 48),
            "Mdf"
        ){}
    }

    public class Ma12Mdf : SheetMaterial
    {
        public Ma12Mdf() : base(
            new MThick(.5),
            new MCost(25),
            new MdRectangle(49, 97),
            "Mdf"
        ){}

    }

    public class Ma34PinePly : SheetMaterial
    {
        public Ma34PinePly() : base(
            new MThick(.72), 
            new MCost(35.0), 
            new MdRectangle(49, 97),
            "Pine Ply"
        ){}
    }

    public class Ma18Backing : SheetMaterial
    {
        public Ma18Backing() : base(
            new MThick(.23),
            new MCost(15.0), 
            new MdRectangle(49, 97),
            "Backer Board"
        ){}
    }

    public class Ma14Backing : SheetMaterial
    {
        public Ma14Backing() : base(
            new MThick(.23), 
            new MCost(15.0), 
            new MdRectangle(49, 97),
            "Backer Board"
        ){}
    }

    public class Ma12BirchPly : SheetMaterial
    {
        public Ma12BirchPly() : base(
            new MThick(.5), 
            new MCost(35.0), 
            new MdRectangle(49, 97),
            "Birch Ply"
        ){}
    }

    public class MaPoplar : BoardMaterial{
        public MaPoplar(MdRectangle rect) : base(
            new MThick(.75), 
            new MCost(3.00), 
            rect,
            "Poplar"
        ){}
    }
}
