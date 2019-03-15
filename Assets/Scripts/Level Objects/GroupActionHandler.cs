using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class GroupActionHandler : MonoBehaviour
{
    [Header("Initial Parameters")]
    public List<PlayerObject> units;
    public List<Action> actions;

    [Header("Derived Parameters")]
    public PlayerObject groupLeader;
    public Vector3 destPos;
    public LevelObject destObj;

    [Header("Group Formation")]
    public List<Vector3> offsets;

    //public bool handlingStarted;
    public Vector3 groupPos = Vector3.zero;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CheckIfGroupIsNeeded();
        CalculateGroupPos();
	}

    private void CheckIfGroupIsNeeded()
    {
        //if (handlingStarted && 
        if (units.Count <= 0 || actions.Count <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void CalculateGroupPos()
    {
        foreach (PlayerObject unit in units)
            groupPos += unit.transform.position;
        groupPos /= units.Count;
    }

    public void ActionsForUnits(Vector3 targetPos, LevelObject targetObj)
    {
        if (units.Count <= 0 || actions.Count <= 0)
            return;

        groupLeader = units[0];
        destPos = targetPos;
        destObj = targetObj;

        //If the action requires an target object and not an specific position, then we can forget about the rest.
        if (destObj)
            return;

        //Calculate the offsets required, and add the destination position to them.
        //Then, rotate the offsets based on the direction the group will take at the end of the nav mesh path.
        NavMeshPath nmPath = new NavMeshPath();
        NavMesh.CalculatePath(groupLeader.transform.position, destPos, NavMesh.AllAreas, nmPath);
        Vector3 initialPos = nmPath.corners[nmPath.corners.Length - 2];
        Vector3 finalPos = nmPath.corners[nmPath.corners.Length - 1];
        float direction = -1 * Mathf.Atan2(finalPos.z - initialPos.z, finalPos.x - initialPos.x) * 180 / Mathf.PI;
        List<Vector3> formationOffsets = Zem.Formations.Formation.Box(units.Count, groupLeader.getSize());
        formationOffsets = Zem.Formations.Formation.ChangeOffsets(formationOffsets, destPos, direction);

        //Sorts all units and all offsets based on the distance to the destination position.
        units = units.OrderBy(x => Vector3.Distance(x.transform.position, destPos)).ToList();
        actions = actions.OrderBy(x => Vector3.Distance(x.caster.transform.position, destPos)).ToList();
        formationOffsets = formationOffsets.OrderBy(x => Vector3.Distance(x, destPos)).ToList();

        //Give actions to all units in the group, with the respective offsetted destination position applied.
        for (int i = 0; i < units.Count; i++)
        {
            actions[i].targetPos = formationOffsets[i];
            offsets.Add(formationOffsets[i]);
        }
    }
}
