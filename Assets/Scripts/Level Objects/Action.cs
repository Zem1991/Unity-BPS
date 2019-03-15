using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BPS.InGame;
using BPS.InGame.Error;

public class Action : MonoBehaviour {
    [Header("Identification")]
    public string actionName;
    public Sprite img_icon;
    public int preferedIndex;                           //Where is this action button placed in the GUI?
    public ActionCommandType actionCommandType;

    [Header("Action Parameters")]
    public CastType castType;
    public RangeType rangeType;
    public TargetType targetType;
    public TargetDiplomacy targetDiplomacy;
    public TargetOwner targetOwner;

    [Header("Costs and Limitations")]
    public float cooldown_current;      //How much we have to wait betore (re)using it?
    public float cooldown_maximum;
    public int moneyCost;               //What does it cost to use this action?
    public int hpCost;
    public int mpCost;
    public float minCastRange;          //How close and how far we can use it?
    public float maxCastRange;
    public float charge_current;        //How much we have to wait for channeled actions (spell charnging, unit in training)?
    public float charge_maximum;
    public float aoeRadius;             //How far the area of effect reaches and how strong is the effect at the extremities?
    public float aoeDispersion;

    [Header("Source and Target")]
    public Action callingAction;
    public PlayerObject caster;
    public Vector3 targetPos;
    public LevelObject targetObj;
    public GroupActionHandler group;

    [Header("Execution Parameters")]
    public bool inExecution = false;
    public FeedbackManager fm;

    protected virtual void Awake()
    {

    }

    // Use this for initialization
    protected virtual void Start () {
        
    }

    // Update is called once per frame
    protected virtual void Update () {
        ExecuteAction();
	}

    public virtual void SetSourceAndTarget(Action callingAction, PlayerObject caster, Vector3 targetPos, LevelObject targetObj, GroupActionHandler group)
    {
        this.callingAction = callingAction;
        this.caster = caster;
        this.targetPos = targetPos;
        this.targetObj = targetObj;
        this.group = group;
    }

    public bool VerifyActionUsage()
    {
        ActionCostError costsError;
        ActionRangeTypeError rangeTypeError;
        ActionTargetTypeError targetTypeError;
        ActionTargetDiplomacyError targetDiploError;
        ActionTargetOwnerError targetOwnerError;

        bool costs = ActionVerifier.CheckCosts(this, out costsError);
        bool range = ActionVerifier.CheckRangeType(this, out rangeTypeError);
        bool tType = ActionVerifier.CheckTargetType(this, out targetTypeError);
        bool tDiplo = ActionVerifier.CheckTargetDiplomacy(this, out targetDiploError);
        bool tOwner = ActionVerifier.CheckTargetOwner(this, out targetOwnerError);

        if (costs && range && tType && tDiplo && tOwner)
        {
            return true;
        }
        else
        {
            string errorMsg = "";
            FeedbackManagerTools.GetActionErrors(this,
                costsError, rangeTypeError, targetTypeError, targetDiploError, targetOwnerError,
                out errorMsg);
            SendErrorMessage(errorMsg);
            return false;
        }
    }

    public virtual void ApplyActionCosts()
    {
        callingAction.cooldown_current = callingAction.cooldown_maximum;
        caster.GetOwnerOrController().money -= moneyCost;
        caster.ChangeHP(hpCost, false);
        caster.ChangeMP(mpCost, false);
    }

    public virtual bool InitializeActionExecution()
    {
        inExecution = true;
        //Specific details of whatever an action should before its execution are placed on the overridden functions.
        return true;
    }

    public virtual bool ExecuteAction()
    {
        //Specific details of whatever an action should do during its execution are placed on the overridden functions.
        //Debug.Log("ACTION IN EXECUTION: + " + actionName);
        return inExecution;
    }

    public virtual bool CheckActionComplete()
    {
        //Specific details of how an action is considered complete are placed on the overridden functions.
        return true;
    }

    public bool SendErrorMessage(string errorMsg)
    {
        if (!fm)
            fm = FindObjectOfType<FeedbackManager>();

        if (fm)
        {
            fm.ContextMsg_ActionError(errorMsg);
            return true;
        }
        else
        {
            Debug.LogWarning("Action generated an error but no FeedbackManager was found to report it to the user.");
            return false;
        }
    }
}
