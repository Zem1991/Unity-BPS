using BPS.InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defend : Action {
    protected override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    protected override void Start () {
        base.Start();
        actionCommandType = ActionCommandType.DEFEND;
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
            caster.defendPos = targetPos;
            caster.defendObj = targetObj as PlayerObject;
            return ActionTools.AdditionalMovement(this);
        }
        return false;
    }

    public override bool ExecuteAction()
    {
        if (base.ExecuteAction())
        {
            //Debug.Log("EXECUTING DEFEND ACTION");
            /*  WE HAVE TWO WAYS TO DEAL WITH THIS ACTION, DEPENDING IF AN TARGET OBJECT WAS GIVEN.
             *  1. TargetObj was given - deal with its hostiles:
             *  1.1.    Attack hostile found.
             *  1.2.    Look for hostiles (prioritize the ones given by targetObj).
             *  2. No targetObj was given - use targetPos instead:
             *  2.1.    Attack hostile found.
             *  2.2.    Look for hostiles while it moves towards targetPos.
             */
            if (targetObj && targetObj.GetComponent<PlayerObject>())
            {
                if (caster.attackObj)
                {
                    //Only case where the caster will move towards an enemy while DEFENDING something.
                    if ((targetObj as PlayerObject).hostilePlayerObjects.IndexOf(caster.attackObj) != -1)
                        return caster.MoveToAttack(caster.attackObj); 
                }
                AttackObj_FromTargetObj();
            }
            else
            {
                AttackObj_FromTargetPos();
            }

            if (caster.attackObj)
                AttackObj_FromAround();

            if (caster.MakeAttack(caster.attackObj))
                return true;
            else
                return ActionTools.AdditionalMovement(this);
        }
        return false;
    }

    public override bool CheckActionComplete()
    {
        if (base.CheckActionComplete())
        {
            //As long as the targetObj is alive/exists, keep defending it.
            if (targetObj)
                return targetObj.isDead;

            /*  However, if no targetObj was given, the caster is expected to HOLD POSITION over its targetPos.
             *  Thats why we don't report the action as complete, or even check if the destination was reached.
             */ //return caster.CheckDestinationReached(targetPos);
        }
        return false;
    }

    private void AttackObj_FromAround()
    {
        //TODO USE OTHER SORTING METHOD?
        List<PlayerObject> possibleTargets = ActionTools.Targets_SortByDistance(caster.transform.position, caster.hostilePlayerObjects);
        if (possibleTargets.Count > 0)
            caster.attackObj = possibleTargets[0];
    }

    private void AttackObj_FromTargetPos()
    {
        //Attempts to change caster.attackObj to one of the hostiles from targetPos.
        //Said targetPos must be within sight range of the caster and actually have hostiles.
        float distance = Vector3.Distance(caster.transform.position, targetPos);
        if (distance <= caster.sightRange)
        {
            //TODO USE OTHER SORTING METHOD?
            List<PlayerObject> possibleTargets = ActionTools.Targets_SortByDistance(targetPos, caster.hostilePlayerObjects);
            if (possibleTargets.Count > 0)
                caster.attackObj = possibleTargets[0];
        }
    }

    private void AttackObj_FromTargetObj()
    {
        //Attempts to change caster.attackObj to one of the hostiles from targetObj.
        //Said targetObj must be within sight range of the caster and actually have hostiles.
        float distance = Vector3.Distance(caster.transform.position, targetObj.transform.position);
        if (distance <= caster.sightRange)
        {
            //TODO USE OTHER SORTING METHOD?
            List<PlayerObject> possibleTargets = ActionTools.Targets_SortByDistance(targetObj.transform.position, (targetObj as PlayerObject).hostilePlayerObjects);
            if (possibleTargets.Count > 0)
                caster.attackObj = possibleTargets[0];
        }
    }
}
