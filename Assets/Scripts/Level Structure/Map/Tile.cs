using BPS;
using BPS.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zem.Directions;

public class Tile : MonoBehaviour {
    public MapManager mapManager;

    public Vector2 tileId;
    public TileTypes tileType;

    public bool isWaterTile;
    public bool isCliff;
    public bool isRiver;
    public bool isRoad;

    public bool mustDrawMeshes = true;
    public Vector3[] meshVertices;
    public int[] meshTriangles;
    public Vector2[] meshUVs;
    public Mesh mesh;
    public MeshFilter meshFilter;

    public bool forcedFlatness;
    public float directionOfMiddleVertex;       //Will be either 0, +tileHeight or -tileHeight.
    public float heightOfLowestVertex;          //Used to draw water over this tile if its low enough.
    //public bool isCorneredAgainstCliffTiles;

    public MapChunk mapChunk;
    public Tile[] groundNeighbours = new Tile[DirectionsClass.MAX_DIRECTIONS];
    public Tile[] waterNeighbours = new Tile[DirectionsClass.MAX_DIRECTIONS];
    public Tile groundTile;
    public Tile waterTile;
    public List<TileSide> tileSides;

    public Road road_SingleTile;
    public Road road_SouthToNorth;
    public Road road_WestToEast;
    public CityBlock cityBlock;

    public LevelObject mapFeature;
    public LevelObject building;
    public float artificialElevation;

    // Use this for initialization
    void Start()
    {
        mapManager = GetComponentInParent<MapManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float getElevation()
    {
        return transform.position.y;
    }

    public void changeElevation(float height, bool forceFlatness)
    {
        if (height == 0)
            return;

        Vector3 newPos = transform.position;
        newPos.y += height;
        transform.position = newPos;

        forcedFlatness = forceFlatness;

        if (!forcedFlatness && 
            (directionOfMiddleVertex == 0 || Mathf.Sign(directionOfMiddleVertex) == Mathf.Sign(height)))
            directionOfMiddleVertex = height;
        else
            directionOfMiddleVertex = 0;
    }

    public void SetTileTypeAndMaterial(TileTypes tileType, Material m)
    {
        this.tileType = tileType;
        GetComponent<Renderer>().material = m;
    }

    public void DefineMeshParameters(float tileSize, float tileHeight, float cliffHeight)
    {
        MapStructureTools.Tile_DefineMeshVertices(this, tileSize, tileHeight, cliffHeight);
        MapStructureTools.Tile_DefineMeshTriangles(this);
        MapStructureTools.Tile_DefineMeshUVs(this);
    }

    public void DrawMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (!mustDrawMeshes)
        {
            if (meshFilter.mesh)
                Destroy(meshFilter.mesh);
            return;
        }
        
        if (meshFilter.mesh)
        {
            mesh = meshFilter.mesh;
        }
        else
        {
            mesh = new Mesh();
            meshFilter.mesh = mesh;
        }

        mesh.Clear();
        mesh.vertices = meshVertices;
        mesh.triangles = meshTriangles;
        mesh.uv = meshUVs;
        //mesh.Optimize();
        //UnityEditor.MeshUtility.Optimize(mesh);
        mesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void GenerateTileSides(TileSide pref_CliffSide, Material mat_CliffSide, float lowestHeightPossible)
    {
        if (tileSides != null)
        {
            foreach (var item in tileSides)
                if (item != null)
                    Destroy(item.gameObject);
        }
        tileSides = new List<TileSide>();

        Tile groundNeighbour, waterNeighbour;
        
        //TileSide facing South
        groundNeighbour = groundNeighbours[(int)Directions.S];
        waterNeighbour = waterNeighbours[(int)Directions.S];
        ValidateNeighboursTileSides(groundNeighbour, waterNeighbour, 5, 7, 3, 1, pref_CliffSide, mat_CliffSide, lowestHeightPossible);

        //TileSide facing West
        groundNeighbour = groundNeighbours[(int)Directions.W];
        waterNeighbour = waterNeighbours[(int)Directions.W];
        ValidateNeighboursTileSides(groundNeighbour, waterNeighbour, 3, 5, 1, 7, pref_CliffSide, mat_CliffSide, lowestHeightPossible);

        //TileSide facing North
        groundNeighbour = groundNeighbours[(int)Directions.N];
        waterNeighbour = waterNeighbours[(int)Directions.N];
        ValidateNeighboursTileSides(groundNeighbour, waterNeighbour, 1, 3, 7, 5, pref_CliffSide, mat_CliffSide, lowestHeightPossible);

        //TileSide facing East
        groundNeighbour = groundNeighbours[(int)Directions.E];
        waterNeighbour = waterNeighbours[(int)Directions.E];
        ValidateNeighboursTileSides(groundNeighbour, waterNeighbour, 7, 1, 5, 3, pref_CliffSide, mat_CliffSide, lowestHeightPossible);
    }

    public void DrawTileSidesMeshes()
    {
        if (!mustDrawMeshes)
            return;

        foreach (var item in tileSides)
        {
            item.DrawMesh();
        }
    }

    private bool ValidateNeighboursTileSides(Tile groundNeighbour, Tile waterNeighbour, 
        int vertexA, int vertexB, int neighbourVertexA, int neighbourVertexB,
        TileSide pref_CliffSide, Material mat_CliffSide, float lowestHeightPossible)
    {
        Vector3 topA, topB, bottomA, bottomB;

        /*  THERE ARE THREE CASES WHERE TILE SIDES MUST BE MADE:
         * 1.   GroundTile/GroundTile (lowest == difference between ground tiles)
         * 2.   GroundTile/Void (lowest == lowest height possible)
         * 3.   WaterTile/Void (lowest == difference between water tile and ground tile)
         */

        if (!isWaterTile)
        {
            if (groundNeighbour)
            {
                if (MapStructureTools.TileSide_IdentifyVertexes(
                    meshVertices[vertexA], meshVertices[vertexB], getElevation(),
                    groundNeighbour.meshVertices[neighbourVertexA], groundNeighbour.meshVertices[neighbourVertexB], groundNeighbour.getElevation(),
                    out topA, out topB, out bottomA, out bottomB))
                {
                    GenerateTileSide(pref_CliffSide, mat_CliffSide, topA, topB, bottomA, bottomB);
                }
            }
            else
            {
                if (MapStructureTools.TileSide_IdentifyVertexes(
                    meshVertices[vertexA], meshVertices[vertexB], getElevation(),
                    lowestHeightPossible,
                    out topA, out topB, out bottomA, out bottomB))
                {
                    GenerateTileSide(pref_CliffSide, mat_CliffSide, topA, topB, bottomA, bottomB);
                }
            }
        }
        else
        {
            if (waterNeighbour)
            {
                //No WaterTile/WaterTile tile sides!
                //(maybe later, with different heights?)
            }
            else
            {
                if (MapStructureTools.TileSide_IdentifyVertexes(
                    meshVertices[vertexA], meshVertices[vertexB], getElevation(),
                    groundTile.meshVertices[vertexA], groundTile.meshVertices[vertexB], groundTile.getElevation(),
                    out topA, out topB, out bottomA, out bottomB))
                {
                    GenerateTileSide(pref_CliffSide, mat_CliffSide, topA, topB, bottomA, bottomB);
                }
            }
        }
        return true;
    }

    private void GenerateTileSide(TileSide pref_CliffSide, Material mat_CliffSide, Vector3 topA, Vector3 topB, Vector3 bottomA, Vector3 bottomB)
    {
        TileSide cf = Instantiate(pref_CliffSide, transform);
        cf.DefineMeshParameters(topA, topB, bottomA, bottomB);
        cf.SetMaterial(mat_CliffSide);
        tileSides.Add(cf);
    }
}
