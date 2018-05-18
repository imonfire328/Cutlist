﻿using System;
using System.Collections.Generic;

namespace CLGenerator.MD
{
    public class MdSection 
    {
        public MdPoint Start { get; private set; }
        public MdDimension Dimension { get; private set; }


        public MdSection(MdPoint start, MdDimension dimension){
            Start = start;
            Dimension = dimension;
        }

        public bool Collides(MdPiece piece){
            if(piece.Collides(Dimension, Start)){
                return true; 
            }
            return false;
        }
    }
}
