using BPS.InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stop : Action{
    protected override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    protected override void Start () {
        base.Start();
        actionCommandType = ActionCommandType.STOP;
        castType = CastType.INSTANTANEOUS;
        rangeType = RangeType.SELF;
        targetType = TargetType.NO_TYPE;
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
            caster.UpdateDestinationPosition(caster.transform.position, null);
            //TODO Insert here code to actually stop everything else other than moving.
            return true;
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
            //TODO Insert here code to actually check if everything has stopped.
            return true;
        }
        return false;
    }
}
