using System;
using CLGenerator.MD.MdMaterial;
using CLGenerator.MD.MdMeasure;

namespace CLGenerator.MD
{
    public class MdCutListSettings
    {
        Material MainMaterial { get; set; }
        Material DoorMaterial { get; set; } 
        Material DoorStyleMaterial { get; set; } 
        MHeight MainHeight { get; set; } 
        MDepth MainDepth { get; set; } 
        MWidth Kerf { get; set; } 

        string CutPreference { get; set; } 
        bool LockLongToHeight { get; set; } 
        bool AllowRotation { get; set; } 
        bool RestrictOverhang { get; set;  }
        
        public MdCutListSettings()
        {
        }
    }
}
