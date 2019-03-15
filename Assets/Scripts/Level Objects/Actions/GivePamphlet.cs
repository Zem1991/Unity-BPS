using BPS.InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePamphlet : Action {
    public float baseEffect;

    [Header("Effects")]
    public Effect effect;

    protected override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    protected override void Start () {
        base.Start();
        actionCommandType = ActionCommandType.ABILITY;
        castType = CastType.TARGETABLE;
        rangeType = RangeType.GLOBAL;
        targetType = TargetType.BUILDING;
        targetDiplomacy = TargetDiplomacy.NO_DIPLOMACY;
        targetOwner = TargetOwner.THE_CITY;
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
            return true;
        }
        return false;
    }

    public override bool CheckActionComplete()
    {
        if (base.CheckActionComplete())
        {
            if (caster.CheckDestinationReached(targetPos))
            {
                CityObject res = targetObj.GetComponent<CityObject>();
                if (!res)
                {
                    LevelObject lo = targetObj.GetComponent<LevelObject_Component>().getLevelObject();
                    res = lo as CityObject;
                }
                if (res)
                {
                    Debug.Log(res);
                    Player p = caster.GetOwnerOrController();

                    float comparisonModifier = BPSHelperFunctions.PoliticalPositionComparison(
                        p.faction.politicalPosition,
                        res.politicalPosition);

                    float stackModifier = 1F;
                    int aux = targetObj.activeEffects.IndexOf(effect);
                    if (aux != -1)
                        stackModifier += targetObj.activeEffects[aux].stack;

                    res.setRating(p, res.getRating(p) + (baseEffect * comparisonModifier / stackModifier));
                    res.addActiveEffet(effect);
                    return true;
                }
            }
        }
        return false;
    }
}
