using BPS.InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRallyPoint : Action {
    protected override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    protected override void Start () {
        base.Start();
        actionCommandType = ActionCommandType.RALLY_POINT;
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
            //TODO ADD CHECK FOR ACTUAL EXISTANCE OF TRAINABLE UNITS
            if (true)
            {
                caster.updateRallyPointPosition(targetPos, targetObj);
                return true;
            }
            else
            {
                SendErrorMessage(FeedbackManager.NO_RALLY_POINT);
            }
        }
        return false;
    }

    public override bool ExecuteAction()
    {
        if (base.ExecuteAction())
        {
            return true;
        }
        return false;
    }

    public override bool CheckActionComplete()
    {
        if (base.CheckActionComplete())
        {
            Vector3 position, destination;
            position = targetPos;
            position.y = 0;     //We do this because of Unity's NavMesh
            destination = caster.rallyPoint.transform.position;
            destination.y = 0;  //We do this because of Unity's NavMesh
            if (position == destination)
            {
                return true;
            }
        }
        return false;
    }
}
