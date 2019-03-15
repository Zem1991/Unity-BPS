using BPS;
using BPS.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zem.Directions;

public static class CityEditorTools
{
    public static void CityRoad_SetSingleTile(Road road, Tile[,] allTiles, bool[,] usedTiles, int col, int row)
    {
        road.tiles.Add(allTiles[col, row]);
        allTiles[col, row].road_SingleTile = road;
        usedTiles[col, row] = true;
    }

    public static void CityRoad_SetTiles_StoN(Road road, Tile[,] allTiles, bool[,] usedTiles, int col, int row)
    {
        road.tiles.Add(allTiles[col, row]);
        allTiles[col, row].road_SouthToNorth = road;
        usedTiles[col, row] = true;

        if (row - 1 >= 0)
            CityRoad_ValidateTile_StoN(road, allTiles, usedTiles, col, row - 1);
        if (row + 1 < allTiles.GetLength(1))
            CityRoad_ValidateTile_StoN(road, allTiles, usedTiles, col, row + 1);
    }

    public static void CityRoad_SetTiles_WtoE(Road road, Tile[,] allTiles, bool[,] usedTiles, int col, int row)
    {
        road.tiles.Add(allTiles[col, row]);
        allTiles[col, row].road_WestToEast = road;
        usedTiles[col, row] = true;

        if (col - 1 >= 0)
            CityRoad_ValidateTile_WtoE(road, allTiles, usedTiles, col - 1, row);
        if (col + 1 < allTiles.GetLength(0))
            CityRoad_ValidateTile_WtoE(road, allTiles, usedTiles, col + 1, row);
    }

    private static void CityRoad_ValidateTile_StoN(Road road, Tile[,] allTiles, bool[,] usedTiles, int col, int row)
    {
        if (allTiles[col, row].isRoad && road.tiles.IndexOf(allTiles[col, row]) == -1)
            CityRoad_SetTiles_StoN(road, allTiles, usedTiles, col, row);
    }

    private static void CityRoad_ValidateTile_WtoE(Road road, Tile[,] allTiles, bool[,] usedTiles, int col, int row)
    {
        if (allTiles[col, row].isRoad && road.tiles.IndexOf(allTiles[col, row]) == -1)
            CityRoad_SetTiles_WtoE(road, allTiles, usedTiles, col, row);
    }

    public static void Tile_AdjustForRoad(Tile t, float tileHeight)
    {
        Tile s = t.groundNeighbours[(int)Directions.S];
        Tile w = t.groundNeighbours[(int)Directions.W];
        Tile n = t.groundNeighbours[(int)Directions.N];
        Tile e = t.groundNeighbours[(int)Directions.E];

        if (t.road_SouthToNorth && !t.road_WestToEast)
        {
            if (Tile_AdjustForRoad_Aux(t, s) || Tile_AdjustForRoad_Aux(t, n))
                MapEditorTools.Tile_Raise(t, tileHeight);

            if (w && t.getElevation() > w.getElevation())
                MapEditorTools.Tile_Raise(w, tileHeight);
            if (e && t.getElevation() > e.getElevation())
                MapEditorTools.Tile_Raise(e, tileHeight);
        }

        if (!t.road_SouthToNorth && t.road_WestToEast)
        {
            if (Tile_AdjustForRoad_Aux(t, w) || Tile_AdjustForRoad_Aux(t, e))
                MapEditorTools.Tile_Raise(t, tileHeight);

            if (s && t.getElevation() > s.getElevation())
                MapEditorTools.Tile_Raise(s, tileHeight);
            if (n && t.getElevation() > n.getElevation())
                MapEditorTools.Tile_Raise(n, tileHeight);
        }

        if (t.road_SouthToNorth && t.road_WestToEast)
        {
            if ((s && t.getElevation() < s.getElevation()) ||
                (w && t.getElevation() < w.getElevation()) ||
                (n && t.getElevation() < n.getElevation()) ||
                (e && t.getElevation() < e.getElevation()))
                MapEditorTools.Tile_Raise(t, tileHeight);

            if (s && t.getElevation() > s.getElevation())
                MapEditorTools.Tile_Raise(s, tileHeight);
            if (w && t.getElevation() > w.getElevation())
                MapEditorTools.Tile_Raise(w, tileHeight);
            if (n && t.getElevation() > n.getElevation())
                MapEditorTools.Tile_Raise(n, tileHeight);
            if (e && t.getElevation() > e.getElevation())
                MapEditorTools.Tile_Raise(e, tileHeight);
        }
    }

    private static bool Tile_AdjustForRoad_Aux(Tile tile, Tile other)
    {
        if (other && tile.getElevation() < other.getElevation())
            if (other.road_SouthToNorth && other.road_WestToEast)
                return true;
        return false;
    }
}
