using BPS;
using BPS.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public GameManager gameManager;

    [Header("Wrappers")]
    public GameObject wrap_Chunks;

    [Header("Constants")]
    public const int MIN_CHUNK_HEIGHT = -4;
    public const int MIN_TILE_HEIGHT = -2;
    public const int MAX_TILE_HEIGHT = 8;
    public const int MAIN_WATER_HEIGHT = 0;

    [Header("Initial Parameters: [1] Base Values")]
    public int mapSizeX;
    public int mapSizeZ;
    public int chunkSizeX;
    public int chunkSizeZ;
    public int initialElevation;
    public int tileSize;
    public float tileHeight;
    [Header("Initial Parameters: [2] Elevation")]
    public int elevBlobsToRaise;
    public int elevBlobsToLower;
    public int elevBlobInitialSpread;
    public int elevBlobSpreadDecay;
    public int cliffCoverage_Pct;
    [Header("Initial Parameters: [3] Water")]
    public int riversToCreate;
    public int maxRiverLength;
    public int maxRiverTiles;
    [Header("Initial Parameters: [4] Vegetation")]
    public int vegBlobsToCreate;
    public int vegBlobInitialSpread;
    public int vegBlobSpreadDecay;
    public int treeCoverage_Pct;

    [Header("Derived Parameters")]
    public int chunkCountX;
    public int chunkCountZ;
    public float cliffHeight;

    [Header("Current Status")]
    public bool baseMapCreated = false;

    [Header("Generated Stuff")]
    public MapChunk tcCenter_SW;
    public MapChunk tcCenter_NW;
    public MapChunk tcCenter_NE;
    public MapChunk tcCenter_SE;
    public MapChunk[,] mapChunks;
    public Tile[,] groundTiles;
    public Tile[,] waterTiles;

    // Use this for initialization
    void Start() {
        gameManager = FindObjectOfType<GameManager>();

        if (!CheckParameters())
            Debug.Log("Some Map Generation Parameters were wrong and were changed automatically to more acceptable values.");
    }

    // Update is called once per frame
    void Update() {

    }

    public bool CheckParameters()
    {
        //TODO add individual verifications for each parameter, so we know if something was wrongly set
        bool result = true;

        mapSizeX = Mathf.Clamp(mapSizeX, 20, 40);
        mapSizeZ = Mathf.Clamp(mapSizeZ, 20, 40);
        chunkSizeX = Mathf.Clamp(chunkSizeX, 5, 5);
        chunkSizeZ = Mathf.Clamp(chunkSizeZ, 5, 5);
        initialElevation = Mathf.Clamp(initialElevation, MIN_TILE_HEIGHT, MAX_TILE_HEIGHT);
        tileSize = Mathf.Clamp(tileSize, 5, 5);

        cliffHeight = tileSize / 2F;

        tileHeight = Mathf.Clamp(tileHeight, tileSize / 5F, cliffHeight);
        mapSizeX -= (mapSizeX % chunkSizeX);
        mapSizeZ -= (mapSizeZ % chunkSizeZ);

        chunkCountX = (mapSizeX / chunkSizeX);
        chunkCountZ = (mapSizeZ / chunkSizeZ);
        
        int tiles = mapSizeX * mapSizeZ;
        int heights = MIN_TILE_HEIGHT * (int)Mathf.Sign(MIN_TILE_HEIGHT) + MAX_TILE_HEIGHT * (int)Mathf.Sign(MAX_TILE_HEIGHT);

        elevBlobsToRaise = Mathf.Clamp(elevBlobsToRaise, 0, tiles * heights);
        elevBlobsToLower = Mathf.Clamp(elevBlobsToLower, 0, tiles * heights);
        elevBlobInitialSpread = Mathf.Clamp(elevBlobInitialSpread, 100, 100);
        elevBlobSpreadDecay = Mathf.Clamp(elevBlobSpreadDecay, 5, 100);
        cliffCoverage_Pct = Mathf.Clamp(cliffCoverage_Pct, 0, 100);

        riversToCreate = Mathf.Clamp(riversToCreate, 0, Mathf.Min(chunkSizeX, chunkSizeZ));
        maxRiverLength = Mathf.Clamp(maxRiverLength, 0, Mathf.Min(mapSizeX, mapSizeZ));
        maxRiverTiles = Mathf.Clamp(maxRiverTiles, 0, mapSizeX + mapSizeZ);

        vegBlobsToCreate = Mathf.Clamp(vegBlobsToCreate, 0, tiles);
        vegBlobInitialSpread = Mathf.Clamp(vegBlobInitialSpread, 100, 100);
        vegBlobSpreadDecay = Mathf.Clamp(vegBlobSpreadDecay, 5, 100);
        treeCoverage_Pct = Mathf.Clamp(cliffCoverage_Pct, 0, 100);

        return result;
    }

    public void CorrectEverything()
    {
        MapEditorTools.ValidateAllMapChunks(this);
        MapEditorTools.ValidateAllTiles(mapSizeX, mapSizeZ, groundTiles, waterTiles);
        MapEditorTools.DrawAllTiles(mapSizeX, mapSizeZ, groundTiles, waterTiles);
    }

    public void Step1_ApplyBaseValues()
    {
        MapManagerTools.S1_DeleteMapChunks(this);
        MapManagerTools.S1_DeleteTiles(this);

        MapManagerTools.S1_CreateMapChunks(this);
        MapManagerTools.S1_CreateTiles(this);

        MapManagerTools.S1_FindMapChunkNeighbours(this);
        MapManagerTools.S1_FindTileNeighbours(this);

        CorrectEverything();
        baseMapCreated = true;
    }

    public void Step2_ResetElevation()
    {
        MapManagerTools.S2_ResetTileElevations(this);

        CorrectEverything();
    }

    public void Step2_ApplyElevation()
    {
        MapManagerTools.S2_ResetTileElevations(this);

        MapManagerTools.S2_RandomizeTileElevations(this);
        //S2_GenerateCliffs();

        CorrectEverything();
    }

    public void Step3_ResetWater()
    {
        MapManagerTools.S2_RemoveCliffs(this);
        //S3_RemoveRivers();

        CorrectEverything();
    }

    public void Step3_ApplyWater()
    {
        MapManagerTools.S2_RemoveCliffs(this);
        //S3_RemoveRivers();

        MapManagerTools.S2_GenerateCliffs(this);
        //S3_GenerateRivers();

        CorrectEverything();
    }

    public void Step4_ResetVegetation()
    {
        MapManagerTools.S3_RemoveRivers(this);
        //S4_RemoveVegetation();

        CorrectEverything();
    }

    public void Step4_ApplyVegetation()
    {
        MapManagerTools.S3_RemoveRivers(this);
        //S4_RemoveVegetation();

        MapManagerTools.S3_GenerateRivers(this);
        //S4_GenerateVegetation();

        CorrectEverything();
    }

    public Vector3 GetCenterOfMap()
    {
        Vector3 result = tcCenter_SW.transform.position + tcCenter_NW.transform.position + tcCenter_NE.transform.position + tcCenter_SE.transform.position;
        result.y = tcCenter_SW.getElevation() + tcCenter_NW.getElevation() + tcCenter_NE.getElevation() + tcCenter_SE.getElevation();
        result /= 4F;
        return result;
    }

    public Texture2D GenerateMiniMap()
    {
        return null;//TODO FIX STUFF

        if (groundTiles == null || groundTiles.Length <= 0)
            return null;

        Texture2D texture = new Texture2D(mapSizeX, mapSizeZ);
        for (int row = 0; row < mapSizeZ; row++)
        {
            for (int col = 0; col < mapSizeX; col++)
            {
                Color color;
                if (waterTiles[col, row].mustDrawMeshes)
                    color = waterTiles[col, row].GetComponent<Renderer>().material.color;
                else
                    color = groundTiles[col, row].GetComponent<Renderer>().material.color;
                color.a = 1F;

                texture.SetPixel(col, row, color);
            }
        }
        texture.Apply();
        return texture;
    }
}
