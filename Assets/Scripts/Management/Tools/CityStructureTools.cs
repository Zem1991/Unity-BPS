using BPS;
using BPS.Map;
using BPS.Population;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zem.Directions;

public static class CityStructureTools
{
    public static bool Road_CheckDirection(Tile[,] allTiles, int tilePosX, int tilePosZ, Directions dir)
    {
        switch (dir)
        {
            case Directions.S: //South
                if (tilePosZ - 1 >= 0)
                {
                    if (allTiles[tilePosX, tilePosZ - 1].isRoad)
                        return true;
                }
                break;
            case Directions.W: //West
                if (tilePosX - 1 >= 0)
                {
                    if (allTiles[tilePosX - 1, tilePosZ].isRoad)
                        return true;
                }
                break;
            case Directions.N: //North
                if (tilePosZ + 1 < allTiles.GetLength(1))
                {
                    if (allTiles[tilePosX, tilePosZ + 1].isRoad)
                        return true;
                }
                break;
            case Directions.E: //East
                if (tilePosX + 1 < allTiles.GetLength(0))
                {
                    if (allTiles[tilePosX + 1, tilePosZ].isRoad)
                        return true;
                }
                break;
            default:
                break;
        }
        return false;
    }

    public static void CityBlock_CheckSizeAndShape(CityBlock cb, float tileSize, float cityBlockSize, out bool isSquare, out bool isFlat)
    {
        isSquare = false;
        isFlat = false;

        float minX = float.NaN;
        float maxX = float.NaN;
        float minZ = float.NaN;
        float maxZ = float.NaN;
        float minY = float.NaN;
        float maxY = float.NaN;

        foreach (Tile t in cb.allTiles)
        {
            if (float.IsNaN(minX) || float.IsNaN(maxX))
            {
                minX = t.transform.position.x;
                maxX = t.transform.position.x;
            }
            else
            {
                if (minX > t.transform.position.x)
                    minX = t.transform.position.x;
                else if (maxX < t.transform.position.x)
                    maxX = t.transform.position.x;
            }

            if (float.IsNaN(minZ) || float.IsNaN(maxZ))
            {
                minZ = t.transform.position.z;
                maxZ = t.transform.position.z;
            }
            else
            {
                if (minZ > t.transform.position.z)
                    minZ = t.transform.position.z;
                else if (maxZ < t.transform.position.z)
                    maxZ = t.transform.position.z;
            }

            if (float.IsNaN(minY) || float.IsNaN(maxY))
            {
                minY = t.transform.position.y;
                maxY = t.transform.position.y;
            }
            else
            {
                if (minY > t.transform.position.y)
                    minY = t.transform.position.y;
                else if (maxY < t.transform.position.y)
                    maxY = t.transform.position.y;
            }
        }

        if (((maxX - minX + tileSize) / tileSize == cityBlockSize) &&
            ((maxZ - minZ + tileSize) / tileSize == cityBlockSize))
            isSquare = true;

        if (maxY == minY)
            isFlat = true;
    }

    public static void CityBlock_SetTiles_FloodFill(CityBlock cb, Tile[,] allMapTiles, bool[,] usedTiles, int col, int row)
    {
        cb.allTiles.Add(allMapTiles[col, row]);
        allMapTiles[col, row].cityBlock = cb;
        usedTiles[col, row] = true;

        if (row - 1 >= 0)
            CityBlock_SetTiles_FloodFill_PickNext(cb, allMapTiles, usedTiles, col, row - 1);
        if (row + 1 < allMapTiles.GetLength(1))
            CityBlock_SetTiles_FloodFill_PickNext(cb, allMapTiles, usedTiles, col, row + 1);
        if (col - 1 >= 0)
            CityBlock_SetTiles_FloodFill_PickNext(cb, allMapTiles, usedTiles, col - 1, row);
        if (col + 1 < allMapTiles.GetLength(0))
            CityBlock_SetTiles_FloodFill_PickNext(cb, allMapTiles, usedTiles, col + 1, row);
    }

    private static void CityBlock_SetTiles_FloodFill_PickNext(CityBlock cb, Tile[,] allMapTiles, bool[,] usedTiles, int col, int row)
    {
        if (!usedTiles[col, row] &&
            (allMapTiles[col, row].tileType == TileTypes.GROUND || allMapTiles[col, row].tileType == TileTypes.CLIFF_GROUND))
        {
            CityBlock_SetTiles_FloodFill(cb, allMapTiles, usedTiles, col, row);
        }
    }

    public static bool CityBlock_GetTilesForBuilding(CityBlock cb, LevelObject building,
        float cliffHeight, out List<Tile> tiles, out Road road, out Directions direction)
    {
        tiles = new List<Tile>();
        road = null;
        direction = Directions.NO_DIRECTION;

        List<Tile> usableTiles = new List<Tile>(cb.allBuildableTiles);
        while (usableTiles.Count > 0)
        {
            Tile currentTile = usableTiles[Random.Range(0, usableTiles.Count)];
            Road road_StoN, road_WtoE;
            Directions dir_StoN, dir_WtoE;
            if (Tile_GetCityRoads(currentTile, out road_StoN, out dir_StoN, out road_WtoE, out dir_WtoE))
            {
                if (road_WtoE)
                {
                    road = road_WtoE;
                    direction = dir_WtoE;
                }
                else if (road_StoN)
                {
                    road = road_StoN;
                    direction = dir_StoN;
                }

                if (road != null)
                {
                    if (CityBlock_GetTilesForBuilding_MeasureTiles(
                        currentTile, direction, building.tilesGoingX, building.tilesGoingZ,
                        cliffHeight, building.ignoreCliff, building.ignoreWater, out tiles))
                        return true;
                }
            }
            usableTiles.Remove(currentTile);
        }
        return false;
    }

    private static bool CityBlock_GetTilesForBuilding_MeasureTiles(
        Tile initialTile, Directions facingDirection, int bldgSizeX, int bldgSizeZ, float cliffHeight, bool allowCliff, bool allowWater,
        out List<Tile> resultingTiles)
    {
        resultingTiles = new List<Tile>();

        Tile roadCheckTile = null;
        Directions directionToWalkHorizontal = Directions.NO_DIRECTION;
        Directions directionToWalkVertical = Directions.NO_DIRECTION;
        switch (facingDirection)
        {
            case Directions.S:
                roadCheckTile = initialTile.groundNeighbours[(int)Directions.S];
                directionToWalkHorizontal = Directions.E;
                directionToWalkVertical = Directions.N;
                break;
            case Directions.W:
                roadCheckTile = initialTile.groundNeighbours[(int)Directions.W];
                directionToWalkHorizontal = Directions.S;
                directionToWalkVertical = Directions.E;
                break;
            case Directions.N:
                roadCheckTile = initialTile.groundNeighbours[(int)Directions.N];
                directionToWalkHorizontal = Directions.W;
                directionToWalkVertical = Directions.S;
                break;
            case Directions.E:
                roadCheckTile = initialTile.groundNeighbours[(int)Directions.E];
                directionToWalkHorizontal = Directions.N;
                directionToWalkVertical = Directions.W;
                break;
        }

        //We can only build stuff if all road tiles that would be in front of the building are in the same elevation as the selected tile.
        for (int col = 0; col < bldgSizeX; col++)
        {
            Tile tile = BPSHelperFunctions.TileWalk(roadCheckTile, directionToWalkHorizontal, col);
            if (tile && tile.getElevation() != roadCheckTile.getElevation())
                return false;
        }

        for (int row = 0; row < bldgSizeZ; row++)
        {
            for (int col = 0; col < bldgSizeX; col++)
            {
                Tile nextTile = BPSHelperFunctions.TileWalk(initialTile, directionToWalkVertical, row);
                if (nextTile && Tile_CheckAvailable(nextTile, initialTile, cliffHeight, allowCliff, allowWater))
                {
                    nextTile = BPSHelperFunctions.TileWalk(BPSHelperFunctions.TileWalk(initialTile, directionToWalkHorizontal, col), directionToWalkVertical, row);
                    if (nextTile && Tile_CheckAvailable(nextTile, initialTile, cliffHeight, allowCliff, allowWater))
                    {
                        resultingTiles.Add(nextTile);
                    }
                }
            }
        }

        if (resultingTiles.Count >= bldgSizeX * bldgSizeZ)
            return true;
        else
            return false;
    }

    public static float CityBlock_CheckLandUsage(CityBlock cb)
    {
        int usedTiles = 0;
        foreach (var item in cb.allTiles)
            if (item.mapFeature || item.building)
                usedTiles++;

        return 100F * usedTiles / cb.allTiles.Count;
    }

    public static bool Tile_CheckBuildable(Tile t)
    {
        bool result = false;
        if ((t.tileType == TileTypes.GROUND || t.tileType == TileTypes.CLIFF_GROUND) && t.heightOfLowestVertex == 0)
        {
            if (Tile_CheckBuildable_Aux(t, Directions.S) ||
                Tile_CheckBuildable_Aux(t, Directions.W) ||
                Tile_CheckBuildable_Aux(t, Directions.N) ||
                Tile_CheckBuildable_Aux(t, Directions.E))
                result = true;
        }
        return result;
    }

    private static bool Tile_CheckBuildable_Aux(Tile t, Directions dir)
    {
        Tile other = t.groundNeighbours[(int)dir];
        if (other)
        {
            if (other.tileType == TileTypes.ROAD && other.mapFeature == null)
                return true;
        }
        return false;
    }

    public static bool Tile_CheckAvailable(Tile current, Tile other, float cliffHeight, bool allowCliffToLand, bool allowSubmerged)
    {
        bool result = true;

        float heightDifference = current.getElevation() - other.getElevation();
        heightDifference *= Mathf.Sign(heightDifference);

        if (current.mapFeature != null || current.building != null)
            result = false;

        if (!allowCliffToLand && heightDifference >= cliffHeight)
            result = false;

        if (current.tileType != TileTypes.GROUND && current.tileType != TileTypes.CLIFF_GROUND)
        {
            if (!(allowSubmerged && current.tileType == TileTypes.WATER))
                result = false;
        }

        return result;
    }

    public static bool Tile_GetCityRoads(Tile t, out Road road_StoN, out Directions dir_StoN, out Road road_WtoE, out Directions dir_WtoE)
    {
        //VisualStudio demands those initializations.
        road_StoN = null;
        dir_StoN = Directions.NO_DIRECTION;
        road_WtoE = null;
        dir_WtoE = Directions.NO_DIRECTION;

        Tile aux = t.groundNeighbours[(int)Directions.W];
        if (aux && aux.road_SouthToNorth)
        {
            road_StoN = aux.road_SouthToNorth;
            dir_StoN = Directions.W;
        }

        aux = t.groundNeighbours[(int)Directions.E];
        if (aux && aux.road_SouthToNorth)
        {
            road_StoN = aux.road_SouthToNorth;
            dir_StoN = Directions.E;
        }

        aux = t.groundNeighbours[(int)Directions.S];
        if (aux && aux.road_WestToEast)
        {
            road_WtoE = aux.road_WestToEast;
            dir_WtoE = Directions.S;
        }

        aux = t.groundNeighbours[(int)Directions.N];
        if (aux && aux.road_WestToEast)
        {
            road_WtoE = aux.road_WestToEast;
            dir_WtoE = Directions.N;
        }

        return (road_StoN || road_WtoE);
    }
}
