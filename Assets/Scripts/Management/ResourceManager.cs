using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    public GameManager gameManager;

    [Header("GUI - Button Graphics")]
    public Sprite icon_NoIcon;
    public Sprite icon_Highlighted;

    [Header("HUD - Selection Box")]
    public Sprite hpBar;
    public Sprite mpBar;
    public GUISkin selectionBoxSkin;
    public Texture2D selectionBoxTexture;
    public Color selectionBoxFillingColor;
    public Color selectionBoxBorderColor;
    public int selectionBoxBorderThickness;

    [Header("HUD - Cursors")]
    public GUISkin mouseCursorSkin;
    public Texture2D[] cursorsDefault;
    public Texture2D[] cursorsPanUp;
    public Texture2D[] cursorsPanLeft;
    public Texture2D[] cursorsPanDown;
    public Texture2D[] cursorsPanRight;
    public Texture2D[] cursorsSelect;
    public Texture2D[] cursorsMove;
    public Texture2D[] cursorsDefend;
    public Texture2D[] cursorsAttack;
    public Texture2D[] cursorsRallyPoint;
    public Texture2D[] cursorsAbility;

    [Header("PLAY - Construction Materials")]
    public Material mat_Construction_Allowed;
    public Material mat_Construction_Denied;

    [Header("PLAY - Visual Objects")]
    public RallyPointFlag rallyPointFlag;

    [Header("LOGIC - Logic Objects")]
    public Player pref_player;
    public GroupActionHandler pref_groupActionHandler;

    [Header("Map Materials")]
    public Material mat_Tile_Ground;
    public Material mat_Tile_CliffGround;
    public Material mat_CliffSide;
    public Material mat_Water;
    public Material mat_Road;
    public Material mat_Test;

    [Header("MapManager Prefabs")]
    public MapChunk pref_Chunk;
    public Tile pref_GroundTile;
    public Tile pref_WaterTile;
    public TileSide pref_CliffSide;

    [Header("CityManager Prefabs")]
    public Road pref_Road;
    public CityBlock pref_CityBlock;
    public GameObject pref_Ramp;
    public GameObject pref_Bridge;

    [Header("PopulationManager Prefabs")]
    //All valid special buildings
    public LevelObject pref_TownHall;
    public LevelObject pref_Hospital;
    public LevelObject pref_PoliceStation;
    public LevelObject pref_Church;
    public LevelObject pref_School;
    public LevelObject pref_SoccerStadium;
    //All valid residential buildings
    public List<LevelObject> prefs_ResPoor = new List<LevelObject>();
    public List<LevelObject> prefs_ResMedium = new List<LevelObject>();
    public List<LevelObject> prefs_ResRich = new List<LevelObject>();
    //All valid commercial buildings
    public List<LevelObject> prefs_ComPoor = new List<LevelObject>();
    public List<LevelObject> prefs_ComMedium = new List<LevelObject>();
    public List<LevelObject> prefs_ComRich = new List<LevelObject>();
    //All valid industrial buildings
    public List<LevelObject> prefs_IndPoor = new List<LevelObject>();
    public List<LevelObject> prefs_IndMedium = new List<LevelObject>();
    public List<LevelObject> prefs_IndRich = new List<LevelObject>();

    [Header("Player Prefabs")]
    public LevelObject pref_PlayerStartLocation;

    // Use this for initialization
    void Start () {
        gameManager = FindObjectOfType<GameManager>();

        if (!selectionBoxTexture)
        {
            selectionBoxTexture = new Texture2D(1, 1);
            selectionBoxTexture.SetPixel(0, 0, Color.white);
            selectionBoxTexture.Apply();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameManager getGameManager()
    {
        return gameManager;
    }
}
