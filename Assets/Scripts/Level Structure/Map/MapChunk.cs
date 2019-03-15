using BPS.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChunk : MonoBehaviour {
    public MapManager mapManager;

    [Header("Wrappers")]
    public GameObject wrap_GroundTiles;
    public GameObject wrap_WaterTiles;

    [Header("Neighbouring Map Chunks")]
    public MapChunk[] neighbourChunks = new MapChunk[Zem.Directions.DirectionsClass.MAX_DIRECTIONS];

    [Header("Belonging Tiles")]
    public Tile[,] groundTiles;
    public Tile[,] waterTiles;

    [Header("Other Stuff")]
    public float currentElevation;

    public TileChunkTypes tileChunkType;
    public bool raisedAsCliff;
    //public bool loweredAsFlood;

    // Use this for initialization
    void Start () {
        mapManager = GetComponentInParent<MapManager>();
    }
	
	// Update is called once per frame
	void Update () {
        currentElevation = getElevation();
    }

    public float getElevation()
    {
        if (groundTiles == null || groundTiles.Length <= 0)
        {
            return transform.position.y;
        }
        else
        {
            float elev = 0;
            foreach (var item in groundTiles)
                elev += item.getElevation();
            return elev / groundTiles.Length;
        }
    }

    public void SetupArrays(int chunkSizeX, int chunkSizeZ)
    {
        groundTiles = new Tile[chunkSizeX, chunkSizeZ];
        waterTiles = new Tile[chunkSizeX, chunkSizeZ];
    }
}
