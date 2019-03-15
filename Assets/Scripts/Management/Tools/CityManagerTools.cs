using BPS;
using BPS.Population;
using BPS.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zem.Directions;

public static class CityManagerTools
{
    public static void S1_DeleteTownHall(CityManager cm)
    {
        if (cm.theTownHall)
        {
            foreach (Tile item in cm.theTownHall.occupiedTiles)
                item.building = null;
            Object.Destroy(cm.theTownHall.gameObject);
        }
    }

    public static void S1_DeleteRoads(CityManager cm)
    {
        MapManager mm = cm.gameManager.MapManager();

        foreach (var item in mm.groundTiles)
        {
            if (item.isRoad)
            {
                item.road_SingleTile = null;
                item.road_WestToEast = null;
                item.isRoad = false;
            }
        }

        if (cm.roads != null)
        {
            foreach (Road item in cm.roads)
                if (item != null)
                    Object.Destroy(item.gameObject);
            cm.roads.Clear();
        }
    }

    public static void S1_DeleteCityBlocks(CityManager cm)
    {
        MapManager mm = cm.gameManager.MapManager();

        foreach (var item in mm.groundTiles)
        {
            if (item.cityBlock)
            {
                item.cityBlock = null;
            }
        }

        if (cm.cityBlocks != null)
        {
            foreach (CityBlock item in cm.cityBlocks)
                if (item != null)
                    Object.Destroy(item.gameObject);
            cm.cityBlocks.Clear();
        }
    }

    public static void S1_PrepareGroundForTownHall(CityManager cm)
    {
        MapManager mm = cm.gameManager.MapManager();

        float adjustedElevation = mm.tcCenter_SW.getElevation() + mm.tcCenter_NW.getElevation() + mm.tcCenter_NE.getElevation() + mm.tcCenter_SE.getElevation();
        adjustedElevation /= 4;
        adjustedElevation -= (adjustedElevation % mm.tileHeight);
        while (adjustedElevation <= MapManager.MAIN_WATER_HEIGHT)
            adjustedElevation += mm.tileHeight;

        MapEditorTools.MapChunk_Flatten(mm.tcCenter_SW, mm.tileHeight, adjustedElevation);
        MapEditorTools.MapChunk_Flatten(mm.tcCenter_NW, mm.tileHeight, adjustedElevation);
        MapEditorTools.MapChunk_Flatten(mm.tcCenter_NE, mm.tileHeight, adjustedElevation);
        MapEditorTools.MapChunk_Flatten(mm.tcCenter_SE, mm.tileHeight, adjustedElevation);
    }

    public static void S1_CreateTownHall(CityManager cm)
    {
        ResourceManager rm = cm.gameManager.ResourceManager();
        MapManager mm = cm.gameManager.MapManager();

        foreach (CityBlock cb in cm.cityBlocks)
        {
            if (cb.transform.position == mm.GetCenterOfMap())
            {
                cm.theTownHall = PopulationEditorTools.CreateBuilding(rm.pref_TownHall, cb.allTiles, null, cb, Directions.S, cm.gameObject);
                cb.cityBlockType = CityBlockUsage.TOWN_CENTER;
                break;
            }
        }
    }

    public static void S1_CreateRoads(CityManager cm)
    {
        ResourceManager rm = cm.gameManager.ResourceManager();
        MapManager mm = cm.gameManager.MapManager();
        int roadTileCount = cm.cityBlockSize + 1;

        for (int row = 0; row < mm.mapSizeZ; row++)
        {
            for (int col = 0; col < mm.mapSizeX; col++)
            {
                if ((mm.groundTiles[col, row].tileType == TileTypes.GROUND || mm.groundTiles[col, row].tileType == TileTypes.CLIFF_GROUND) &&
                    (col % roadTileCount == cm.roadsOffset || row % roadTileCount == cm.roadsOffset))
                {
                    mm.groundTiles[col, row].isRoad = true;
                }
            }
        }

        bool[,] usedTiles = new bool[mm.mapSizeX, mm.mapSizeZ];
        for (int row = 0; row < mm.mapSizeZ; row++)
        {
            for (int col = 0; col < mm.mapSizeX; col++)
            {
                if (!usedTiles[col, row])
                {
                    if (!mm.groundTiles[col, row].isRoad)
                    {
                        usedTiles[col, row] = true;
                    }
                    else
                    {
                        if (CityStructureTools.Road_CheckDirection(mm.groundTiles, col, row, Directions.S) ||
                            CityStructureTools.Road_CheckDirection(mm.groundTiles, col, row, Directions.N))
                        {
                            //Creates "vertical" roads
                            Road crStoN = Object.Instantiate(rm.pref_Road, cm.wrap_Roads.transform);
                            CityEditorTools.CityRoad_SetTiles_StoN(crStoN, mm.groundTiles, usedTiles, col, row);
                            crStoN.centralizePosition();
                            cm.roads.Add(crStoN);
                        }

                        if (CityStructureTools.Road_CheckDirection(mm.groundTiles, col, row, Directions.W) ||
                            CityStructureTools.Road_CheckDirection(mm.groundTiles, col, row, Directions.E))
                        {
                            //Creates "horizontal" roads
                            Road crWtoE = Object.Instantiate(rm.pref_Road, cm.wrap_Roads.transform);
                            CityEditorTools.CityRoad_SetTiles_WtoE(crWtoE, mm.groundTiles, usedTiles, col, row);
                            crWtoE.centralizePosition();
                            cm.roads.Add(crWtoE);
                        }

                        if (!CityStructureTools.Road_CheckDirection(mm.groundTiles, col, row, Directions.S) &&
                            !CityStructureTools.Road_CheckDirection(mm.groundTiles, col, row, Directions.W) &&
                            !CityStructureTools.Road_CheckDirection(mm.groundTiles, col, row, Directions.N) &&
                            !CityStructureTools.Road_CheckDirection(mm.groundTiles, col, row, Directions.E))
                        {
                            //Creates roads in isolated road tiles (>implying someone would want that to happen)
                            Road cr = Object.Instantiate(rm.pref_Road, cm.wrap_Roads.transform);
                            CityEditorTools.CityRoad_SetSingleTile(cr, mm.groundTiles, usedTiles, col, row);
                            cr.centralizePosition();
                            cm.roads.Add(cr);
                        }
                    }
                }
            }
        }

        foreach (var item in mm.groundTiles)
        {
            if (item.isRoad)
                CityEditorTools.Tile_AdjustForRoad(item, mm.tileHeight);
        }
    }

    public static void S1_CreateCityBlocks(CityManager cm)
    {
        ResourceManager rm = cm.gameManager.ResourceManager();
        MapManager mm = cm.gameManager.MapManager();

        bool[,] usedTiles = new bool[mm.mapSizeX, mm.mapSizeZ];
        for (int row = 0; row < mm.mapSizeZ; row++)
        {
            for (int col = 0; col < mm.mapSizeX; col++)
            {
                if (!usedTiles[col, row])
                {
                    if (mm.groundTiles[col, row].tileType != TileTypes.GROUND &&
                        mm.groundTiles[col, row].tileType != TileTypes.CLIFF_GROUND)
                    {
                        usedTiles[col, row] = true;
                    }
                    else
                    {
                        CityBlock cb = Object.Instantiate(rm.pref_CityBlock, cm.wrap_CityBlocks.transform);
                        CityStructureTools.CityBlock_SetTiles_FloodFill(cb, mm.groundTiles, usedTiles, col, row);
                        cb.centralizePosition();
                        cm.cityBlocks.Add(cb);
                    }
                }
            }
        }

        foreach (CityBlock cb in cm.cityBlocks)
            foreach (Tile t in cb.allTiles)
                if (CityStructureTools.Tile_CheckBuildable(t))
                    cb.allBuildableTiles.Add(t);
    }

    public static void S1_IdentifyCityBlocksSizeAndShapes(CityManager cm)
    {
        MapManager mm = cm.gameManager.MapManager();

        foreach (CityBlock cb in cm.cityBlocks)
        {
            CityStructureTools.CityBlock_CheckSizeAndShape(cb, mm.tileSize, cm.cityBlockSize, out cb.isSquare, out cb.isFlat);

            if (cb.isSquare)
                cm.cb_isSquare.Add(cb);

            if (cb.isFlat)
                cm.cb_isFlat.Add(cb);
        }
    }
}
