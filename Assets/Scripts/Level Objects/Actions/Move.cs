using BPS.InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Action {
    protected override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    protected override void Start () {
        base.Start();
        actionCommandType = ActionCommandType.MOVE;
        castType = CastType.TARGETABLE;
        rangeType = RangeType.GLOBAL;
        targetType = TargetType.POINT;
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
            return ActionTools.AdditionalMovement(this);
        }
        return false;
    }

    public override bool ExecuteAction()
    {
        if (base.ExecuteAction())
        {
            //Debug.Log("EXECUTING MOVE ACTION");
            return true;
        }
        return false;
    }

    public override bool CheckActionComplete()
    {
        if (base.CheckActionComplete())
        {
            return caster.CheckDestinationReached(targetPos);
            //Vector3 position, destination;
            //position = caster.transform.position;
            //position.y = 0;     //We do this because of Unity's NavMesh
            //destination = caster.destinationPos;
            //destination.y = 0;  //We do this because of Unity's NavMesh
            //if (position == destination)
            //{
            //    return true;
            //}
        }
        return false;
    }
}
