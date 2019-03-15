using BPS.InGame;
using BPS.InGame.Error;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActionVerifier
{
    public static bool CheckCosts(Action a, out ActionCostError costsError)
    {
        costsError = ActionCostError.OK;

        if (a.callingAction.cooldown_current > 0)
        {
            costsError = ActionCostError.IN_COOLDOWN;
        }
        else if (a.caster.GetOwnerOrController().money < a.moneyCost)
        {
            costsError = ActionCostError.NO_MONEY;
        }
        else if (a.caster.hp_current < a.hpCost)
        {
            costsError = ActionCostError.NO_HP;
        }
        else if (a.caster.mp_current < a.mpCost)
        {
            costsError = ActionCostError.NO_MP;
        }

        return (costsError == ActionCostError.OK);
    }

    public static bool CheckRangeType(Action a, out ActionRangeTypeError rangeTypeError)
    {
        rangeTypeError = ActionRangeTypeError.OK;

        PO_Unit casterAsUnit = a.caster as PO_Unit;
        float distance;

        //TODO CHECK IF TARGET IS WITHIN PLAY BOUNDS!

        if (a.targetObj)
        {
            distance = Vector3.Distance(a.caster.transform.position, a.targetObj.transform.position);
        }
        else
        {
            distance = Vector3.Distance(a.caster.transform.position, a.targetPos);
        }
        switch (a.rangeType)
        {
            case RangeType.SELF:
                //Nothing to check here.
                break;
            case RangeType.MELEE:
                //Nothing to check here (TODO YET).
                break;
            case RangeType.RANGED:
                //Buildings can't move in a way they can use the ordered action. So they will generate Range Errors.
                //On the other hand, Units will move closer/further from the target.
                if (!casterAsUnit)
                {
                    if (distance < a.minCastRange)
                    {
                        rangeTypeError = ActionRangeTypeError.OUTSIDE_MIN_RANGE;
                    }
                    else if (distance > a.maxCastRange)
                    {
                        rangeTypeError = ActionRangeTypeError.OUTSIDE_MAX_RANGE;
                    }
                }
                else
                {
                    if (distance < a.minCastRange)
                    {
                        casterAsUnit.UpdateDestinationPosition(a.targetPos, a.targetObj, a.minCastRange, true);
                    }
                    else if (distance > a.maxCastRange)
                    {
                        casterAsUnit.UpdateDestinationPosition(a.targetPos, a.targetObj, a.maxCastRange, false);
                    }
                }
                break;
            case RangeType.GLOBAL:
                //Nothing to check here (TODO I think...).
                break;
        }

        return (rangeTypeError == ActionRangeTypeError.OK);
    }

    public static bool CheckTargetType(Action a, out ActionTargetTypeError targetTypeError)
    {
        targetTypeError = ActionTargetTypeError.OK;

        //PlayerObject targetObjAsPlayerObject = a.targetObj as PlayerObject;
        switch (a.targetType)
        {
            case TargetType.NO_TYPE:
                //Nothing to check here (?)
                break;
            case TargetType.POINT:
                //TODO add code to verify if the selected point is inside the city/map bounds
                break;
            case TargetType.BUILDING:
                if (a.targetObj && a.targetObj.levelObjectType != LevelObjectType.BUILDING)
                {
                    targetTypeError = ActionTargetTypeError.NOT_AN_BUILDING;
                }
                break;
            case TargetType.UNIT:
                if (a.targetObj && a.targetObj.levelObjectType != LevelObjectType.UNIT)
                {
                    targetTypeError = ActionTargetTypeError.NOT_AN_UNIT;
                }
                break;
            case TargetType.ANY:
                //Nothing to check here (?)
                break;
        }

        return targetTypeError == ActionTargetTypeError.OK;
    }

    public static bool CheckTargetDiplomacy(Action a, out ActionTargetDiplomacyError targetDiplomacyError)
    {
        targetDiplomacyError = ActionTargetDiplomacyError.OK;

        PlayerObject targetObjAsPlayerObject = a.targetObj as PlayerObject;
        switch (a.targetDiplomacy)
        {
            case TargetDiplomacy.NO_DIPLOMACY:
                //Nothing to check here (?)
                break;
            case TargetDiplomacy.NEUTRAL:
                //TODO add code to verify if the selected point is inside the city/map bounds
                break;
            case TargetDiplomacy.ALLY:
                if (targetObjAsPlayerObject && ((a.targetObj == null) || (a.caster.GetOwnerOrController() != targetObjAsPlayerObject.GetOwnerOrController())))
                {
                    targetDiplomacyError = ActionTargetDiplomacyError.NOT_ALLIED;
                }
                break;
            case TargetDiplomacy.ENEMY:
                if (targetObjAsPlayerObject && ((a.targetObj == null) || (a.caster.GetOwnerOrController() == targetObjAsPlayerObject.GetOwnerOrController())))
                {
                    targetDiplomacyError = ActionTargetDiplomacyError.NOT_ENEMY;
                }
                break;
        }

        return targetDiplomacyError == ActionTargetDiplomacyError.OK;
    }

    public static bool CheckTargetOwner(Action a, out ActionTargetOwnerError targetOwnerError)
    {
        targetOwnerError = ActionTargetOwnerError.OK;

        PlayerObject targetObjAsPlayerObject = a.targetObj as PlayerObject;
        CityObject targetObjAsCityObject = a.targetObj as CityObject;
        switch (a.targetOwner)
        {
            case TargetOwner.NO_OWNER:
                //Nothing to check here (?)
                break;
            case TargetOwner.SELF:
                if (!targetObjAsPlayerObject || a.caster.GetOwnerOrController() != targetObjAsPlayerObject.GetOwnerOrController())
                {
                    targetOwnerError = ActionTargetOwnerError.NOT_SELF;
                }
                break;
            case TargetOwner.ANOTHER_PLAYER:
                if (!targetObjAsPlayerObject || a.caster.GetOwnerOrController() == targetObjAsPlayerObject.GetOwnerOrController())
                {
                    targetOwnerError = ActionTargetOwnerError.NOT_OTHER_PLAYER;
                }
                break;
            case TargetOwner.ANY_PLAYER:
                if (!targetObjAsPlayerObject || a.caster.GetOwnerOrController() != null)
                {
                    targetOwnerError = ActionTargetOwnerError.NOT_ANY_PLAYER;
                }
                break;
            case TargetOwner.THE_CITY:
                if (!targetObjAsCityObject)
                {
                    targetOwnerError = ActionTargetOwnerError.NOT_THE_CITY;
                }
                break;
        }

        return targetOwnerError == ActionTargetOwnerError.OK;
    }
}
