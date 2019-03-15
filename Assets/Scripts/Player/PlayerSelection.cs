using BPS.InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MonoBehaviour {
    public Player player;
    public List<PlayerObject> selectedPlayerObjects = new List<PlayerObject>();
    public List<PlayerObject> relevantSelectedPOs = new List<PlayerObject>();
    public CityObject selectedCityObject;

    [Header("Quick Selections")]
    public List<PlayerObject> qs01 = new List<PlayerObject>();
    public List<PlayerObject> qs02 = new List<PlayerObject>();
    public List<PlayerObject> qs03 = new List<PlayerObject>();
    public List<PlayerObject> qs04 = new List<PlayerObject>();
    public List<PlayerObject> qs05 = new List<PlayerObject>();
    public List<PlayerObject> qs06 = new List<PlayerObject>();
    public List<PlayerObject> qs07 = new List<PlayerObject>();
    public List<PlayerObject> qs08 = new List<PlayerObject>();
    public List<PlayerObject> qs09 = new List<PlayerObject>();
    public List<PlayerObject> qs10 = new List<PlayerObject>();

    // Use this for initialization
    void Start () {
        player = GetComponentInParent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
        //TODO allow that the player changes the relevant subselection usint TAB and SHIFT+TAB like in Starcraft2
        relevantSelectedPOs = BPSHelperFunctions.SelectionRelevantSubgroup(selectedPlayerObjects);
    }

    public void clearSelection()
    {
        //foreach (PlayerObject po in selectedPlayerObjects)
        //{
        //    po.hideSelectionObjects(player.resourceManager);
        //}
        selectedPlayerObjects.Clear();
        selectedCityObject = null;
    }

    public void changeSelection(PlayerObject obj, bool allEquals)
    {
        if (!allEquals)
        {
            clearSelection();
            selectedPlayerObjects.Add(obj);
        }
        else
        {
            foreach(var item in selectedPlayerObjects)
            {
                if (!obj.Equals(item))
                {
                    //item.hideSelectionObjects(player.resourceManager);
                    selectedPlayerObjects.Remove(item);
                }
            }
        }

        //SortPlayerObjectsByRelevancy(selectedPlayerObjects);  //NOT REQUIRED
    }

    public void changeSelection(List<PlayerObject> selectedObjects)
    {
        clearSelection();
        selectedPlayerObjects = selectedObjects;

        SortPlayerObjectsByRelevancy();
    }

    public void changeSelection(CityObject selectedObject)
    {
        clearSelection();
        selectedCityObject = selectedObject;
    }

    public void addToSelection(PlayerObject obj, bool removeIfAlreadySelected)
    {
        //Will only add IF the object is not already in the selection.
        //ELSE, will remove the object from selection if that was desired.
        if (selectedPlayerObjects.IndexOf(obj) == -1)
        {
            //obj.showSelectionObjects(player.resourceManager);
            selectedPlayerObjects.Add(obj);
        }
        else
        {
            if (removeIfAlreadySelected)
            {
                //obj.hideSelectionObjects(player.resourceManager);
                selectedPlayerObjects.Remove(obj);
            }
        }
    }

    public void addToSelection(List<PlayerObject> selectedObjects, bool removeIfAlreadySelected)
    {
        foreach (var item in selectedObjects)
        {
            addToSelection(item, removeIfAlreadySelected);
        }

        SortPlayerObjectsByRelevancy();
    }

    public void removeFromSelection(PlayerObject obj, bool allEquals)
    {
        if (!allEquals)
        {
            //obj.hideSelectionObjects(player.resourceManager);
            selectedPlayerObjects.Remove(obj);
        }
        else
        {
            foreach (var item in selectedPlayerObjects)
            {
                if (obj.Equals(item))
                {
                    //item.hideSelectionObjects(player.resourceManager);
                    selectedPlayerObjects.Remove(item);
                }
            }
        }
    }

    public void SortPlayerObjectsByRelevancy()
    {
        selectedPlayerObjects.Sort(delegate (PlayerObject x, PlayerObject y)
        {
            if (x.relevancy > y.relevancy) return -1;
            else if (x.relevancy < y.relevancy) return 1;
            else return 0;
        });
    }

    public void DrawSelection(GameManager gm, ResourceManager rm)
    {
        foreach (PlayerObject obj in selectedPlayerObjects)
        {
            obj.DrawSelection(gm, rm);
        }
        if (selectedCityObject)
        {
            selectedCityObject.DrawSelection(gm, rm);
        }
    }

    public Action FindAction(PlayerObject po, int actionIndex)
    {
        return po.allActions[actionIndex];
    }

    public bool AddNextAtcion(PlayerObject po, Action a, Vector3 targetPos, LevelObject targetObj, GroupActionHandler gah, bool addToQueue)
    {
        foreach (var item in po.allActions)
        {
            if (item && item.Equals(a))
            {
                if (!addToQueue)
                    po.clearAllActions();

                Action action = Instantiate(item);
                action.SetSourceAndTarget(a, po, targetPos, targetObj, gah);
                po.actionList.Add(action);

                if (gah)    // && po.levelObjectType == LevelObjectType.UNIT)
                {
                    gah.units.Add(po);
                    gah.actions.Add(action);
                }

                return true;
            }
        }
        return false;
    }

    public bool SetNextAction(Vector3 targetPos, LevelObject targetObj, bool addToQueue)
    {
        int actionsToGive = selectedPlayerObjects.Count;

        //  FIRST, check if we have units and/or buildings in the current selection.
        List<PlayerObject> pSelBuildings = new List<PlayerObject>();
        List<PlayerObject> pSelUnits = new List<PlayerObject>();
        List<PlayerObject> pSelOther = new List<PlayerObject>();
        foreach (var item in selectedPlayerObjects)
        {
            if (item.levelObjectType == LevelObjectType.BUILDING)
                pSelBuildings.Add(item);
            else if (item.levelObjectType == LevelObjectType.UNIT)
                pSelUnits.Add(item);
            else
                pSelOther.Add(item);
        }

        //  THEN, we read what is the current target.
        PlayerObject pObj = targetObj as PlayerObject;
        bool enemyPObj = (pObj && pObj.GetOwnerOrController() != player);

        //  FINALLY, we give actions accordingly to what is currently selected and the current target.
        //      Enemy PlayerObjects are to be ATTACKED
        //      OR
        //      Units move to the target position   /   Buildings relocate their rally point to the target position
        GroupActionHandler gah = Instantiate(player.resourceManager.pref_groupActionHandler);
        //gah.handlingStarted = true;
        if (enemyPObj)
        {
            foreach (var item in pSelBuildings)
            {
                Action action = FindAction(item, (int)ActionCommandType.ATTACK);
                if (action)
                {
                    AddNextAtcion(item, action, targetPos, targetObj, null, addToQueue);
                    actionsToGive--;
                }
            }
            foreach (var item in pSelUnits)
            {
                Action action = FindAction(item, (int)ActionCommandType.ATTACK);
                if (action)
                {
                    AddNextAtcion(item, action, targetPos, targetObj, gah, addToQueue);
                    actionsToGive--;
                }
            }
            
        }
        else
        {
            foreach (var item in pSelBuildings)
            {
                Action action = FindAction(item, (int)ActionCommandType.RALLY_POINT);
                if (action)
                {
                    AddNextAtcion(item, action, targetPos, targetObj, null, addToQueue);
                    actionsToGive--;
                }
            }
            foreach (var item in pSelUnits)
            {
                Action action = FindAction(item, (int)ActionCommandType.MOVE);
                if (action)
                {
                    AddNextAtcion(item, action, targetPos, targetObj, gah, addToQueue);
                    actionsToGive--;
                }
            }
        }
        gah.ActionsForUnits(targetPos, targetObj);

        if (pSelOther.Count > 0)
            Debug.LogWarning("pSelOther.Count > 0 ! No actions were given to some Player Objects!");

        return (actionsToGive == 0);
    }

    public bool SetNextAction(List<PlayerObject> playerObjects, Action action, Vector3 targetPos, LevelObject targetObj, bool addToQueue)
    {
        int actionsToGive = playerObjects.Count;

        //  FIRST, check if we have units and/or buildings in the current selection.
        List<PlayerObject> pSelBuildings = new List<PlayerObject>();
        List<PlayerObject> pSelUnits = new List<PlayerObject>();
        List<PlayerObject> pSelOther = new List<PlayerObject>();
        foreach (var item in playerObjects)
        {
            if (item.levelObjectType == LevelObjectType.BUILDING)
                pSelBuildings.Add(item);
            else if (item.levelObjectType == LevelObjectType.UNIT)
                pSelUnits.Add(item);
            else
                pSelOther.Add(item);
        }

        //  THEN, we give actions accordingly.
        GroupActionHandler gah = Instantiate(player.resourceManager.pref_groupActionHandler);
        //gah.handlingStarted = true;
        foreach (var item in pSelBuildings)
        {
            if (AddNextAtcion(item, action, targetPos, targetObj, null, addToQueue))
                actionsToGive--;
        }
        foreach (var item in pSelUnits)
        {
            if (AddNextAtcion(item, action, targetPos, targetObj, gah, addToQueue))
                actionsToGive--;
        }
        gah.ActionsForUnits(targetPos, targetObj);

        if (pSelOther.Count > 0)
            Debug.LogWarning("pSelOther.Count > 0 ! No actions were given to some Player Objects!");

        return (actionsToGive == 0);
    }
}
