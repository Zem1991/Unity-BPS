using BPS.InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Action {
    protected override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    protected override void Start () {
        base.Start();
        actionCommandType = ActionCommandType.ATTACK;
        castType = CastType.TARGETABLE;
        rangeType = RangeType.SAME_AS_ATTACK_RANGE;
        targetType = TargetType.ANY;
        targetDiplomacy = TargetDiplomacy.NO_DIPLOMACY;
        targetOwner = TargetOwner.NO_OWNER;
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
	}

    public override bool InitializeActionExecution()
    {
        if (base.InitializeActionExecution())
        {
            caster.attackPos = targetPos;
            caster.attackObj = targetObj as PlayerObject;
            return ActionTools.AdditionalMovement(this);
        }
        return false;
    }

    public override bool ExecuteAction()
    {
        if (base.ExecuteAction())
        {
            //Debug.Log("EXECUTING ATTACK ACTION");
            /*  WE HAVE TWO WAYS TO DEAL WITH THIS ACTION, DEPENDING IF AN TARGET OBJECT WAS GIVEN.
             *  1. TargetObj was given. Then we only have to attack it.
             *  2. No targetObj was given - use targetPos instead:
             *  2.1.    Attack hostile found.
             *  2.2.    Look for hostiles while it moves towards targetPos.
             */
            if (targetObj && targetObj.GetComponent<PlayerObject>())
            {
                return caster.MoveToAttack(targetObj as PlayerObject);
            }
            else
            {
                if (caster.attackObj)
                {
                    return caster.MoveToAttack(caster.attackObj);
                }
                else
                {
                    //TODO USE OTHER SORTING METHOD?
                    List<PlayerObject> possibleTargets = ActionTools.Targets_SortByDistance(caster.transform.position, caster.hostilePlayerObjects);
                    if (possibleTargets.Count > 0)
                    {
                        caster.attackObj = possibleTargets[0];
                    }
                }
            }
            return ActionTools.AdditionalMovement(this);
        }
        return false;
    }

    public override bool CheckActionComplete()
    {
        if (base.CheckActionComplete())
        {
            //As long as the targetObj is alive/exists, keep attacking it.
            if (targetObj)
                return targetObj.isDead;

            return caster.CheckDestinationReached(targetPos);
        }
        return false;
    }
}
