using BPS;
using BPS.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zem.Directions;

public static class MapManagerTools
{
    public static void S1_DeleteMapChunks(MapManager mm)
    {
        if (mm.mapChunks != null)
        {
            foreach (MapChunk item in mm.mapChunks)
                if (item != null)
                    Object.Destroy(item.gameObject);
        }
    }

    public static void S1_DeleteTiles(MapManager mm)
    {
        if (mm.groundTiles != null)
        {
            foreach (Tile item in mm.groundTiles)
                if (item != null)
                    Object.Destroy(item.gameObject);
        }

        if (mm.waterTiles != null)
        {
            foreach (Tile item in mm.waterTiles)
                if (item != null)
                    Object.Destroy(item.gameObject);
        }
    }

    public static void S1_CreateMapChunks(MapManager mm)
    {
        ResourceManager rm = mm.gameManager.ResourceManager();

        mm.mapChunks = new MapChunk[mm.chunkCountX, mm.chunkCountZ];
        float chunkOffsetX = (mm.chunkCountX - 1) / 2F;
        float chunkOffsetZ = (mm.chunkCountZ - 1) / 2F;
        for (int row = 0; row < mm.chunkCountZ; row++)
        {
            for (int col = 0; col < mm.chunkCountX; col++)
            {
                float posX = (col - chunkOffsetX) * ((float)mm.tileSize * mm.chunkSizeX);
                float posY = 0;
                float posZ = (row - chunkOffsetZ) * ((float)mm.tileSize * mm.chunkSizeZ);
                Vector3 chunkPos = new Vector3(posX, posY, posZ);
                MapChunk tc = Object.Instantiate(rm.pref_Chunk, chunkPos, Quaternion.identity, mm.wrap_Chunks.transform);
                tc.SetupArrays(mm.chunkSizeX, mm.chunkSizeZ);
                mm.mapChunks[col, row] = tc;
                tc.mapManager = mm;
            }
        }
    }

    public static void S1_CreateTiles(MapManager mm)
    {
        ResourceManager rm = mm.gameManager.ResourceManager();

        mm.groundTiles = new Tile[mm.mapSizeX, mm.mapSizeZ];
        mm.waterTiles = new Tile[mm.mapSizeX, mm.mapSizeZ];
        for (int chunkRow = 0; chunkRow < mm.chunkCountZ; chunkRow++)
        {
            for (int chunkCol = 0; chunkCol < mm.chunkCountX; chunkCol++)
            {
                Tile[,] chunkGroundTiles = new Tile[mm.chunkSizeX, mm.chunkSizeZ];
                Tile[,] chunkWaterTiles = new Tile[mm.chunkSizeX, mm.chunkSizeZ];
                float tileOffsetX = (mm.chunkSizeX - 1) / 2F;
                float tileOffsetZ = (mm.chunkSizeZ - 1) / 2F;
                for (int tileRow = 0; tileRow < mm.chunkSizeZ; tileRow++)
                {
                    for (int tileCol = 0; tileCol < mm.chunkSizeX; tileCol++)
                    {
                        float posX = (tileCol - tileOffsetX) * mm.tileSize;
                        float posZ = (tileRow - tileOffsetZ) * mm.tileSize;
                        Vector3 groundPos = new Vector3(posX, mm.initialElevation * mm.tileHeight, posZ);
                        Vector3 waterPos = new Vector3(posX, 0, posZ);

                        Tile gt = Object.Instantiate(rm.pref_GroundTile, mm.mapChunks[chunkCol, chunkRow].wrap_GroundTiles.transform);
                        Tile wt = Object.Instantiate(rm.pref_WaterTile, mm.mapChunks[chunkCol, chunkRow].wrap_WaterTiles.transform);
                        gt.transform.localPosition = groundPos;
                        wt.transform.localPosition = waterPos;
                        gt.isWaterTile = false;
                        wt.isWaterTile = true;

                        int tileX = chunkCol * mm.chunkSizeX + tileCol;
                        int tileZ = chunkRow * mm.chunkSizeZ + tileRow;
                        gt.tileId = new Vector2(tileX, tileZ);
                        wt.tileId = new Vector2(tileX, tileZ);
                        mm.groundTiles[tileX, tileZ] = gt;
                        mm.waterTiles[tileX, tileZ] = wt;

                        chunkGroundTiles[tileCol, tileRow] = gt;
                        chunkWaterTiles[tileCol, tileRow] = wt;
                        gt.mapChunk = mm.mapChunks[chunkCol, chunkRow];
                        wt.mapChunk = mm.mapChunks[chunkCol, chunkRow];
                        gt.waterTile = wt;
                        wt.groundTile = gt;
                        gt.mapManager = mm;
                        wt.mapManager = mm;
                        gt.name = "Tile " + gt.tileId;
                        wt.name = "WaterTile " + wt.tileId;
                    }
                }
                mm.mapChunks[chunkCol, chunkRow].groundTiles = chunkGroundTiles;
                mm.mapChunks[chunkCol, chunkRow].waterTiles = chunkWaterTiles;
            }
        }
    }

    public static void S1_FindMapChunkNeighbours(MapManager mm)
    {
        for (int row = 0; row < mm.chunkCountZ; row++)
        {
            for (int col = 0; col < mm.chunkCountX; col++)
            {
                if (row - 1 >= 0)
                {
                    mm.mapChunks[col, row].neighbourChunks[(int)Directions.S] = mm.mapChunks[col, row - 1];
                    if (col - 1 >= 0)
                    {
                        mm.mapChunks[col, row].neighbourChunks[(int)Directions.SW] = mm.mapChunks[col - 1, row - 1];
                    }
                    if (col + 1 < mm.chunkCountX)
                    {
                        mm.mapChunks[col, row].neighbourChunks[(int)Directions.SE] = mm.mapChunks[col + 1, row - 1];
                    }
                }
                if (row + 1 < mm.chunkCountZ)
                {
                    mm.mapChunks[col, row].neighbourChunks[(int)Directions.N] = mm.mapChunks[col, row + 1];
                    if (col - 1 >= 0)
                    {
                        mm.mapChunks[col, row].neighbourChunks[(int)Directions.NW] = mm.mapChunks[col - 1, row + 1];
                    }
                    if (col + 1 < mm.chunkCountX)
                    {
                        mm.mapChunks[col, row].neighbourChunks[(int)Directions.NE] = mm.mapChunks[col + 1, row + 1];
                    }
                }
                if (col - 1 >= 0)
                {
                    mm.mapChunks[col, row].neighbourChunks[(int)Directions.W] = mm.mapChunks[col - 1, row];
                }
                if (col + 1 < mm.chunkCountX)
                {
                    mm.mapChunks[col, row].neighbourChunks[(int)Directions.E] = mm.mapChunks[col + 1, row];
                }
            }
        }
    }

    public static void S1_FindTileNeighbours(MapManager mm)
    {
        for (int row = 0; row < mm.mapSizeZ; row++)
        {
            for (int col = 0; col < mm.mapSizeX; col++)
            {
                if (row - 1 >= 0)
                {
                    mm.groundTiles[col, row].groundNeighbours[(int)Directions.S] = mm.groundTiles[col, row - 1];
                    mm.waterTiles[col, row].waterNeighbours[(int)Directions.S] = mm.waterTiles[col, row - 1];
                    if (col - 1 >= 0)
                    {
                        mm.groundTiles[col, row].groundNeighbours[(int)Directions.SW] = mm.groundTiles[col - 1, row - 1];
                        mm.waterTiles[col, row].waterNeighbours[(int)Directions.SW] = mm.waterTiles[col - 1, row - 1];
                    }
                    if (col + 1 < mm.mapSizeX)
                    {
                        mm.groundTiles[col, row].groundNeighbours[(int)Directions.SE] = mm.groundTiles[col + 1, row - 1];
                        mm.waterTiles[col, row].waterNeighbours[(int)Directions.SE] = mm.waterTiles[col + 1, row - 1];
                    }
                }
                if (row + 1 < mm.mapSizeZ)
                {
                    mm.groundTiles[col, row].groundNeighbours[(int)Directions.N] = mm.groundTiles[col, row + 1];
                    mm.waterTiles[col, row].waterNeighbours[(int)Directions.N] = mm.waterTiles[col, row + 1];
                    if (col - 1 >= 0)
                    {
                        mm.groundTiles[col, row].groundNeighbours[(int)Directions.NW] = mm.groundTiles[col - 1, row + 1];
                        mm.waterTiles[col, row].waterNeighbours[(int)Directions.NW] = mm.waterTiles[col - 1, row + 1];
                    }
                    if (col + 1 < mm.mapSizeX)
                    {
                        mm.groundTiles[col, row].groundNeighbours[(int)Directions.NE] = mm.groundTiles[col + 1, row + 1];
                        mm.waterTiles[col, row].waterNeighbours[(int)Directions.NE] = mm.waterTiles[col + 1, row + 1];
                    }
                }
                if (col - 1 >= 0)
                {
                    mm.groundTiles[col, row].groundNeighbours[(int)Directions.W] = mm.groundTiles[col - 1, row];
                    mm.waterTiles[col, row].waterNeighbours[(int)Directions.W] = mm.waterTiles[col - 1, row];
                }
                if (col + 1 < mm.mapSizeX)
                {
                    mm.groundTiles[col, row].groundNeighbours[(int)Directions.E] = mm.groundTiles[col + 1, row];
                    mm.waterTiles[col, row].waterNeighbours[(int)Directions.E] = mm.waterTiles[col + 1, row];
                }
            }
        }
    }

    public static void S2_ResetTileElevations(MapManager mm)
    {
        if (mm.groundTiles != null)
        {
            foreach (var item in mm.groundTiles)
            {
                Vector3 newPos = item.transform.position;
                newPos.y = mm.initialElevation * mm.tileHeight;
                item.transform.position = newPos;
                item.isCliff = false;
            }
        }
    }

    public static void S2_RemoveCliffs(MapManager mm)
    {
        if (mm.groundTiles != null)
        {
            foreach (var item in mm.groundTiles)
            {
                item.isCliff = false;
                //MapEditorTools.Tile_Lower(item, tileHeight);
            }
        }
    }

    public static void S2_RandomizeTileElevations(MapManager mm)
    {
        int blobsRaised = 0;
        int blobsLowered = 0;
        while (blobsRaised < mm.elevBlobsToRaise)
        {
            Tile currentTile = mm.groundTiles[Random.Range(0, mm.mapSizeX), Random.Range(0, mm.mapSizeZ)];
            MapEditorTools.Tile_Raise(currentTile, mm.tileHeight, mm.elevBlobInitialSpread, mm.elevBlobSpreadDecay, new List<Tile>());
            blobsRaised++;
        }
        while (blobsLowered < mm.elevBlobsToLower)
        {
            Tile currentTile = mm.groundTiles[Random.Range(0, mm.mapSizeX), Random.Range(0, mm.mapSizeZ)];
            MapEditorTools.Tile_Lower(currentTile, mm.tileHeight, mm.elevBlobInitialSpread, mm.elevBlobSpreadDecay, new List<Tile>());
            blobsLowered++;
        }
    }

    public static void S2_GenerateCliffs(MapManager mm)
    {
        Debug.Log("S2_GenerateCliffs()");
    }

    public static void S3_RemoveRivers(MapManager mm)
    {
        if (mm.groundTiles != null)
        {
            foreach (var item in mm.groundTiles)
            {
                item.isRiver = false;
                MapEditorTools.Tile_Raise(item, mm.tileHeight);
            }
        }
    }

    public static void S3_GenerateRivers(MapManager mm)
    {
        int riversCreated = 0;
        int riverTiles = 0;
        while (riversCreated < mm.riversToCreate && riverTiles < mm.maxRiverLength)
        {
            Tile currentTile = mm.groundTiles[Random.Range(0, mm.mapSizeX), Random.Range(0, mm.mapSizeZ)];
            Directions riverDirection = DirectionsClass.RandomDirection();
            riversCreated++;
            bool keepDoing = true;

            while (keepDoing)
            {
                if (!currentTile.isRiver)
                {
                    currentTile.isRiver = true;
                    MapEditorTools.Tile_Lower(currentTile, mm.tileHeight);
                    //MapEditorTools.Tile_Lower(currentTile, tileHeight, false, false);
                }
                riverTiles++;

                ////Directions nextDir = DirectionsClass.randomDirectionTowardsDirection(riverDirection, 3, 2, 1, 0, 0);
                //if (currentTile.waterNeighbours[(int)nextDir] && riverTiles < mm.maxRiverLength)
                //    currentTile = currentTile.waterNeighbours[(int)nextDir];
                //else
                //    keepDoing = false;
            }
        }
    }
}
