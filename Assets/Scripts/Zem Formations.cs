using System.Collections.Generic;
using UnityEngine;

namespace Zem.Formations
{
    public enum formationTypes
    {
        //MELEE/STRONG units in the FRONT/OUTSIDE
        //RANGED/WEAK units in the BACK/INSIDE
        //Formations may also be TIGHT or LOOSE
        //Some formations may require a minimum amount of units to properly have the desired shape
        NO_FORMATION,           //Units will take randomized positions around the target position
        STRAIGHT_LINES,         //An straight, horizontal line
        STAGGERED_LINES,        //An zigzagged, horizontal line
        SHALLOW_ENCIRCLEMENT,   //An wide, shallow arc - centered around the target position
        DEEP_ENCIRCLEMENT,      //An tight, deep arc - centered around the target position
        BOX,                    //An rectangle - compensates with more in the front and/or less in the back
        CIRCLE,                 //An circle (no shit, Sherlock!)
        WEDGE,                  //An inverse-V formation - also known as Vanguard Formation
        LEFT_FLANK,             //An diagonal line, with the first unit at the leftmost position
        RIGHT_FLANK             //An diagonal line, with the first unit at the rightmost position
    }

    public static class Formation
    {
        public static List<Vector3> ChangeOffsets(List<Vector3> offsets, Vector3 position, float direction)
        {
            for (int i = 0; i < offsets.Count; i++)
            {
                Vector3 dir = offsets[i] - Vector3.zero;        // get point direction relative to pivot
                dir = Quaternion.Euler(0, direction, 0) * dir;  // rotate it
                offsets[i] = dir + Vector3.zero;                // calculate rotated point

                offsets[i] += position;
            }
            return offsets;
        }

        public static List<Vector3> Box(int unitCount, float biggestUnitSize)     //, int formationSizeIncrement)
        {
            //Step 1:   Identify how big the formation will need to be regarding its shape
            int boxFront = Mathf.CeilToInt(Mathf.Sqrt(unitCount));
            int boxSide = Mathf.FloorToInt(Mathf.Sqrt(unitCount));
            if (unitCount > boxFront * boxSide)
                boxSide = boxFront;
                                                                                //if (formationSizeIncrement < 0)
                                                                                //    formationSizeIncrement = 0;
                                                                                //boxFront += formationSizeIncrement;
                                                                                //boxSide += formationSizeIncrement;

            //Step 2:   Make all the Vector3s
            List<Vector3> result = new List<Vector3>();
            for (int row = 0; row < boxSide; row++)
                for (int col = 0; col < boxFront; col++)
                    result.Add(new Vector3(col, 0, row));

            //Step 3:   Spread the Vector3s
            float spread = (boxFront - 1) / 2F;
            for (int i = 0; i < result.Count; i++)
            {
                Vector3 newVal = result[i];
                newVal.x -= spread;
                newVal.z -= spread;
                newVal *= biggestUnitSize;
                result[i] = newVal;
            }

            //Step 4:   The End
            return result;
        }
    }
}