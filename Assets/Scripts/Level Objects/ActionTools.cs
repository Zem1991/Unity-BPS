using BPS.InGame;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionTools
{
    public static bool AdditionalMovement(Action a)
    {
        if (a.caster.levelObjectType == LevelObjectType.UNIT)
        {
            a.caster.UpdateDestinationPosition(a.targetPos, a.targetObj);
            return true;
        }
        else
        {
            a.SendErrorMessage(FeedbackManager.CANNOT_MOVE);
        }
        return false;
    }

    //public static bool AdditionalMovement(Action action, LevelObject targetObj, float distance, bool retreat)
    //{
    //    if (action.caster.levelObjectType == LevelObjectType.UNIT)
    //    {
    //        action.caster.UpdateDestinationPosition(action.targetPos, (targetObj ? targetObj : action.targetObj), distance, retreat);
    //        return true;
    //    }
    //    else
    //    {
    //        action.SendErrorMessage(FeedbackManager.CANNOT_MOVE);
    //    }
    //    return false;
    //}

    public static List<PlayerObject> Targets_SortByDistance(Vector3 position, List<PlayerObject> objects)
    {
        return objects.OrderBy(x => Vector2.Distance(position, x.transform.position)).ToList();
    }
}
