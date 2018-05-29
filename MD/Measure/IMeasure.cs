using System;
namespace CLGenerator.MD.MdMeasure
{
    public interface IMeasure
    {
        double GetMeasure();
    }

    public interface IQuantity
    {
        int GetQuantity();
    }

    public class MThick : IMeasure
    {
        double _thickness; 
        public MThick(double thickness = Constants.DefThick){
            _thickness = thickness;
        }

        public double GetMeasure(){
            return _thickness;
        }
    }

    public class MWidth : IMeasure{
        double _width; 

        public MWidth(double width){
            _width = width; 
        }
        public double GetMeasure(){
            return _width;
        }
    }

    public class MHeight : IMeasure{
        double _height; 

        public MHeight(double height){
            _height = height; 
        }

        public double GetMeasure(){
            return _height;
        }
    }

    public class MDepth : IMeasure{
        double _depth; 

        public MDepth(double depth){
            _depth = depth; 
        }

        public double GetMeasure(){
            return _depth; 
        }
    }

    public class MCost : IMeasure {
        double _cost; 
        public MCost(double cost){
            _cost = cost;
        }

        public double GetMeasure(){
            return _cost;
        }
    }

    public class MMultiple : IQuantity {
        int _multiple; 
        public MMultiple(int measure){
            _multiple = measure; 
        }

        public int GetQuantity(){
            return _multiple;
        }
    }

    public class MSqFt : IMeasure{
        double _sqft; 
        public MSqFt(double x, double y){
            _sqft = x * y / 144;
        }
        public double GetMeasure(){
            return _sqft;
        }
    }
}
