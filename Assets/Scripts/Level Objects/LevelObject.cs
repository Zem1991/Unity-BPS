using BPS.InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelObject : MonoBehaviour {
    [Header("Identification stuff")]
    public Player owner;
    public string levelObjectName;
    public Sprite img_profile;
    public Sprite img_icon;
    public LevelObjectType levelObjectType;
    public PlacementType placementType;

    [Header("Special parameters")]
    public float decayTimeMax;

    [Header("Control Flags and Parameters")]
    public bool isInvincible;
    public bool isInvisible;
    public bool ignoreCliff;
    public bool ignoreWater;
    public bool isDead;
    public float decayTime;

    [Header("Components that make physically this object")]
    public List<LevelObject_Component> locws = new List<LevelObject_Component>();
    public Bounds selectionBounds;
    public Rect selectionBox;
    private List<Material> oldMaterials = new List<Material>();

    [Header("Spatial sutff [BUILDING]")]
    public int tilesGoingX, tilesGoingZ;
    public List<Tile> occupiedTiles = new List<Tile>();
    public Road frontRoad;
    public CityBlock cityBlock;
    public NavMeshObstacle nav_Obstacle;

    [Header("Locomotion sutff [UNIT]")]
    public float moveSpeed;
    public float angularSpeed;
    public float acceleration;
    public Vector3 destinationPos;
    public LevelObject destinationObj;
    public NavMeshAgent nav_Agent;

    [Header("Stuff used only by derived classes")]
    public LevelObject_Component spawnPoint;
    public LevelObject_Component rallyPoint;
    public LevelObject rallyPointOverObject;
    public RallyPointFlag rallyPointFlag;
    public List<Effect> activeEffects = new List<Effect>();

    protected virtual void Awake()
    {
        locws.AddRange(GetComponentsInChildren<LevelObject_Component>());

        nav_Agent = GetComponent<NavMeshAgent>();
        nav_Obstacle = GetComponent<NavMeshObstacle>();
        if (!nav_Agent && !nav_Obstacle)
            Debug.LogWarning("An " + levelObjectName + "(" + name + ") doesn't have either an NavMeshAgent or an NavMeshObstacle component!");
        if (nav_Agent && levelObjectType == LevelObjectType.BUILDING)
            Debug.LogWarning("An " + levelObjectName + "(" + name + ") is a BUILDING but has an NavMeshAgent component!");
        if (nav_Obstacle && levelObjectType == LevelObjectType.UNIT)
            Debug.LogWarning("An " + levelObjectName + "(" + name + ") is a UNIT but has an NavMeshObstacle component!");

        if (spawnPoint && rallyPoint)
        {
            rallyPoint.transform.position = spawnPoint.getPosition();
            rallyPoint.transform.rotation = spawnPoint.getRotation();
        }

        calculateSelectionBounds();

        //Object must still exist in the level for a brief time, so all ohter objects can register it is dead/destroyed.
        if (decayTimeMax < 0.1F)
            decayTimeMax = 0.1F;
    }

    // Use this for initialization
    protected virtual void Start ()
    {
        switch (levelObjectType)
        {
            case LevelObjectType.BUILDING:
                nav_Obstacle.size = Vector3.Scale(selectionBounds.size, (transform.localScale * -1));
                nav_Obstacle.carving = true;
                break;
            case LevelObjectType.UNIT:
                nav_Agent.speed = moveSpeed;
                nav_Agent.angularSpeed = angularSpeed;
                nav_Agent.acceleration = acceleration;
                UpdateDestinationPosition(transform.position, null);
                break;
        }
    }

    // Update is called once per frame
    protected virtual void Update ()
    {
        setSelectionBoundsCenter();

        switch (levelObjectType)
        {
            case LevelObjectType.BUILDING:
                //Something?
                break;
            case LevelObjectType.UNIT:
                UpdateDestinationPosition(destinationPos, destinationObj);
                break;
        }
    }

    public float getElevation()
    {
        return transform.position.y;
    }

    public float getSize()
    {
        //TODO REDO THIS?
        return selectionBounds.size.magnitude;
    }


    public virtual void DrawSelection(GameManager gm, ResourceManager rm)
    {
        GUI.skin = rm.selectionBoxSkin;
        selectionBox = BPSHelperFunctions.calculateSelectionBox(selectionBounds, gm.screenPlayingArea);

        //Draw the selection box around the currently selected object, within the bounds of the playing area
        GUI.BeginGroup(gm.screenPlayingArea);
        DrawSelectionBox(rm);
        GUI.EndGroup();
    }

    protected virtual void DrawSelectionBox(ResourceManager rm)
    {
        GUI.Box(selectionBox, "");
    }

    public virtual void showSelectionObjects(ResourceManager resourcesManager)
    {
        //TODO basic selection objects
    }

    public virtual void hideSelectionObjects(ResourceManager resourcesManager)
    {
        //TODO basic selection objects
    }

    public void applyMaterial(Material material, bool storeExistingMaterial)
    {
        if (storeExistingMaterial)
            oldMaterials.Clear();

        List<Renderer> renderers = new List<Renderer>();
        //renderers.Add(GetComponent<Renderer>());
        renderers.AddRange(GetComponentsInChildren<Renderer>());
        foreach (Renderer renderer in renderers)
        {
            if (storeExistingMaterial)
                oldMaterials.Add(renderer.material);
            renderer.material = material;
        }
    }

    public void restoreMaterials()
    {
        List<Renderer> renderers = new List<Renderer>();
        //renderers.Add(GetComponent<Renderer>());
        renderers.AddRange(GetComponentsInChildren<Renderer>());
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].material = oldMaterials[i];
        }
    }

    public void setColliders(bool enabled)
    {
        List<Collider> colliders = new List<Collider>();
        //colliders.Add(GetComponent<Collider>());
        colliders.AddRange(GetComponentsInChildren<Collider>());
        foreach (Collider collider in colliders)
        {
            collider.enabled = enabled;
            //collider.gameObject.SetActive(enabled);
        }
    }

    private void calculateSelectionBounds()
    {
        selectionBounds = new Bounds(transform.position, Vector3.zero);
        Renderer rSelf = GetComponent<Renderer>();
        if (rSelf)
        {
            selectionBounds.Encapsulate(rSelf.bounds);
        }
        foreach (Renderer rChild in GetComponentsInChildren<Renderer>())
        {
            selectionBounds.Encapsulate(rChild.bounds);
        }

        //Fix for the EXTENTS - subtract 0.01F so later we don't have problems when placing buildings and stuff.
        Vector3 newExtents = selectionBounds.extents;
        newExtents.x -= 0.01F;
        newExtents.z -= 0.01F;
        selectionBounds.extents = newExtents;

        //Fix for the CENTER - redefine the center's elevation (y coord).
        Vector3 newCenter = transform.position;
        newCenter.y = selectionBounds.center.y;
        selectionBounds.center = newCenter;
    }

    private void setSelectionBoundsCenter()
    {
        Vector3 newCenter = transform.position;
        newCenter.y = selectionBounds.center.y;
        selectionBounds.center = newCenter;
    }

    public void UpdateDestinationPosition(Vector3 destPos, LevelObject destObj)
    {
        destinationPos = destPos;
        destinationObj = destObj;
        if (destinationObj && destinationObj != this)
        {
            PO_Unit pUnit = destinationObj.GetComponent<PO_Unit>();
            PO_Building pBldg = destinationObj.GetComponent<PO_Building>();
            CityObject cBldg = destinationObj.GetComponent<CityObject>();
            if (!pUnit && !pBldg && !cBldg)
            {
                LevelObject lObj = destinationObj.GetComponent<LevelObject_Component>().getLevelObject();
                pUnit = lObj as PO_Unit;
                pBldg = lObj as PO_Building;
                cBldg = lObj as CityObject;
            }

            if (pUnit)
            {
                destinationPos = pUnit.transform.position;
                //destinationPos = pUnit.destinationPos;
            }
            else if (pBldg)
            {
                destinationPos = pBldg.spawnPoint.getPosition();
            }
            else if (cBldg)
            {
                destinationPos = cBldg.spawnPoint.getPosition();
            }
        }
        Vector3 v1 = transform.position;
        Vector3 v2 = destinationPos;
        v1.y = 0;
        v2.y = 0;
        if (v1 != v2)
        {
            nav_Agent.stoppingDistance = 0;
            nav_Agent.SetDestination(destinationPos);
        }
    }

    public void UpdateDestinationPosition(Vector3 destPos, LevelObject destObj, float distance, bool retreat)
    {
        destinationPos = destPos;
        destinationObj = destObj;
        if (destinationObj && destinationObj != this)
        {
            destinationPos = destinationObj.transform.position;
        }

        if (!retreat)
        {
            nav_Agent.stoppingDistance = distance;
        }
        else
        {
            nav_Agent.stoppingDistance = 0;
            Vector3 directionToRetreat = (destinationPos - transform.position).normalized;
            Vector3 newPos = destinationPos - (directionToRetreat * distance);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(newPos, out hit, 1, NavMesh.AllAreas))
            {
                destinationPos = hit.position;
            }
        }
        nav_Agent.SetDestination(destinationPos);
    }

    public bool CheckDestinationReached(Vector3 destPos)
    {
        Vector3 position, destination;
        position = transform.position;
        position.y = 0;     //We do this because of Unity's NavMesh
        destination = destPos;
        destination.y = 0;  //We do this because of Unity's NavMesh
        return (position == destination);
    }

    public Vector3 getVelocity()
    {
        return nav_Agent.velocity;
    }

    public void updateRallyPointPosition(Vector3 targetPos, LevelObject targetObj)
    {
        if (!targetObj)
        {
            rallyPoint.transform.position = targetPos;
        }
        else
        {
            rallyPoint.transform.position = targetObj.transform.position;
        }
    }

    public bool addActiveEffet(Effect e)
    {
        if (!e)
            return false;

        int aux = activeEffects.IndexOf(e);
        if (aux != -1)
        {
            activeEffects[aux].Increase();
        }
        else
        {
            Effect newE = Instantiate(e, transform);
            activeEffects.Add(newE);
            newE.Apply(this);
        }
        return true;
    }
}
