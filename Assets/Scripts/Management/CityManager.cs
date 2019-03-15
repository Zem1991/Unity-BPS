using BPS;
using BPS.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour {
    public GameManager gameManager;

    [Header("Wrappers")]
    public GameObject wrap_Roads;
    public GameObject wrap_CityBlocks;
    public GameObject wrap_Ramps;
    public GameObject wrap_Bridges;

    [Header("Initial Parameters: [1] Base Values")]
    public string cityName;
    public int roadsOffset;
    public int cityBlockSize;
    [Header("Initial Parameters: [2] Accessibility")]
    public int rampsCoverage_Pct;
    public int bridgesCoverage_Pct;

    //[Header("Derived Parameters")]

    [Header("Current Status")]
    public bool baseCityCreated = false;

    [Header("Generated Stuff")]
    public LevelObject theTownHall;
    public List<Road> roads = new List<Road>();
    public List<CityBlock> cityBlocks = new List<CityBlock>();
    public List<GameObject> ramps = new List<GameObject>();
    public List<GameObject> bridges = new List<GameObject>();

    [Header("City Block sizes and shapes")]
    public List<CityBlock> cb_isSquare = new List<CityBlock>();
    public List<CityBlock> cb_isFlat = new List<CityBlock>();

    // Use this for initialization
    void Start() {
        gameManager = FindObjectOfType<GameManager>();

        if (!CheckParameters())
            Debug.Log("Some City Generation Parameters were wrong and were changed automatically to more acceptable values.");
    }

    // Update is called once per frame
    void Update() {

    }

    public bool CheckParameters()
    {
        //TODO add individual verifications for each parameter, so we know if something was wrongly set
        bool result = true;

        if (cityName == null || cityName.Length <= 0)
            cityName = "The Town With No Name";
        roadsOffset = Mathf.Clamp(roadsOffset, 2, 2);
        cityBlockSize = Mathf.Clamp(cityBlockSize, 4, 4);

        rampsCoverage_Pct = Mathf.Clamp(rampsCoverage_Pct, 0, 100);
        bridgesCoverage_Pct = Mathf.Clamp(bridgesCoverage_Pct, 0, 100);

        return result;
    }

    public void Step1_ResetBaseValues()
    {
        CityManagerTools.S1_DeleteTownHall(this);
        CityManagerTools.S1_DeleteRoads(this);

        gameManager.MapManager().CorrectEverything();

        CityManagerTools.S1_DeleteCityBlocks(this);

        baseCityCreated = false;
    }

    public void Step1_ApplyBaseValues()
    {
        Step1_ResetBaseValues();

        CityManagerTools.S1_PrepareGroundForTownHall(this);
        CityManagerTools.S1_CreateRoads(this);

        gameManager.MapManager().CorrectEverything();

        CityManagerTools.S1_CreateCityBlocks(this);
        CityManagerTools.S1_IdentifyCityBlocksSizeAndShapes(this);

        CityManagerTools.S1_CreateTownHall(this);

        baseCityCreated = true;
    }

    public void Step2_ResetAccessibility()
    {
        Debug.Log("FEATURE NOT YET IMPLEMETED!");
    }

    public void Step2_ApplyAccessibility()
    {
        Debug.Log("FEATURE NOT YET IMPLEMETED!");
    }
}
