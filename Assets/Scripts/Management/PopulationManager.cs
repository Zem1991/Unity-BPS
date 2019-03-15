using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Wrappers")]
    public GameObject wrap_SpecialFeatures;
    public GameObject wrap_Residences;
    public GameObject wrap_Commerces;
    public GameObject wrap_Industries;

    [Header("Initial Parameters: [1a] Purpose Zoning")]
    public int zonedBlocks_Pct;
    public int landUsage_Pct;
    public int residential_Pct;
    public int commercial_Pct;
    public int industrial_Pct;
    [Header("Initial Parameters: [1b] Economic Zoning")]
    public int ecoPoor_Pct;
    public int ecoMedium_Pct;
    public int ecoRich_Pct;
    [Header("Initial Parameters: [1c] Special Features")]
    public int health_Pct;
    public int crime_Pct;
    public int religion_Pct;
    public int education_Pct;
    public int soccerFanaticism_Pct;
    public int politicalIdeology_Pct;
    [Header("Initial Parameters: [2] Player Start")]
    public int numberOfPlayers;
    public int minDistance;

    [Header("Derived Parameters")]
    public int suggestedMinPlayers;
    public int suggestedMaxPlayers;
    public int possibleSpecialFeatures;
    public int hospitalsToCreate;
    public int policeStationsToCreate;
    public int churchesToCreate;
    public int schoolsToCreate;
    public int soccerStadiumsToCreate;

    [Header("Current Status")]
    public bool zoningAndFeaturesCreated = false;
    public bool playerStartCreated = false;

    [Header("Generated Stuff: Zonings")]
    public List<CityBlock> cb_notZoned = new List<CityBlock>();
    public List<CityBlock> cb_zoned = new List<CityBlock>();
    public List<CityBlock> cb_allResidential = new List<CityBlock>();
    public List<CityBlock> cb_allCommercial = new List<CityBlock>();
    public List<CityBlock> cb_allIndustrial = new List<CityBlock>();
    public List<CityBlock> cb_resPoor = new List<CityBlock>();
    public List<CityBlock> cb_resMedium = new List<CityBlock>();
    public List<CityBlock> cb_resRich = new List<CityBlock>();
    public List<CityBlock> cb_comPoor = new List<CityBlock>();
    public List<CityBlock> cb_comMedium = new List<CityBlock>();
    public List<CityBlock> cb_comRich = new List<CityBlock>();
    public List<CityBlock> cb_indPoor = new List<CityBlock>();
    public List<CityBlock> cb_indMedium = new List<CityBlock>();
    public List<CityBlock> cb_indRich = new List<CityBlock>();

    [Header("Generated Stuff: Features")]
    public List<LevelObject> co_specialFeatures = new List<LevelObject>();
    public List<LevelObject> co_residences = new List<LevelObject>();
    public List<LevelObject> co_commerces = new List<LevelObject>();
    public List<LevelObject> co_industries = new List<LevelObject>();

    [Header("Generated Stuff: Player Start Locations")]
    public List<LevelObject> playerStartingLocations = new List<LevelObject>();

    // Use this for initialization
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (!CheckParameters())
            Debug.Log("Some Population Generation Parameters were wrong and were changed automatically to more acceptable values.");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CheckParameters()
    {
        MapManager mm = gameManager.MapManager();
        CityManager cm = gameManager.CityManager();

        //TODO add individual verifications for each parameter, so we know if something was wrongly set
        bool result = true;

        zonedBlocks_Pct = Mathf.Clamp(zonedBlocks_Pct, 20, 80);
        landUsage_Pct = Mathf.Clamp(zonedBlocks_Pct, 50, 80);
        int[] rciPcts =
        {
            residential_Pct = Mathf.Clamp(residential_Pct, 0, 100),
            commercial_Pct = Mathf.Clamp(commercial_Pct, 0, 100),
            industrial_Pct = Mathf.Clamp(industrial_Pct, 0, 100)
        };
        int[] rciPctsFixed;
        if (BPSHelperFunctions.combinePercentiles(rciPcts, out rciPctsFixed))
        {
            residential_Pct = rciPctsFixed[0];
            commercial_Pct = rciPctsFixed[1];
            industrial_Pct = rciPctsFixed[2];
        }

        int[] ecoPcts =
        {
            ecoPoor_Pct = Mathf.Clamp(ecoPoor_Pct, 0, 100),
            ecoMedium_Pct = Mathf.Clamp(ecoMedium_Pct, 0, 100),
            ecoRich_Pct = Mathf.Clamp(ecoRich_Pct, 0, 100)
        };
        int[] ecoPctsFixed;
        if (BPSHelperFunctions.combinePercentiles(ecoPcts, out ecoPctsFixed))
        {
            ecoPoor_Pct = ecoPctsFixed[0];
            ecoMedium_Pct = ecoPctsFixed[1];
            ecoRich_Pct = ecoPctsFixed[2];
        }

        health_Pct = Mathf.Clamp(health_Pct, 0, 100);
        crime_Pct = Mathf.Clamp(crime_Pct, 0, 100);
        religion_Pct = Mathf.Clamp(religion_Pct, 0, 100);
        education_Pct = Mathf.Clamp(education_Pct, 0, 100);
        soccerFanaticism_Pct = Mathf.Clamp(soccerFanaticism_Pct, 0, 100);
        politicalIdeology_Pct = Mathf.Clamp(politicalIdeology_Pct, 0, 100);

        suggestedMinPlayers = cm.cb_isSquare.Count / GameManager.MAX_CITYBLOCKS_PER_PLAYER;
        suggestedMaxPlayers = cm.cb_isSquare.Count / GameManager.MIN_CITYBLOCKS_PER_PLAYER;
        suggestedMinPlayers = Mathf.Clamp(suggestedMinPlayers, GameManager.MIN_PLAYERS, GameManager.MAX_PLAYERS);
        suggestedMaxPlayers = Mathf.Clamp(suggestedMaxPlayers, GameManager.MIN_PLAYERS, GameManager.MAX_PLAYERS);

        numberOfPlayers = Mathf.Clamp(numberOfPlayers, suggestedMinPlayers, suggestedMaxPlayers);
        minDistance = Mathf.Clamp(minDistance, 0, 10);

        possibleSpecialFeatures = cm.cb_isSquare.Count - GameManager.MAX_PLAYERS;
        int auxSpecialFeatures = possibleSpecialFeatures / 5;

        hospitalsToCreate = Mathf.RoundToInt(auxSpecialFeatures * health_Pct / 100);
        policeStationsToCreate = Mathf.RoundToInt(auxSpecialFeatures * crime_Pct / 100);
        churchesToCreate = Mathf.RoundToInt(auxSpecialFeatures * religion_Pct / 100);
        schoolsToCreate = Mathf.RoundToInt(auxSpecialFeatures * education_Pct / 100);
        soccerStadiumsToCreate = Mathf.RoundToInt(auxSpecialFeatures * soccerFanaticism_Pct / 100);

        hospitalsToCreate = Mathf.Clamp(hospitalsToCreate, 1, auxSpecialFeatures);
        policeStationsToCreate = Mathf.Clamp(policeStationsToCreate, 1, auxSpecialFeatures);
        churchesToCreate = Mathf.Clamp(churchesToCreate, 1, auxSpecialFeatures);
        schoolsToCreate = Mathf.Clamp(schoolsToCreate, 1, auxSpecialFeatures);
        soccerStadiumsToCreate = Mathf.Clamp(soccerStadiumsToCreate, 0, 4);

        return result;
    }

    public void Step1_ResetZoningAndFeatures()
    {
        PopulationManagerTools.S1A_ClearPurposeZoning(this);
        PopulationManagerTools.S1B_ClearEconomicZoning(this);
        PopulationManagerTools.S1A_ClearCityBlocksUsage(this);

        PopulationManagerTools.S1C_DeleteSpecialFeatures(this);
        PopulationManagerTools.S1D_DeleteCommonFeatures(this);

        gameManager.MapManager().CorrectEverything();
        zoningAndFeaturesCreated = false;
    }

    public void Step1_ApplyZoningAndFeatures()
    {
        PopulationManagerTools.S1A_ClearPurposeZoning(this);
        PopulationManagerTools.S1B_ClearEconomicZoning(this);
        PopulationManagerTools.S1A_ClearCityBlocksUsage(this);

        PopulationManagerTools.S1C_DeleteSpecialFeatures(this);
        PopulationManagerTools.S1D_DeleteCommonFeatures(this);

        PopulationManagerTools.S1A_DefineCityBlocksUsage(this);
        PopulationManagerTools.S1A_DefinePurposeZoning(this);
        PopulationManagerTools.S1B_DefineEconomicZoning(this);

        PopulationManagerTools.S1C_CreateSpecialFeatures(this);
        PopulationManagerTools.S1D_CreateCommonFeatures(this);

        gameManager.MapManager().CorrectEverything();
        zoningAndFeaturesCreated = true;
    }

    public void Step2_ResetPlayerStart()
    {
        PopulationManagerTools.S2_ResetPlayerStart(this);

        gameManager.MapManager().CorrectEverything();
        playerStartCreated = false;
    }

    public void Step2_ApplyPlayerStart()
    {
        PopulationManagerTools.S2_ResetPlayerStart(this);
        PopulationManagerTools.S2_ApplyPlayerStart(this);

        gameManager.MapManager().CorrectEverything();
        playerStartCreated = true;
    }
}
