using BPS;
using BPS.InGame.Error;
using BPS.Population;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FeedbackManagerTools
{
    public static bool GetActionErrors(Action a,
        ActionCostError ace,
        ActionRangeTypeError arte,
        ActionTargetTypeError atte,
        ActionTargetDiplomacyError atde,
        ActionTargetOwnerError atoe,
        out string errorMsg)
    {
        errorMsg = "";

        if (!ActionError_Costs(ace, out errorMsg))
            return false;

        if (!ActionError_RangeType(arte, out errorMsg))
            return false;

        if (!ActionError_TargetType(atte, out errorMsg))
            return false;

        if (!ActionError_TargetDiplomacy(atde, out errorMsg))
            return false;

        if (!ActionError_OwnerError(atoe, out errorMsg))
            return false;

        return true;
    }

    private static bool ActionError_Costs(ActionCostError ace, out string errorMsg)
    {
        errorMsg = "";
        switch (ace)
        {
            case ActionCostError.IN_COOLDOWN:
                errorMsg = "This action is still in Cooldown.";
                break;
            case ActionCostError.NO_MONEY:
                errorMsg = "You don't have enough Money.";
                break;
            case ActionCostError.NO_HP:
                errorMsg = "Your unit doesn't have enough HP.";
                break;
            case ActionCostError.NO_MP:
                errorMsg = "Your unit doesn't have enough MP.";
                break;
        }
        return (errorMsg == "");
    }

    private static bool ActionError_RangeType(ActionRangeTypeError arte, out string errorMsg)
    {
        errorMsg = "";
        switch (arte)
        {
            case ActionRangeTypeError.OUTSIDE_MIN_RANGE:
                errorMsg = "Target is outside the minimum range.";
                break;
            case ActionRangeTypeError.OUTSIDE_MAX_RANGE:
                errorMsg = "Target is outside the maximum range.";
                break;
            case ActionRangeTypeError.OUTSIDE_PLAYING_AREA:
                errorMsg = "Target is outside the playing area.";
                break;
        }
        return (errorMsg == "");
    }

    private static bool ActionError_TargetType(ActionTargetTypeError atte, out string errorMsg)
    {
        errorMsg = "";
        switch (atte)
        {
            case ActionTargetTypeError.NOT_AN_BUILDING:
                errorMsg = "The target is not an building.";
                break;
            case ActionTargetTypeError.NOT_AN_UNIT:
                errorMsg = "The target is not an unit.";
                break;
        }
        return (errorMsg == "");
    }

    private static bool ActionError_TargetDiplomacy(ActionTargetDiplomacyError atde, out string errorMsg)
    {
        errorMsg = "";
        switch (atde)
        {
            case ActionTargetDiplomacyError.NOT_NEUTRAL:
                errorMsg = "You must select an Neutral unit.";
                break;
            case ActionTargetDiplomacyError.NOT_ALLIED:
                errorMsg = "You must select an Allied unit.";
                break;
            case ActionTargetDiplomacyError.NOT_ENEMY:
                errorMsg = "You must select an Enemy unit.";
                break;
        }
        return (errorMsg == "");
    }

    private static bool ActionError_OwnerError(ActionTargetOwnerError atoe, out string errorMsg)
    {
        errorMsg = "";
        switch (atoe)
        {
            case ActionTargetOwnerError.NOT_SELF:
                errorMsg = "You must select something yourself control.";
                break;
            case ActionTargetOwnerError.NOT_OTHER_PLAYER:
                errorMsg = "You must select something other player's control.";
                break;
            case ActionTargetOwnerError.NOT_THE_CITY:
                errorMsg = "You must select something controlled by the city.";
                break;
        }
        return (errorMsg == "");
    }
}
