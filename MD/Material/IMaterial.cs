using System;
using CLGenerator.MD.MdMeasure;

namespace CLGenerator.MD.MdMaterial
{
    public interface IMaterial
    {
        MThick Thickness { get; }
        MCost Cost { get; }
        MSqFt SqFt { get;  }
    }

    public abstract class Material : IMaterial
    {
        public MThick Thickness { get; private set; }
        public MCost Cost { get; private set; }
        public MSqFt SqFt { get; private set; }

        public Material(MThick thickness, MCost cost, MSqFt sqft)
        {
            Thickness = thickness;
            Cost = cost;
            SqFt = sqft;
        }
    }


    public abstract class SheetMaterial : Material{
        public SheetMaterial(MThick thickness, MCost cost, MSqFt sqft): base(
            thickness, 
            cost, 
            sqft){}
    }


    public abstract class BoardMaterial : Material{
        public BoardMaterial(MThick thickness, MCost cost, MSqFt sqft): base(
            thickness, 
            new MCost(
                cost.GetMeasure() * sqft.GetMeasure()
            ), 
            sqft){}
    }


    public class Ma34MDF : SheetMaterial
    {
        public Ma34MDF() : base(
            new MThick(.75), 
            new MCost(27.50), 
            new MSqFt(49, 97)){}
    }

    public class Ma34PinePly : SheetMaterial
    {
        public Ma34PinePly() : base(
            new MThick(.72), 
            new MCost(35.0), 
            new MSqFt(48, 96)){}
    }

    public class Ma14Backing : SheetMaterial
    {
        public Ma14Backing() : base(
            new MThick(.23), 
            new MCost(15.0), 
            new MSqFt(48, 96)){ }
    }

    public class Ma12BirchPly : SheetMaterial
    {
        public Ma12BirchPly() : base(
            new MThick(.5), 
            new MCost(35.0), 
            new MSqFt(48, 96)){}
    }

    public class MaPoplar : BoardMaterial{
        public MaPoplar(MSqFt sqft) : base(
            new MThick(.75), 
            new MCost(3.00), 
            sqft){}
    }
}
