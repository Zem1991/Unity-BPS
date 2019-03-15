using BPS;
using BPS.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zem.Directions;

public static class MapEditorTools
{
    public static bool MapChunk_Flatten(MapChunk mc, float tileHeight, float desiredHeightToAchieve)
    {
        foreach (var item in mc.groundTiles)
        {
            while (item.getElevation() != desiredHeightToAchieve)
            {
                if (item.getElevation() > desiredHeightToAchieve)
                    Tile_Lower(item, tileHeight, true, false);
                else if (item.getElevation() < desiredHeightToAchieve)
                    Tile_Raise(item, tileHeight, true, false);
            }
        }
        return true;
    }

    public static bool Tile_Lower(Tile t, float tileHeight)
    {
        return Tile_Lower(t, tileHeight, false, true);
    }

    public static bool Tile_Lower(Tile t, float tileHeight, bool forceFlatness, bool adjustNeighbours)
    {
        if (!forceFlatness && adjustNeighbours)
        {
            foreach (var item in t.groundNeighbours)
                if (item && MapStructureTools.CompareTileElevations(t, item, tileHeight) < 0)
                    Tile_Lower(item, tileHeight, forceFlatness, adjustNeighbours);
        }

        t.changeElevation(-tileHeight, forceFlatness);
        ValidateAndDrawTile(t);
        DrawTile(t);

        return true;
    }

    public static bool Tile_Lower(Tile t, float tileHeight, int spreadChance, int spreadDecay, List<Tile> tilesAffected)
    {
        int rng = Random.Range(0, 100);
        if (rng < spreadChance)
        {
            Tile_Lower(t, tileHeight, false, true);
            tilesAffected.Add(t);

            if (Tile_Lower_BlobAux(t, t.groundNeighbours[(int)Directions.S], tilesAffected))
                Tile_Lower(t.groundNeighbours[(int)Directions.S], tileHeight, spreadChance - spreadDecay, spreadDecay, tilesAffected);
            if (Tile_Lower_BlobAux(t, t.groundNeighbours[(int)Directions.W], tilesAffected))
                Tile_Lower(t.groundNeighbours[(int)Directions.W], tileHeight, spreadChance - spreadDecay, spreadDecay, tilesAffected);
            if (Tile_Lower_BlobAux(t, t.groundNeighbours[(int)Directions.N], tilesAffected))
                Tile_Lower(t.groundNeighbours[(int)Directions.N], tileHeight, spreadChance - spreadDecay, spreadDecay, tilesAffected);
            if (Tile_Lower_BlobAux(t, t.groundNeighbours[(int)Directions.E], tilesAffected))
                Tile_Lower(t.groundNeighbours[(int)Directions.E], tileHeight, spreadChance - spreadDecay, spreadDecay, tilesAffected);
        }
        return true;
    }
    private static bool Tile_Lower_BlobAux(Tile t, Tile other, List<Tile> tilesAffected)
    {
        if (other && t.getElevation() < other.getElevation() && tilesAffected.IndexOf(other) == -1)
            return true;
        else
            return false;
    }

    public static bool Tile_Raise(Tile t, float tileHeight)
    {
        return Tile_Raise(t, tileHeight, false, true);
    }

    public static bool Tile_Raise(Tile t, float tileHeight, bool forceFlatness, bool adjustNeighbours)
    {
        if (!forceFlatness && adjustNeighbours)
        {
            foreach (var item in t.groundNeighbours)
                if (item && MapStructureTools.CompareTileElevations(t, item, tileHeight) > 0)
                    Tile_Raise(item, tileHeight, forceFlatness, adjustNeighbours);
        }

        t.changeElevation(tileHeight, forceFlatness);
        ValidateAndDrawTile(t);
        DrawTile(t);

        return true;
    }

    public static bool Tile_Raise(Tile t, float tileHeight, int spreadChance, int spreadDecay, List<Tile> tilesAffected)
    {
        int rng = Random.Range(0, 100);
        if (rng < spreadChance)
        {
            Tile_Raise(t, tileHeight, false, true);
            tilesAffected.Add(t);

            if (Tile_Raise_BlobAux(t, t.groundNeighbours[(int)Directions.S], tilesAffected))
                Tile_Raise(t.groundNeighbours[(int)Directions.S], tileHeight, spreadChance - spreadDecay, spreadDecay, tilesAffected);
            if (Tile_Raise_BlobAux(t, t.groundNeighbours[(int)Directions.W], tilesAffected))
                Tile_Raise(t.groundNeighbours[(int)Directions.W], tileHeight, spreadChance - spreadDecay, spreadDecay, tilesAffected);
            if (Tile_Raise_BlobAux(t, t.groundNeighbours[(int)Directions.N], tilesAffected))
                Tile_Raise(t.groundNeighbours[(int)Directions.N], tileHeight, spreadChance - spreadDecay, spreadDecay, tilesAffected);
            if (Tile_Raise_BlobAux(t, t.groundNeighbours[(int)Directions.E], tilesAffected))
                Tile_Raise(t.groundNeighbours[(int)Directions.E], tileHeight, spreadChance - spreadDecay, spreadDecay, tilesAffected);
        }
        return true;
    }
    private static bool Tile_Raise_BlobAux(Tile t, Tile other, List<Tile> tilesAffected)
    {
        if (other && t.getElevation() > other.getElevation() && tilesAffected.IndexOf(other) == -1)
            return true;
        else
            return false;
    }

    public static bool ValidateAllMapChunks(MapManager mm)
    {
        if (mm.mapChunks != null && mm.mapChunks.Length > 3)
        {
            mm.tcCenter_SW = mm.mapChunks[(mm.chunkCountX - 1) / 2, mm.chunkCountZ / 2];
            mm.tcCenter_NW = mm.mapChunks[(mm.chunkCountX - 1) / 2, (mm.chunkCountZ - 1) / 2];
            mm.tcCenter_NE = mm.mapChunks[mm.chunkCountX / 2, (mm.chunkCountZ - 1) / 2];
            mm.tcCenter_SE = mm.mapChunks[mm.chunkCountX / 2, mm.chunkCountZ / 2];
        }
        return true;
    }

    public static bool ValidateAllTiles(int mapSizeX, int mapSizeZ, Tile[,] groundTiles, Tile[,] waterTiles)
    {
        for (int row = 0; row < mapSizeZ; row++)
        {
            for (int col = 0; col < mapSizeX; col++)
            {
                ValidateGroundTile(groundTiles[col, row]);
                ValidateWaterTile(waterTiles[col, row]);
            }
        }
        for (int row = 0; row < mapSizeZ; row++)
        {
            for (int col = 0; col < mapSizeX; col++)
            {
                //ValidateTileSides(groundTiles[col, row], waterTiles[col, row]);
            }
        }
        return true;
    }

    public static bool ValidateGroundTile(Tile t)
    {
        MapManager mm = t.mapManager;
        ResourceManager rm = mm.gameManager.ResourceManager();

        if (!t)
            return false;

        t.DefineMeshParameters(mm.tileSize, mm.tileHeight, mm.cliffHeight);
        t.mustDrawMeshes = true;

        if (t.isRoad)
            t.SetTileTypeAndMaterial(TileTypes.ROAD, rm.mat_Road);
        else if (t.isRiver)
            t.SetTileTypeAndMaterial(TileTypes.RIVER, rm.mat_Test);
        else if (t.isCliff)
            t.SetTileTypeAndMaterial(TileTypes.CLIFF_GROUND, rm.mat_Tile_CliffGround);
        else
            t.SetTileTypeAndMaterial(TileTypes.GROUND, rm.mat_Tile_Ground);

        return true;
    }

    public static bool ValidateWaterTile(Tile t)
    {
        MapManager mm = t.mapManager;
        ResourceManager rm = mm.gameManager.ResourceManager();

        if (!t)
            return false;

        t.DefineMeshParameters(mm.tileSize, mm.tileHeight, mm.cliffHeight);
        if (t.groundTile.getElevation() - t.groundTile.heightOfLowestVertex >= MapManager.MAIN_WATER_HEIGHT)
            t.mustDrawMeshes = false;
        else
            t.mustDrawMeshes = true;

        t.SetTileTypeAndMaterial(TileTypes.WATER, rm.mat_Water);

        return true;
    }

    public static bool ValidateTileSides(Tile gt, Tile wt)
    {
        MapManager mm = gt.mapManager;
        ResourceManager rm = mm.gameManager.ResourceManager();

        if (!gt || !wt)
            return false;

        if (gt.mustDrawMeshes)
            gt.GenerateTileSides(rm.pref_CliffSide, rm.mat_CliffSide, MapManager.MIN_CHUNK_HEIGHT * mm.tileHeight);
        if (wt.mustDrawMeshes)
            wt.GenerateTileSides(rm.pref_CliffSide, rm.mat_Water, MapManager.MIN_CHUNK_HEIGHT * mm.tileHeight);

        return true;
    }

    public static bool DrawAllTiles(int mapSizeX, int mapSizeZ, Tile[,] groundTiles, Tile[,] waterTiles)
    {
        for (int row = 0; row < mapSizeZ; row++)
        {
            for (int col = 0; col < mapSizeX; col++)
            {
                DrawTile(groundTiles[col, row]);
                DrawTile(waterTiles[col, row]);
            }
        }
        return true;
    }

    public static bool DrawTile(Tile t)
    {
        t.DrawMesh();
        //t.DrawTileSidesMeshes();
        return true;
    }

    public static bool ValidateAndDrawTile(Tile t)
    {
        ValidateGroundTile(t);
        ValidateWaterTile(t.waterTile);
        //ValidateTileSides(t, t.waterTile);

        foreach (var item in t.groundNeighbours)
        {
            if (item)
            {
                ValidateGroundTile(item);
                ValidateWaterTile(item.waterTile);
                //ValidateTileSides(item, item.waterTile);
            }
        }

        DrawTile(t);

        foreach (var item in t.groundNeighbours)
        {
            if (item)
            {
                DrawTile(item);
            }
        }

        return true;
    }
}
