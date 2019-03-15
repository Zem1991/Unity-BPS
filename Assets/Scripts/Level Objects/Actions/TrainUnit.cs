using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BPS.InGame;

public class TrainUnit : Action {
    public PO_Unit unitToTrain;

    protected override void Awake()
    {
        base.Awake();
        img_icon = unitToTrain.img_icon;
    }

    // Use this for initialization
    protected override void Start () {
        base.Start();
        castType = CastType.INSTANTANEOUS;
        rangeType = RangeType.SELF;
        targetType = TargetType.NO_TYPE;
        actionName = "Train: " + unitToTrain.levelObjectName;
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
    }

    public override bool InitializeActionExecution()
    {
        if (base.InitializeActionExecution())
        {
            PO_Building casterAsBuilding = caster as PO_Building;
            casterAsBuilding.unitsInTraining.Add(unitToTrain);
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool CheckActionComplete()
    {
        PO_Building casterAsBuilding = caster as PO_Building;
        return (casterAsBuilding.trainingTimeElapsed >= unitToTrain.productionTime);
    }
}
