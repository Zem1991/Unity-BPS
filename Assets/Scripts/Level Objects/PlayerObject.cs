using BPS;
using BPS.InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : LevelObject {
    [Header("Configuration Parameters")]
    public int hp_current;
    public int hp_maximum;
    public int mp_current;
    public int mp_maximum;
    public float atkDamage;
    public float atkRange;      //TODO NEEDS MINIMUM AND MAXIMUM ATTACK RANGES
    public float atkSpeed;
    public float defense;
    public float sightRange;
    public int moneyCost;
    public int moneyUpkeep;
    public float productionTime;
    public int relevancy;

    [Header("Actions")]
    //all actions that can be performed
    public Action[] allActions = new Action[Constants.MAX_ACTIONS];
    //actions queued to be performed
    public List<Action> actionList = new List<Action>();
    //current action being performed
    public Action currentAction;

    [Header("Unit Training")]
    //public List<Unit> trainableUnits;
    public List<PO_Unit> unitsInTraining;
    public float trainingTimeElapsed;

    [Header("Decision Making")]
    public float timeSinceLastDecision, timeBetweenDecisions;
    public CombatStance combatStance;
    public List<PlayerObject> allKnownPlayerObjects = new List<PlayerObject>();
    public List<PlayerObject> friendlyPlayerObjects = new List<PlayerObject>();
    public List<PlayerObject> hostilePlayerObjects = new List<PlayerObject>();
    public Vector3 attackPos, defendPos;
    public PlayerObject attackObj, defendObj;

    [Header("Execution Parameters")]
    public float atkWaitTime;

    protected override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        if (timeBetweenDecisions <= 0)
            timeBetweenDecisions = 0.1F;
        if (relevancy < 0)
            relevancy = 0;

        ApplyPlayerColor();

        //Quick fix if we have an incorrect amount of actions.
        if (allActions.Length != Constants.MAX_ACTIONS)
        {
            Action[] tempActions = new Action[Constants.MAX_ACTIONS];
            for (int i = 0; i < tempActions.Length; i++)
            {
                if (i < allActions.Length)
                    tempActions[i] = allActions[i];
            }
            allActions = tempActions;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        CheckDeath();

        if (ShouldMakeDecision())
        {
            UpdateKnownPlayerObjects();
            MakeDecision();
        }

        if (!currentAction && actionList.Count > 0)
        {
            currentAction = actionList[0];
            actionList.RemoveAt(0);

            //bool costsOk = currentAction.checkCosts();
            //bool targetTypeOk = currentAction.checkTargetType();
            //bool targetDiplomacyOk = currentAction.checkTargetDiplomacy();
            //bool targetOwnerOk = currentAction.checkTargetOwnerType();
            //if (costsOk && targetTypeOk && targetDiplomacyOk && targetOwnerOk)
            if (currentAction.VerifyActionUsage())
            {
                currentAction.ApplyActionCosts();
                currentAction.InitializeActionExecution();

                //TODO CLEAN AN "revisar" THIS

                //bool rangeOk = currentAction.checkRange();
                //if (rangeOk)
                //{
                //    currentAction.applyActionCosts();
                //    currentAction.executeAction();
                //}
            }
        }

        if (currentAction && currentAction.inExecution && currentAction.CheckActionComplete())
        {
            clearCurrentAction();
        }

        UpdateProduction();

        if (atkWaitTime > 0)
            atkWaitTime -= Time.deltaTime;
    }

    public override void DrawSelection(GameManager gm, ResourceManager rm)
    {
        base.DrawSelection(gm, rm);

        //if (!rallyPointFlag)
        //{
        //    rallyPointFlag = Instantiate(rm.rallyPointFlag);
        //    rallyPointFlag.transform.parent = rallyPoint.transform;
        //    rallyPointFlag.transform.position = rallyPoint.getPosition();
        //}
        //if (rallyPoint.transform.position != spawnPoint.transform.position)
        //    rallyPointFlag.show();
        //else
        //    rallyPointFlag.hide();
    }

    protected override void DrawSelectionBox(ResourceManager rm)
    {
        base.DrawSelectionBox(rm);

        GUIStyle hpBar = new GUIStyle();
        GUIStyle mpBar = new GUIStyle();
        hpBar.normal.background = rm.hpBar.texture;
        mpBar.normal.background = rm.mpBar.texture;

        GUI.Label(new Rect(selectionBox.x, selectionBox.y - 7, selectionBox.width * (hp_current / hp_maximum), 5), "", hpBar);
    }

    public override void showSelectionObjects(ResourceManager resourcesManager)
    {
        base.showSelectionObjects(resourcesManager);
    }

    public override void hideSelectionObjects(ResourceManager resourcesManager)
    {
        base.hideSelectionObjects(resourcesManager);
    }

    //THIS MAY BE USED LATER WHEN DEALING WITH MIND CONTROLLED UNTIS (TEMPORARY OWNERS)
    public Player GetOwnerOrController()
    {
        return owner;
    }

    public void SetOwner(Player p)
    {
        owner = p;
        ApplyPlayerColor();
    }

    public void ApplyPlayerColor()
    {
        foreach (LevelObject_Component locw in locws)
        {
            if (locw.applyPlayerColor && locw.GetComponent<Renderer>())
            {
                Material m = locw.GetComponent<Renderer>().material;
                m.SetColor("_Color", owner.playerColor);
            }
        }
    }

    public void CheckDeath()
    {
        if (isDead)
        {
            decayTime += Time.deltaTime;

            if (decayTime >= decayTimeMax)
                Destroy(gameObject);
        }

        if (hp_current <= 0)
        {
            //Debug.Log("CheckDeath() TRUE | PlayerObject + " + this);
            isDead = true;

            owner.pSelection.removeFromSelection(this, false);

            foreach (var item in PlayerManager.FindGroupActionHandlers())
            {
                item.units.Remove(this);
            }

            List<PlayerObject> allPOs = PlayerManager.FindPlayerObjects(this, transform.position, sightRange, false, false);
            foreach (var item in allPOs)
            {
                item.friendlyPlayerObjects.Remove(this);
                item.hostilePlayerObjects.Remove(this);
            }
        }
    }

    public void ChangeHP(int amount, bool recovering)
    {
        hp_current = Mathf.Clamp(hp_current + (recovering ? +amount : -amount), 0, hp_maximum);
    }

    public void ChangeMP(int amount, bool recovering)
    {
        mp_current = Mathf.Clamp(mp_current + (recovering ? +amount : -amount), 0, mp_maximum);
    }

    public void clearCurrentAction()
    {
        if (currentAction)
        {
            if (currentAction.group)
                currentAction.group.units.Remove(this);

            Destroy(currentAction.gameObject);
            currentAction = null;
        }
    }

    public void clearAllActions()
    {
        foreach (var action in actionList)
        {
            if (action.group)
                action.group.units.Remove(this);

            Destroy(action.gameObject);
        }
        clearCurrentAction();
    }

    private void UpdateProduction()
    {
        if (unitsInTraining.Count > 0)
        {
            if (trainingTimeElapsed >= unitsInTraining[0].productionTime)
            {
                PO_Unit u = Instantiate(unitsInTraining[0], spawnPoint.getPosition(), spawnPoint.getRotation(), owner.pBelongings.wrap_Units.transform);
                u.SetOwner(owner);
                u.destinationPos = rallyPoint.getPosition();
                unitsInTraining.RemoveAt(0);
                trainingTimeElapsed = 0;

                if (spawnPoint.transform.position != rallyPoint.transform.position)
                    u.InstantiateAction_Move(rallyPoint.transform.position, rallyPointOverObject);
            }
            else
            {
                trainingTimeElapsed += Time.deltaTime;
            }
        }
        else
        {
            trainingTimeElapsed = 0;
        }
    }

    /*
     *  !!  !!  !!  !!  !!  !!  !!  !!  !!  !!  !!  !!  !!  !!  !!  !!  !!  !!  !!  !!  
     *  DECISION MAKING STUFF
     */

    protected virtual bool ShouldMakeDecision()
    {
        //if (!attacking && !movingIntoPosition && !aiming)
        if (actionList.Count == 0 && !currentAction)
        {
            //We are not doing anything at the moment
            if (timeSinceLastDecision >= timeBetweenDecisions)
            {
                timeSinceLastDecision = 0.0f;
                return true;
            }
            timeSinceLastDecision += Time.deltaTime;
        }
        return false;
    }

    private void UpdateKnownPlayerObjects()
    {
        allKnownPlayerObjects = PlayerManager.FindPlayerObjects(this, transform.position, sightRange, false, true);

        friendlyPlayerObjects = new List<PlayerObject>();
        hostilePlayerObjects = new List<PlayerObject>();

        foreach (var item in allKnownPlayerObjects)
        {
            if (item.GetOwnerOrController() == GetOwnerOrController())
            {
                friendlyPlayerObjects.Add(item);
            }
            else
            {
                //(item.attackObj == this)  ??? 
                hostilePlayerObjects.Add(item);
            }
        }
    }

    private void WarnFriendlies_EnemySighted(PlayerObject enemy)
    {
        foreach (PlayerObject item in friendlyPlayerObjects)
            item.ReceiveWarning_EnemySighted(this, enemy);
    }

    private void WarnFriendlies_EnemyIsAttacking(PlayerObject enemy)
    {
        foreach (PlayerObject item in friendlyPlayerObjects)
            item.ReceiveWarning_EnemyIsAttacking(this, enemy);
    }

    private void WarnFriendlies_AttackingTheEnemy(PlayerObject enemy)
    {
        foreach (PlayerObject item in friendlyPlayerObjects)
            item.ReceiveWarning_AttackingTheEnemy(this, enemy);
    }

    public void ReceiveWarning_EnemySighted(PlayerObject source, PlayerObject enemy)
    {
        if ((combatStance == CombatStance.OFFENSIVE) && (attackObj == null))
            InstantiateAction_Attack(enemy.transform.position, enemy);
    }

    public void ReceiveWarning_EnemyIsAttacking(PlayerObject source, PlayerObject enemy)
    {
        if ((combatStance == CombatStance.DEFENSIVE) && (defendObj == source))
            InstantiateAction_Attack(enemy.transform.position, enemy);
        else if ((combatStance == CombatStance.OFFENSIVE) && (attackObj == null))
            InstantiateAction_Attack(enemy.transform.position, enemy);
    }

    public void ReceiveWarning_AttackingTheEnemy(PlayerObject source, PlayerObject enemy)
    {
        if ((combatStance == CombatStance.OFFENSIVE) && (attackObj == null))
            InstantiateAction_Attack(enemy.transform.position, enemy);
    }

    protected virtual void MakeDecision()
    {
        //Won't do anything if its currently in passive (noncombatant) stance.
        //(at least for now...)
        if (combatStance == CombatStance.NO_COMBAT)
            return;

        //if (!defendObj && friendlyPlayerObjects.Count > 0)
        //    defendObj = friendlyPlayerObjects[0];
        if (!attackObj && hostilePlayerObjects.Count > 0)
            attackObj = hostilePlayerObjects[0];

        switch (combatStance)
        {
            case CombatStance.DEFENSIVE:
                //InstantiateAction_Defend(defendPos, defendObj);
                break;
            case CombatStance.OFFENSIVE:
                InstantiateAction_Attack(attackPos, attackObj);
                break;
        }
    }
    private bool InstantiateAction_Move(Vector3 movePos, LevelObject moveObj)
    {
        Action action = null;
        if (allActions[(int)ActionCommandType.MOVE])
        {
            action = Instantiate(allActions[(int)ActionCommandType.MOVE]);
            action.SetSourceAndTarget(action, this, movePos, moveObj, null);
            action.transform.parent = transform;
        }
        if (action)
        {
            actionList.Add(action);
            return true;
        }
        return false;
    }


    //private bool InstantiateAction_Defend(Vector3 defendPos, LevelObject defendObj)
    //{
    //    Action action = null;
    //    if (allActions[(int)ActionCommandType.DEFEND])
    //    {
    //        action = Instantiate(allActions[(int)ActionCommandType.DEFEND]);
    //        action.SetSourceAndTarget(action, this, defendPos, defendObj, null);
    //        action.transform.parent = transform;
    //    }
    //    if (action)
    //    {
    //        actionList.Add(action);
    //        return true;
    //    }
    //    return false;
    //}

    private bool InstantiateAction_Attack(Vector3 attackPos, LevelObject attackObj)
    {
        Action action = null;
        if (allActions[(int)ActionCommandType.ATTACK])
        {
            action = Instantiate(allActions[(int)ActionCommandType.ATTACK]);
            action.SetSourceAndTarget(action, this, attackPos, attackObj, null);
            action.transform.parent = transform;
        }
        if (action)
        {
            actionList.Add(action);
            return true;
        }
        return false;
    }

    public bool MoveToAttack(PlayerObject target)
    {
        if (Vector3.Distance(transform.position, target.transform.position) > atkRange)
        {
            //TODO bool retreat should be TRUE when minimum attack range is not okay!
            UpdateDestinationPosition(target.transform.position, target, atkRange, false);
            return true;
        }
        else
        {
            return MakeAttack(target);
        }
    }

    public bool MakeAttack(PlayerObject target)
    {
        if (!target)
            return false;

        if (Vector3.Distance(transform.position, target.transform.position) > atkRange)
            return false;

        //TODO ACTUAL ATTACK WITH ANIMATIONS, PROJECTILES AND SHIT.
        WarnFriendlies_AttackingTheEnemy(target);

        if (atkWaitTime <= 0)
        {
            atkWaitTime = atkSpeed;
            target.ChangeHP(Mathf.RoundToInt(atkDamage), false);
            Debug.Log("an attack was made - pew! pew!");
        }
        return true;
    }
}
