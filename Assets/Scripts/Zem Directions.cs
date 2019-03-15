using UnityEngine;

namespace Zem.Directions
{
    public enum Directions
    {
        NO_DIRECTION = -1,
        E = 0,
        NE = 1,
        N = 2,
        NW = 3,
        W = 4,
        SW = 5,
        S = 6,
        SE = 7
    }

    //public enum CardinalDirections
    //{
    //    NO_DIRECTION = -1,
    //    EAST = 0,
    //    NORTH = 1,
    //    WEST = 2,
    //    SOUTH = 3
    //}

    public static class DirectionsClass
    {
        public const int MAX_DIRECTIONS = 8;

        public static int DirectionAsDegrees(Directions dir)
        {
            int aux = (int)dir;
            return aux * 45;
        }

        public static void SnapToDireciton(float currentDir, out float dirAsFloat, out Directions dirAsEnum)
        {
            if (currentDir < 0)
                currentDir += 360;
            int aux = Mathf.RoundToInt(currentDir / 45);

            switch (aux)
            {
                case 0:
                    dirAsFloat = 0;
                    dirAsEnum = Directions.E;
                    break;
                case 1:
                    dirAsFloat = 45;
                    dirAsEnum = Directions.NE;
                    break;
                case 2:
                    dirAsFloat = 90;
                    dirAsEnum = Directions.N;
                    break;
                case 3:
                    dirAsFloat = 135;
                    dirAsEnum = Directions.NW;
                    break;
                case 4:
                    dirAsFloat = 180;
                    dirAsEnum = Directions.W;
                    break;
                case 5:
                    dirAsFloat = 225;
                    dirAsEnum = Directions.SW;
                    break;
                case 6:
                    dirAsFloat = 270;
                    dirAsEnum = Directions.S;
                    break;
                case 7:
                    dirAsFloat = 315;
                    dirAsEnum = Directions.SE;
                    break;
                default:
                    dirAsFloat = float.NaN;
                    dirAsEnum = Directions.NO_DIRECTION;
                    break;
            }
        }

        public static void SnapToCardinalDireciton(float currentDir, out float dirAsFloat, out Directions dirAsEnum)
        {
            if (currentDir < 0)
                currentDir += 360;
            int aux = Mathf.RoundToInt(currentDir / 90);

            switch (aux)
            {
                case 0:
                    dirAsFloat = 0;
                    dirAsEnum = Directions.E;
                    break;
                case 1:
                    dirAsFloat = 90;
                    dirAsEnum = Directions.N;
                    break;
                case 2:
                    dirAsFloat = 180;
                    dirAsEnum = Directions.W;
                    break;
                case 3:
                    dirAsFloat = 270;
                    dirAsEnum = Directions.S;
                    break;
                default:
                    dirAsFloat = float.NaN;
                    dirAsEnum = Directions.NO_DIRECTION;
                    break;
            }
        }

        public static Directions RandomDirection()
        {
            int amount = System.Enum.GetNames(typeof(Directions)).Length;
            amount--;   //Here we discount the "NO_DIRECTION" value.
            return (Directions)Random.Range(0, amount);
        }

        //THIS FUNCTION WAS USED BEFORE TO CREATE RIVERS.
        //THE SAID RIVERS KINDA SUCKED.
        //public static Directions randomDirectionTowardsDirection(Directions directionToGo,
        //    int repeatSame, int repeatDiagFront, int repeatLateral, int repeatDiagBack, int repeatOpposite)
        //{
        //    List<Directions> choices = new List<Directions>();
        //    int auxA, auxB;
        //    for (int i = 0; i < repeatSame; i++)
        //    {
        //        choices.Add(directionToGo);
        //    }
        //    for (int i = 0; i < repeatDiagFront; i++)
        //    {
        //        auxA = (int)directionToGo - 1;
        //        auxB = (int)directionToGo + 1;
        //        if (auxA < 0) auxA += 8;
        //        if (auxB >= 8) auxB -= 8;
        //        choices.Add((Directions)auxA);
        //        choices.Add((Directions)auxB);
        //    }
        //    for (int i = 0; i < repeatLateral; i++)
        //    {
        //        auxA = (int)directionToGo - 2;
        //        auxB = (int)directionToGo + 2;
        //        if (auxA < 0) auxA += 8;
        //        if (auxB >= 8) auxB -= 8;
        //        choices.Add((Directions)auxA);
        //        choices.Add((Directions)auxB);
        //    }
        //    for (int i = 0; i < repeatDiagBack; i++)
        //    {
        //        auxA = (int)directionToGo - 3;
        //        auxB = (int)directionToGo + 3;
        //        if (auxA < 0) auxA += 8;
        //        if (auxB >= 8) auxB -= 8;
        //        choices.Add((Directions)auxA);
        //        choices.Add((Directions)auxB);
        //    }
        //    for (int i = 0; i < repeatOpposite; i++)
        //    {
        //        auxA = (int)directionToGo - 4;
        //        if (auxA < 0) auxA += 8;
        //        choices.Add((Directions)auxA);
        //    }
        //    return choices[Random.Range(0, choices.Count)];
        //}
        //}
    }   
}