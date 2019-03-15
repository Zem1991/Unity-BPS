using BPS;
using BPS.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zem.Directions;

public class GameManager : MonoBehaviour {
    [Header("Constants")]
    public const float MIN_GAMESPEED = 0.25F;
    public const float MAX_GAMESPEED = 4;
    public const float DEFAULT_GAMESPEED = 1;
    public const int MIN_PLAYERS = 2;
    public const int MAX_PLAYERS = 12;
    public const int MIN_CITYBLOCKS_PER_PLAYER = 4;
    public const int MAX_CITYBLOCKS_PER_PLAYER = 6;

    [Header("Initial Parameters")]
    public InGameUseMode useMode;
    public int numberOfPlayers = MIN_PLAYERS;
    public float gameSpeed = DEFAULT_GAMESPEED;
    public int matchLength_firstRound = 1;

    [Header("Stuff in Objects")]
    public LocalNavMeshBuilder localNavBldgr;
    public Camera theCamera;
    public VoteCounter voteCounter;

    [Header("Derived Parameters: Loading Status")]
    public bool step1_LoadInitialObjects = false;
    public bool step2_AdjustObjects = false;
    public bool fullInitializationDone = false;

    [Header("Derived Parameters: Execution Status")]
    public CameraHolder currentCameraHolder;
    public bool playTesting = false;
    public bool gamePaused = false;
    public float actualGameSpeed;
    public MatchStatus matchStatus = MatchStatus.NO_MATCH;
    TimeSpan remainingTime;
    public DateTime dtStart = new DateTime();
    public DateTime dtFirstTurn = new DateTime();
    public DateTime dtFirstTurnLimit = new DateTime();

    [Header("Derived Parameters: Match Timers")]
    public string sTimer_firstRound;

    [Header("Derived Parameters: Game Results")]
    public int[] playerVotes;
    public float[] playerPcts;
    public int maxVotes;

    [Header("Management")]
    public ResourceManager resourceManager;
    public MapManager mapManager;
    public CityManager cityManager;
    public PopulationManager populationManager;
    public ScreenManager screenManager;
    public InputManager inputManager;
    public PlayerManager playerManager;
    public FeedbackManager feedbackManager;

    public Bounds levelPlayingArea; //The physical extents of the level (map + city + population).
    public Rect screenPlayingArea;  //Where in the screen is actual game objects being shown (Screen - HUD = playingArea).
    public Quaternion defaultRotation = new Quaternion();

    // Use this for initialization
    void Start() {
        gameSpeed = DEFAULT_GAMESPEED;
        actualGameSpeed = gameSpeed;

        localNavBldgr = GetComponent<LocalNavMeshBuilder>();
        theCamera = GetComponentInChildren<Camera>();
        voteCounter = GetComponentInChildren<VoteCounter>();

        resourceManager = FindObjectOfType<ResourceManager>();
        mapManager = FindObjectOfType<MapManager>();
        cityManager = FindObjectOfType<CityManager>();
        populationManager = FindObjectOfType<PopulationManager>();
        screenManager = FindObjectOfType<ScreenManager>();
        inputManager = FindObjectOfType<InputManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        feedbackManager = FindObjectOfType<FeedbackManager>();

        if (!CheckParameters())
            Debug.Log("Some Map Generation Parameters were wrong and were changed automatically to more acceptable values.");
    }
	
	// Update is called once per frame
	void Update () {
        if (!fullInitializationDone)
        {
            if (!step1_LoadInitialObjects)
            {
                Initialization_Step1_LoadInitialObjects();
            }
            else if (!step2_AdjustObjects)
            {
                Initialization_Step2_AdjustObjects();
            }

            if (step1_LoadInitialObjects && step2_AdjustObjects)
            {
                fullInitializationDone = true;
            }
        }

        Time.timeScale = actualGameSpeed;
        UpdateScreenPlayingArea();

        if (fullInitializationDone && useMode == InGameUseMode.PLAY)
            CheckGameResults();
    }

    void FixedUpdate()
    {
        FU_MatchStatus();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(levelPlayingArea.center, levelPlayingArea.size);
    }

    public bool CheckParameters()
    {
        //TODO add individual verifications for each parameter, so we know if something was wrongly set
        bool result = true;

        numberOfPlayers = Mathf.Clamp(numberOfPlayers, MIN_PLAYERS, MAX_PLAYERS);

        defaultRotation.eulerAngles = new Vector3(0, 0, 0);

        return result;
    }

    public ResourceManager ResourceManager()
    {
        if (!resourceManager)
            resourceManager = FindObjectOfType<ResourceManager>();
        return resourceManager;
    }

    public MapManager MapManager()
    {
        if (!mapManager)
            mapManager = FindObjectOfType<MapManager>();
        return mapManager;
    }

    public CityManager CityManager()
    {
        if (!cityManager)
            cityManager = FindObjectOfType<CityManager>();
        return cityManager;
    }

    public PopulationManager PopulationManager()
    {
        if (!populationManager)
            populationManager = FindObjectOfType<PopulationManager>();
        return populationManager;
    }

    public ScreenManager ScreenManager()
    {
        if (!screenManager)
            screenManager = FindObjectOfType<ScreenManager>();
        return screenManager;
    }

    public InputManager InputManager()
    {
        if (!inputManager)
            inputManager = FindObjectOfType<InputManager>();
        return inputManager;
    }

    public PlayerManager PlayerManager()
    {
        if (!playerManager)
            playerManager = FindObjectOfType<PlayerManager>();
        return playerManager;
    }

    public FeedbackManager FeedbackManager()
    {
        if (!feedbackManager)
            feedbackManager = FindObjectOfType<FeedbackManager>();
        return feedbackManager;
    }

    public bool ChangeUseMode(InGameUseMode newUM, bool inPlaytest)
    {
        if (useMode == newUM)
            return false;

        bool result = true;

        switch (useMode)
        {
            case InGameUseMode.EDITOR:
                if (newUM == InGameUseMode.PLAY)
                {
                    if (mapManager.baseMapCreated &&
                        cityManager.baseCityCreated &&
                        populationManager.zoningAndFeaturesCreated &&
                        populationManager.playerStartCreated)
                    {
                        SpawnPlayers();
                        playTesting = inPlaytest;
                    }
                    else
                    {
                        result = false;
                    }
                }
                break;
            case InGameUseMode.PLAY:
                if (newUM == InGameUseMode.EDITOR)
                {
                    if (playTesting)
                    {
                        playTesting = false;
                    }
                    else
                    {
                        result = false;
                    }
                }
                break;
            default:
                result = false;
                break;
        }

        if (result)
        {
            step1_LoadInitialObjects = false;
            step2_AdjustObjects = false;
            fullInitializationDone = false;
            useMode = newUM;
        }
        return result;
    }

    public bool PAUSE_GAME()
    {
        if (!gamePaused)
        {
            Time.timeScale = 0;
            gamePaused = true;
            return true;
        }
        return false;
    }

    public bool RESUME_GAME()
    {
        if (gamePaused)
        {
            Time.timeScale = gameSpeed;
            gamePaused = false;
            return true;
        }
        return false;
    }

    public void QUIT_GAME()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }

    public bool pointInsideLevelBounds(Vector3 point)
    {
        return levelPlayingArea.Contains(point);
    }

    public bool canPlaceBuilding(PO_Building building, Directions facingDirection, 
        out bool placementOK, out bool roadAvailable, out List<Tile> tilesFound, out Road roadFound)
    {
        placementOK = true;
        roadAvailable = false;
        tilesFound = new List<Tile>();
        roadFound = null;

        //We can only place new buildings inside the city bounds.
        if (!pointInsideLevelBounds(building.transform.position))
            return false;

        //Check if within the desired position there is any collider that counts as an obstruction.
        //And while doing so, identify all Tiles that are going to be used and if those are all elegible.
        Bounds placeBounds = building.selectionBounds;
        List<Collider> colliders = new List<Collider>();
        colliders.AddRange(Physics.OverlapBox(placeBounds.center, placeBounds.extents, building.transform.rotation));
        foreach (Collider col in colliders)
        {
            if (!col.CompareTag("Ground"))
            {
                placementOK = false;
                break;
            }

            Tile t = col.gameObject.GetComponent<Tile>();
            if (t)
            {
                if (t.getElevation() != building.getElevation() || 
                    (t.tileType != TileTypes.GROUND && t.tileType != TileTypes.CLIFF_GROUND))
                {
                    placementOK = false;
                    break;
                }

                if (tilesFound.IndexOf(t) == -1)
                    tilesFound.Add(t);
            }
        }

        //With the tiles identified, we now check if those tiles are available and if those have an Road adjacent
        //(based on the facing direction).
        if (placementOK)
        {
            Tile otherT;
            foreach (Tile currentT in tilesFound)
            {
                if (facingDirection == Directions.E)
                {
                    otherT = currentT.groundNeighbours[(int)Directions.E];
                    if (otherT && otherT.tileType == TileTypes.ROAD)
                        roadFound = otherT.road_SouthToNorth;
                }
                if (facingDirection == Directions.N)
                {
                    otherT = currentT.groundNeighbours[(int)Directions.N];
                    if (otherT && otherT.tileType == TileTypes.ROAD)
                        roadFound = otherT.road_WestToEast;
                }
                if (facingDirection == Directions.W)
                {
                    otherT = currentT.groundNeighbours[(int)Directions.W];
                    if (otherT && otherT.tileType == TileTypes.ROAD)
                        roadFound = otherT.road_SouthToNorth;
                }
                if (facingDirection == Directions.S)
                {
                    otherT = currentT.groundNeighbours[(int)Directions.S];
                    if (otherT && otherT.tileType == TileTypes.ROAD)
                        roadFound = otherT.road_WestToEast;
                }

                if (roadFound)
                {
                    roadAvailable = true;
                    break;
                }
            }
        }

        return true;
    }

    private void Initialization_Step1_LoadInitialObjects()
    {
        SpawnLevel();
        UpdateLevelPlayingArea();

        ScreenManager().ChangeScreenMode(useMode);
        //InputManager().ChangeMode(useMode);

        switch (useMode)
        {
            case InGameUseMode.EDITOR:
                break;
            case InGameUseMode.PLAY:
                matchStatus = MatchStatus.INITIALIZING;
                SpawnPlayers();
                break;
            default:
                break;
        }

        step1_LoadInitialObjects = true;
    }

    private void Initialization_Step2_AdjustObjects()
    {
        //UpdateScreenPlayingArea();
        ReallocateTheCamera();

        switch (useMode)
        {
            case InGameUseMode.EDITOR:
                RemovePlayersAndTheirStuff();
                break;
            case InGameUseMode.PLAY:
                GivePlayersInitialStuff();
                voteCounter.GeneratePlayerIntentions(playerManager.allPlayers);
                StartTimers();
                matchStatus = MatchStatus.FIRST_ROUND;
                break;
            default:
                break;
        }

        step2_AdjustObjects = true;
    }

    private void SpawnLevel()
    {
        if (!MapManager().baseMapCreated)
            MapManager().Step1_ApplyBaseValues();

        if (useMode == InGameUseMode.PLAY)
        {
            if (!cityManager.baseCityCreated)
                cityManager.Step1_ApplyBaseValues();
            if (!populationManager.zoningAndFeaturesCreated)
                populationManager.Step1_ApplyZoningAndFeatures();
            if (!populationManager.playerStartCreated)
                populationManager.Step2_ApplyPlayerStart();
        }
    }

    private void SpawnPlayers()
    {
        if (useMode != InGameUseMode.PLAY)
            return;

        PlayerManager pm = PlayerManager();
        List<LevelObject> startingLocations = new List<LevelObject>(populationManager.playerStartingLocations);

        for (int i = 0; i < numberOfPlayers; i++)
        {
            LevelObject startingLocation = startingLocations[UnityEngine.Random.Range(0, startingLocations.Count)];
            startingLocations.Remove(startingLocation);

            Player p = Instantiate(resourceManager.pref_player, startingLocation.transform.position, defaultRotation, pm.transform);
            p.playerName = "Player " + ((i + 1) <= 9 ? "0" : "") + (i + 1);
            p.faction = pm.defaultFaction;
            p.startingLocation = startingLocation;

            if (pm.useBasicColorSchemes)
                p.playerColor = pm.basicColorSchemes[i].color;

            pm.allPlayers.Add(p);
        }
        pm.localPlayer = pm.allPlayers[0];  //TODO NOT ALWAYS THE LOCAL PLAYER WILL BE THE FIRST ONE LISTED!
    }

    private void UpdateLevelPlayingArea()
    {
        //No actual need to check the bounds of everything within CityManager and PopulationManager.
        levelPlayingArea = new Bounds();
        Vector3 lpaExtents = new Vector3();
        lpaExtents.x = mapManager.mapSizeX * mapManager.tileSize / 2;
        lpaExtents.z = mapManager.mapSizeZ * mapManager.tileSize / 2;
        lpaExtents.y = mapManager.tileSize * 10;

        levelPlayingArea.center = mapManager.transform.position;
        levelPlayingArea.extents = lpaExtents;

        localNavBldgr.m_Size = levelPlayingArea.size;
    }

    private void UpdateScreenPlayingArea()
    {
        screenPlayingArea = new Rect(0, 0, Screen.width, Screen.height);
    }

    private void ReallocateTheCamera()
    {
        CameraHolder holder;

        if (useMode == InGameUseMode.PLAY && playerManager.localPlayer)
            holder = playerManager.localPlayer.pCamera.cameraHolder;
        else
            holder = inputManager.localCamera.cameraHolder;

        if (holder)
        {
            theCamera.transform.parent = holder.transform;
            theCamera.transform.position = holder.transform.position;
            theCamera.transform.rotation = holder.transform.rotation;
            currentCameraHolder = holder;
        }
    }

    private void RemovePlayersAndTheirStuff()
    {
        //TODO REMOVE EVERYTHING PLAYER RELATED WHEN SWITCHING BACK TO EDITOR MODE
        //PROPERLY
        foreach (Player p in playerManager.allPlayers)
        {
            foreach (PO_Building b in p.pBelongings.allBuildings)
            {
                //TODO THIIIIS
            }

            Destroy(p.gameObject);
        }
        playerManager.allPlayers.Clear();
    }

    private void GivePlayersInitialStuff()
    {
        PlayerManager pm = PlayerManager();

        foreach (var pl in pm.allPlayers)
        {
            LevelObject sl = pl.startingLocation;
            sl.gameObject.SetActive(false);

            PO_Building phq = pl.faction.bldg_basic_hq;

            PlayerBelongings pb = pl.pBelongings;
            pb.bldg_PartyHeadquarters = Instantiate(phq, sl.transform.position, sl.transform.rotation, pb.wrap_Buildings.transform);
            pb.bldg_PartyHeadquarters.owner = pl;
            pb.allBuildings.Add(pb.bldg_PartyHeadquarters);
        }
    }

    private void StartTimers()
    {
        dtStart = DateTime.UtcNow;
        dtFirstTurnLimit = dtFirstTurnLimit.AddMinutes(matchLength_firstRound);
    }

    private void FU_MatchStatus()
    {
        if (matchStatus == MatchStatus.FIRST_ROUND)
        {
            dtFirstTurn = dtFirstTurn.AddSeconds(Time.deltaTime);
            remainingTime = dtFirstTurnLimit - dtFirstTurn;
            sTimer_firstRound = "[FIRST ROUND] " + remainingTime.Minutes + ":" + remainingTime.Seconds;

            if (remainingTime.TotalSeconds <= 0)
                matchStatus = MatchStatus.FIRST_RESULTS;
        }
    }

    private void CheckGameResults()
    {
        //TODO CHANGE THE AMOUNT OF VOTES PER RESIDENCE ?
        List<LevelObject> allVoters = new List<LevelObject>(populationManager.co_residences);
        playerVotes = new int[numberOfPlayers];;

        foreach (var item in allVoters)
        {
            int playerId = (item as CityObject).selectCandidateFromVotingIntetions();
            if (playerId >= 0)
                playerVotes[playerId]++;
        }

        voteCounter.UpdatePlayerIntentions(playerVotes, allVoters.Count);

        if (matchStatus == MatchStatus.FIRST_RESULTS)
        {
            List<PlayerIntentions> winners = new List<PlayerIntentions>(voteCounter.SortVotesPerPlayer());

            Debug.Log("PLAYER " + winners[0].player.playerName + " WON THE ELECTION!");
        }
    }
}
