using BPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zem.Directions;

public class PlayerBelongings : MonoBehaviour {
    public Player player;

    [Header("Wrappers")]
    public GameObject wrap_Buildings;
    public GameObject wrap_Units;
    public GameObject wrap_Undesirables;

    [Header("Stuff")]
    public PO_Building bldg_PartyHeadquarters;
    public List<PO_Building> allBuildings = new List<PO_Building>();
    public List<PO_Unit> allUnits = new List<PO_Unit>();

    [Header("Construction Handling")]
    public PO_Building temporaryBuilding;
    public bool isFindingPlacement = false;
    public bool tempBldgRotationChanged = false;

    // Use this for initialization
    void Start () {
        player = GetComponentInParent<Player>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void createTemporaryBuilding(PO_Building building, Vector3 position, Quaternion rotation, Material constructionDenied)  //Rect playingArea  ???
    {
        temporaryBuilding = Instantiate(building, position, rotation);

        temporaryBuilding.SetOwner(player);
        temporaryBuilding.applyMaterial(constructionDenied, true);
        temporaryBuilding.setColliders(false);

        isFindingPlacement = true;
    }

    public void changeTemporaryBuildingPosition(Vector3 position, float snapToTileSize)
    {
        Vector3 newPos = position;
        if (snapToTileSize > 0)
        {
            newPos.x = BPSHelperFunctions.RoundToMultipleOf(newPos.x, snapToTileSize);
            newPos.y = Mathf.Round(newPos.y);
            newPos.z = BPSHelperFunctions.RoundToMultipleOf(newPos.z, snapToTileSize);
        }
        temporaryBuilding.transform.position = newPos;
    }

    public void changeTemporaryBuildingRotation(float direction, bool snapToGrid)
    {
        tempBldgRotationChanged = true;

        float newdir = direction;
        if (snapToGrid)
        {
            Directions cd = Directions.NO_DIRECTION;
            DirectionsClass.SnapToCardinalDireciton(direction, out newdir, out cd);
        }

        Quaternion newRot = new Quaternion();
        newRot.eulerAngles = new Vector3(0, newdir, 0);
        temporaryBuilding.transform.rotation = newRot;
    }

    public void changeTemporaryBuildingRotation(Vector3 torwardsPoint, bool snapToGrid)
    {
        Vector3 p1 = temporaryBuilding.transform.position;
        Vector3 p2 = torwardsPoint;
        float direction = Mathf.Atan2(p2.z - p1.z, p2.x - p1.x) * 180 / Mathf.PI;
        //Debug.Log("direction ANTES " + direction);

        //Here we should have the following values:
        // *    0           for EAST / RIGHT
        // *    90          for NORTH / UP
        // *    -180 or 180 for WEST / LEFT
        // *    -90 or 270  for SOUTH / DOWN
        //However, we need to translate that to the building rotation.
        direction += 90;
        direction *= -1;
        if (direction <= -180)
            direction += 360;

        //Debug.Log("direction DEPOIS = " + direction);
        changeTemporaryBuildingRotation(direction, snapToGrid);
    }

    public void startConstruction(List<Tile> tilesToBeUsed, Road roadToBeUsed)
    {
        isFindingPlacement = false;

        temporaryBuilding.setColliders(true);
        temporaryBuilding.restoreMaterials();
        temporaryBuilding.ApplyPlayerColor();
        //tempBuilding.StartConstruction(); //TODO here we make the building construct itself
        //RemoveResource(ResourceType.Money, tempBuilding.cost);    //TODO and here we pay the costs
    }

    public void constructionObstructed()
    {
        Debug.Log("CONSTRUCTION IS OBSTRUCTED!");
    }

    public void noRoadAvailable()
    {
        Debug.Log("NO ROAD AVAILABLE!");
    }

    public void cancelConstruction()
    {
        isFindingPlacement = false;
        Destroy(temporaryBuilding.gameObject);
    }
}
