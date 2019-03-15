using BPS.Population;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zem.Directions;

public static class BPSHelperFunctions
{
    public static float RoundToMultipleOf(float value, float multiple)
    {
        float signal, remainder;
        float result;

        signal = Mathf.Sign(value);
        result = value * signal;
        remainder = result % multiple;
        result -= remainder;

        if (remainder >= multiple / 2)
            result += multiple;

        return result * signal;
    }

    public static bool combinePercentiles(int[] pcts, out int[] result)
    {
        result = new int[pcts.Length];

        float allPcts = 0;
        foreach (var item in pcts)
            allPcts += item;
        float correction = (100 - allPcts) / allPcts;

        if (correction < 0)
        {
            for (int i = 0; i < pcts.Length; i++)
                result[i] = pcts[i] + Mathf.FloorToInt(pcts[i] * correction);
            return true;
        }
        return false;
    }

    public static bool checkCountersAndThreshold(int counter, int allCounters, int threshold, int minimumRequired)
    {
        if (threshold <= 0)
            return false;
        float valueA = (float)counter / allCounters;
        float valueB = (float)threshold / 100;
        return ((counter < minimumRequired) || (valueA <= valueB));
    }

    public static Tile TileWalk(Tile currentTile, Directions directionToGo, int remainingSteps)
    {
        if (currentTile == null)
            return null;
        if (directionToGo == Directions.SW)
            return null;

        if (remainingSteps <= 0)
        {
            return currentTile;
        }
        else
        {
            Tile nextTile;
            switch (directionToGo)
            {
                case Directions.S:
                    nextTile = currentTile.groundNeighbours[(int)Directions.S];
                    if (nextTile)
                        return TileWalk(nextTile, directionToGo, remainingSteps - 1);
                    else
                        return null;
                case Directions.W:
                    nextTile = currentTile.groundNeighbours[(int)Directions.W];
                    if (nextTile)
                        return TileWalk(nextTile, directionToGo, remainingSteps - 1);
                    else
                        return null;
                case Directions.N:
                    nextTile = currentTile.groundNeighbours[(int)Directions.N];
                    if (nextTile)
                        return TileWalk(nextTile, directionToGo, remainingSteps - 1);
                    else
                        return null;
                case Directions.E:
                    nextTile = currentTile.groundNeighbours[(int)Directions.E];
                    if (nextTile)
                        return TileWalk(nextTile, directionToGo, remainingSteps - 1);
                    else
                        return null;
                default:
                    return null;
            }
        }
    }

    public static Rect calculateSelectionBox(Bounds selectionBounds, Rect playingArea)
    {
        //shorthand for the coordinates of the centre of the selection bounds
        float cx = selectionBounds.center.x;
        float cy = selectionBounds.center.y;
        float cz = selectionBounds.center.z;
        //shorthand for the coordinates of the extents of the selection bounds
        float ex = selectionBounds.extents.x;
        float ey = selectionBounds.extents.y;
        float ez = selectionBounds.extents.z;

        //Determine the screen coordinates for the corners of the selection bounds
        ArrayList corners = new ArrayList();
        corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx + ex, cy + ey, cz + ez)));
        corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx + ex, cy + ey, cz - ez)));
        corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx + ex, cy - ey, cz + ez)));
        corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx - ex, cy + ey, cz + ez)));
        corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx + ex, cy - ey, cz - ez)));
        corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx - ex, cy - ey, cz + ez)));
        corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx - ex, cy + ey, cz - ez)));
        corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx - ex, cy - ey, cz - ez)));

        //Determine the bounds on screen for the selection bounds
        Bounds screenBounds = new Bounds((Vector3)corners[0], Vector3.zero);
        for (int i = 1; i < corners.Count; i++)
        {
            screenBounds.Encapsulate((Vector3)corners[i]);
        }

        //Screen coordinates start in the bottom left corner, rather than the top left corner
        //this correction is needed to make sure the selection box is drawn in the correct place
        float selectBoxTop = playingArea.height - (screenBounds.center.y + screenBounds.extents.y);
        float selectBoxLeft = screenBounds.center.x - screenBounds.extents.x;
        float selectBoxWidth = 2 * screenBounds.extents.x;
        float selectBoxHeight = 2 * screenBounds.extents.y;

        return new Rect(selectBoxLeft, selectBoxTop, selectBoxWidth, selectBoxHeight);
    }

    public static string GenerateActionInstructions(Action a)
    {
        string result = "Left Click - Confirm action over target\nRight Click - Cancel selected action";
        if (a.castType != BPS.InGame.CastType.TARGETABLE)
            return result;

        result += "\n\n";
        switch (a.targetType)
        {
            case BPS.InGame.TargetType.NO_TYPE:
                result += "No target type identified.";
                break;
            case BPS.InGame.TargetType.POINT:
                result += "Target must be any point in the map.";
                break;
            case BPS.InGame.TargetType.BUILDING:
                result += "Target must be an Building.";
                break;
            case BPS.InGame.TargetType.UNIT:
                result += "Target must be an Unit.";
                break;
            case BPS.InGame.TargetType.ANY:
                result += "Target must be either an Building or an Unit.";
                break;
        }

        result += "\n";
        switch (a.targetDiplomacy)
        {
            case BPS.InGame.TargetDiplomacy.NO_DIPLOMACY:
                result += "No target diplomacy identified.";
                break;
            case BPS.InGame.TargetDiplomacy.NEUTRAL:
                result += "Target must be Neutral to you.";
                break;
            case BPS.InGame.TargetDiplomacy.ALLY:
                result += "Target must be from an Ally.";
                break;
            case BPS.InGame.TargetDiplomacy.ENEMY:
                result += "Target must be from an Enemy";
                break;
            case BPS.InGame.TargetDiplomacy.ANY:
                result += "Target can be from any side.";
                break;
        }

        result += "\n";
        switch (a.targetOwner)
        {
            case BPS.InGame.TargetOwner.NO_OWNER:
                result += "Target cannot have any owner.";
                break;
            case BPS.InGame.TargetOwner.SELF:
                result += "Target owner must be you.";
                break;
            case BPS.InGame.TargetOwner.ANOTHER_PLAYER:
                result += "Target owner must another player.";
                break;
            case BPS.InGame.TargetOwner.ANY_PLAYER:
                result += "Target owner must be any player.";
                break;
            case BPS.InGame.TargetOwner.THE_CITY:
                result += "Target owner must be the city.";
                break;
        }

        return result;
    }

    public static List<PlayerObject> SelectionRelevantSubgroup(List<PlayerObject> fullSelection)
    {
        List<PlayerObject> result = new List<PlayerObject>();

        //  FIRST, we identify wich unit has the biggest RELEVANCY value
        int relevancyAux = -1;
        PlayerObject objAux = null;
        foreach (PlayerObject obj in fullSelection)
        {
            if (obj.relevancy > relevancyAux)
            {
                relevancyAux = obj.relevancy;
                objAux = obj;
            }
        }

        //  THEN, we return only the similar units
        foreach (PlayerObject obj in fullSelection)
        {
            if (obj.Equals(objAux))
            {
                result.Add(obj);
            }
        }

        return result;
    }

    public static float PoliticalPositionComparison(PoliticalPosition pp1, PoliticalPosition pp2)
    {
        float result = 1F;

        if (pp1 != PoliticalPosition.None && pp2 != PoliticalPosition.None)
        {
            //VERTICAL AXIS
            int pp1div = (int)pp1 / 3;
            int pp2div = (int)pp2 / 3;
            //HORIZONTAL AXIS
            int pp1mod = (int)pp1 % 3;
            int pp2mod = (int)pp2 % 3;

            float div = (pp1div - pp2div);
            div *= Mathf.Sign(div);
            float mod = (pp1mod - pp2mod);
            mod *= Mathf.Sign(mod);

            result = 1.25F;
            result -= 0.25F * (div + mod);
        }

        return result;
    }
}
