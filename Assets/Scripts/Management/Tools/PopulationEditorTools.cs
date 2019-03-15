using BPS;
using BPS.Map;
using BPS.Population;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zem.Directions;

public static class PopulationEditorTools
{
    public static void PurposeZoningForCityBlocks(List<CityBlock> cityBlocks, PurposeZoning zoningType, int threshold, int minimumRequired,
        out List<CityBlock> zonedCBs)
    {
        zonedCBs = new List<CityBlock>();
        int cityBlocksToDefine = Mathf.RoundToInt(cityBlocks.Count * threshold / 100);

        List<CityBlock> remainingCBs = new List<CityBlock>(cityBlocks);
        foreach (var cb in cityBlocks)
            if (cb.purposeZoning != PurposeZoning.UNKNOWN)
                remainingCBs.Remove(cb);

        while (zonedCBs.Count < cityBlocksToDefine || zonedCBs.Count < minimumRequired)
        {
            CityBlock cb = remainingCBs[Random.Range(0, remainingCBs.Count)];
            cb.purposeZoning = zoningType;
            zonedCBs.Add(cb);
            remainingCBs.Remove(cb);
        }
    }

    public static void EconomicalZoningForCityBlocks(List<CityBlock> cityBlocks, EconomicalZoning zoningType, int threshold, int minimumRequired,
         out List<CityBlock> zonedCBs)
    {
        zonedCBs = new List<CityBlock>();
        int cityBlocksToDefine = Mathf.RoundToInt(cityBlocks.Count * threshold / 100);

        List<CityBlock> remainingCBs = new List<CityBlock>(cityBlocks);
        foreach (var cb in cityBlocks)
            if (cb.economicalZoning != EconomicalZoning.UNKNOWN)
                remainingCBs.Remove(cb);

        while (zonedCBs.Count < cityBlocksToDefine || zonedCBs.Count < minimumRequired)
        {
            CityBlock cb = remainingCBs[Random.Range(0, remainingCBs.Count)];
            cb.economicalZoning = zoningType;
            zonedCBs.Add(cb);
            remainingCBs.Remove(cb);
        }
    }

    public static LevelObject CreateBuilding(LevelObject cityBuilding, List<Tile> tilesToUse, Road road, CityBlock cityBlock, Directions facingDirection, GameObject parentObj)
    {
        Vector3 pos = Vector3.zero;
        float highestY = float.NaN;
        foreach (var item in tilesToUse)
        {
            pos += item.transform.position;
            if (float.IsNaN(float.NaN) || highestY < item.transform.position.y)
                highestY = item.transform.position.y;
        }
        pos /= tilesToUse.Count;
        pos.y = highestY;

        Directions fixedFacingDirection;
        switch (facingDirection)
        {
            case (Directions.E):
                fixedFacingDirection = Directions.W;
                break;
            case (Directions.W):
                fixedFacingDirection = Directions.E;
                break;
            default:
                fixedFacingDirection = facingDirection;
                    break;
        }
        Quaternion rot = Quaternion.Euler(0, DirectionsClass.DirectionAsDegrees(fixedFacingDirection) + 90, 0);

        LevelObject newBldg = Object.Instantiate(cityBuilding, pos, rot, parentObj.transform);
        newBldg.frontRoad = road;
        newBldg.cityBlock = cityBlock;
        foreach (Tile item in tilesToUse)
        {
            newBldg.occupiedTiles.Add(item);
            item.building = newBldg;
            item.artificialElevation = newBldg.getElevation() - item.getElevation();
        }
        return newBldg;
    }

    public static void CreateMultipleBuildings(LevelObject cityBuilding, int amountToCreate, List<CityBlock> cityBlocks, GameObject parentObj, float cliffHeight,
         out List<LevelObject> featuresCreated, out List<CityBlock> remainingCityBlocks)
    {
        featuresCreated = new List<LevelObject>();
        remainingCityBlocks = new List<CityBlock>(cityBlocks);

        while (featuresCreated.Count < amountToCreate && remainingCityBlocks.Count > 0)
        {
            CityBlock cb = remainingCityBlocks[Random.Range(0, remainingCityBlocks.Count)];

            LevelObject bldg = CreateMultipleCityBuildings_CreateOne(cb, cityBuilding, cliffHeight, parentObj);
            if (bldg)
                featuresCreated.Add(bldg);

            remainingCityBlocks.Remove(cb);
        }
    }

    public static void CreateMultipleBuildings(List<LevelObject> options, int landUsage_Pct, List<CityBlock> cityBlocks, GameObject parentObj, float cliffHeight,
         out List<LevelObject> featuresCreated)
    {
        featuresCreated = new List<LevelObject>();

        foreach (var cb in cityBlocks)
        {
            int attempts = 0;

            while (CityStructureTools.CityBlock_CheckLandUsage(cb) < landUsage_Pct && attempts < 3)
            {
                LevelObject option = options[Random.Range(0, options.Count)];
                LevelObject bldg = CreateMultipleCityBuildings_CreateOne(cb, option, cliffHeight, parentObj);
                if (bldg)
                    featuresCreated.Add(bldg);

                attempts++;
            }
        }
    }

    private static LevelObject CreateMultipleCityBuildings_CreateOne(CityBlock cb, LevelObject cityBuilding, float cliffHeight, GameObject parentObj)
    {
        LevelObject bldg = null;

        List<Tile> tiles = new List<Tile>();
        Road road;
        Directions direction;
        if (CityStructureTools.CityBlock_GetTilesForBuilding(cb, cityBuilding, cliffHeight, out tiles, out road, out direction))
            bldg = CreateBuilding(cityBuilding, tiles, road, cb, direction, parentObj);

        return bldg;
    }
}
