using BPS;
using BPS.Map;
using BPS.Population;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PopulationManagerTools
{
    public static void S1A_ClearPurposeZoning(PopulationManager pm)
    {
        foreach (var item in pm.cb_notZoned)
            item.purposeZoning = PurposeZoning.UNKNOWN;
        foreach (var item in pm.cb_zoned)
            item.purposeZoning = PurposeZoning.UNKNOWN;
    }

    public static void S1B_ClearEconomicZoning(PopulationManager pm)
    {
        foreach (var item in pm.cb_notZoned)
            item.economicalZoning = EconomicalZoning.UNKNOWN;
        foreach (var item in pm.cb_zoned)
            item.economicalZoning = EconomicalZoning.UNKNOWN;
    }

    public static void S1A_ClearCityBlocksUsage(PopulationManager pm)
    {
        foreach (var item in pm.cb_notZoned)
            item.cityBlockType = CityBlockUsage.UNKNOWN;
        foreach (var item in pm.cb_zoned)
            item.cityBlockType = CityBlockUsage.UNKNOWN;

        pm.cb_notZoned.Clear();
        pm.cb_zoned.Clear();
        pm.cb_allResidential.Clear();
        pm.cb_allCommercial.Clear();
        pm.cb_allIndustrial.Clear();
    }

    public static void S1C_DeleteSpecialFeatures(PopulationManager pm)
    {
        foreach (var item in pm.co_specialFeatures)
        {
            foreach (Tile t in item.occupiedTiles)
                t.building = null;
            Object.Destroy(item.gameObject);
        }
        pm.co_specialFeatures.Clear();
    }

    public static void S1D_DeleteCommonFeatures(PopulationManager pm)
    {
        foreach (var item in pm.co_residences)
        {
            foreach (Tile t in item.occupiedTiles)
                t.building = null;
            Object.Destroy(item.gameObject);
        }
        foreach (var item in pm.co_commerces)
        {
            foreach (Tile t in item.occupiedTiles)
                t.building = null;
            Object.Destroy(item.gameObject);
        }
        foreach (var item in pm.co_industries)
        {
            foreach (Tile t in item.occupiedTiles)
                t.building = null;
            Object.Destroy(item.gameObject);
        }
        pm.co_residences.Clear();
        pm.co_commerces.Clear();
        pm.co_industries.Clear();
    }

    public static void S1A_DefineCityBlocksUsage(PopulationManager pm)
    {
        CityManager cm = pm.gameManager.CityManager();

        List<CityBlock> allCB = new List<CityBlock>(cm.cityBlocks);
        Vector3 pos = pm.gameManager.MapManager().GetCenterOfMap();

        //First we remove the already defined CityBlocks (such as TOWN_CENTER)
        foreach (CityBlock cb in cm.cityBlocks)
            if (cb.cityBlockType != CityBlockUsage.UNKNOWN)
                allCB.Remove(cb);

        //ZONED
        //Some CitBlocks we mark as zoned so we can define them later as either Residential, Commercial or Industrial.
        int amount = Mathf.RoundToInt(allCB.Count * pm.zonedBlocks_Pct / 100F);
        while (pm.cb_zoned.Count < amount && 0 < allCB.Count)
        {
            CityBlock cb = allCB[Random.Range(0, allCB.Count)];
            cb.cityBlockType = CityBlockUsage.ZONED;
            pm.cb_zoned.Add(cb);
            allCB.Remove(cb);
        }

        //NOT_ZONED
        //The remaining CityBlocks are free of zoning.
        foreach (var cb in allCB)
        {
            cb.cityBlockType = CityBlockUsage.NOT_ZONED;
            pm.cb_notZoned.Add(cb);
        }
    }

    public static void S1A_DefinePurposeZoning(PopulationManager pm)
    {
        List<CityBlock> zonedCBs = new List<CityBlock>();
        List<CityBlock> allCB = new List<CityBlock>(pm.cb_zoned);

        PopulationEditorTools.PurposeZoningForCityBlocks(allCB, PurposeZoning.RESIDENTIAL, pm.residential_Pct, 0, out zonedCBs);
        pm.cb_allResidential = zonedCBs;

        PopulationEditorTools.PurposeZoningForCityBlocks(allCB, PurposeZoning.COMMERCIAL, pm.commercial_Pct, 0, out zonedCBs);
        pm.cb_allCommercial = zonedCBs;

        PopulationEditorTools.PurposeZoningForCityBlocks(allCB, PurposeZoning.INDUSTRIAL, pm.industrial_Pct, 0, out zonedCBs);
        pm.cb_allIndustrial = zonedCBs;

        //If for some reason we still have CityBlocks without PurposeZoning, we set them to RESIDENTIAL.
        foreach (var cb in allCB)
        {
            if (cb.purposeZoning == PurposeZoning.UNKNOWN)
            {
                cb.purposeZoning = PurposeZoning.RESIDENTIAL;
                pm.cb_allResidential.Add(cb);
            }
        }
    }

    public static void S1B_DefineEconomicZoning(PopulationManager pm)
    {
        List<CityBlock> zonedCBs = new List<CityBlock>();
        List<CityBlock> allResCB = new List<CityBlock>(pm.cb_allResidential);

        PopulationEditorTools.EconomicalZoningForCityBlocks(allResCB, EconomicalZoning.POOR, pm.ecoPoor_Pct, 0, out zonedCBs);
        pm.cb_resPoor = zonedCBs;

        PopulationEditorTools.EconomicalZoningForCityBlocks(allResCB, EconomicalZoning.MEDIUM, pm.ecoMedium_Pct, 0, out zonedCBs);
        pm.cb_resMedium = zonedCBs;

        PopulationEditorTools.EconomicalZoningForCityBlocks(allResCB, EconomicalZoning.RICH, pm.ecoRich_Pct, 0, out zonedCBs);
        pm.cb_resRich = zonedCBs;

        //If for some reason we still have Residential CityBlocks without EconomicalZoning, we set them to POOR.
        foreach (var cb in allResCB)
        {
            if (cb.economicalZoning == EconomicalZoning.UNKNOWN)
            {
                cb.economicalZoning = EconomicalZoning.POOR;
                pm.cb_resPoor.Add(cb);
            }
        }

        List<CityBlock> allComCB = new List<CityBlock>(pm.cb_allCommercial);

        PopulationEditorTools.EconomicalZoningForCityBlocks(allComCB, EconomicalZoning.POOR, pm.ecoPoor_Pct, 0, out zonedCBs);
        pm.cb_comPoor = zonedCBs;

        PopulationEditorTools.EconomicalZoningForCityBlocks(allComCB, EconomicalZoning.MEDIUM, pm.ecoMedium_Pct, 0, out zonedCBs);
        pm.cb_comMedium = zonedCBs;

        PopulationEditorTools.EconomicalZoningForCityBlocks(allComCB, EconomicalZoning.RICH, pm.ecoRich_Pct, 0, out zonedCBs);
        pm.cb_comRich = zonedCBs;

        //If for some reason we still have Commercial CityBlocks without EconomicalZoning, we set them to POOR.
        foreach (var cb in allComCB)
        {
            if (cb.economicalZoning == EconomicalZoning.UNKNOWN)
            {
                cb.economicalZoning = EconomicalZoning.POOR;
                pm.cb_comPoor.Add(cb);
            }
        }

        List<CityBlock> allIndCB = new List<CityBlock>(pm.cb_allIndustrial);

        PopulationEditorTools.EconomicalZoningForCityBlocks(allIndCB, EconomicalZoning.POOR, pm.ecoPoor_Pct, 0, out zonedCBs);
        pm.cb_indPoor = zonedCBs;

        PopulationEditorTools.EconomicalZoningForCityBlocks(allIndCB, EconomicalZoning.MEDIUM, pm.ecoMedium_Pct, 0, out zonedCBs);
        pm.cb_indMedium = zonedCBs;

        PopulationEditorTools.EconomicalZoningForCityBlocks(allIndCB, EconomicalZoning.RICH, pm.ecoRich_Pct, 0, out zonedCBs);
        pm.cb_indRich = zonedCBs;

        //If for some reason we still have Industrial CityBlocks without EconomicalZoning, we set them to POOR.
        foreach (var cb in allIndCB)
        {
            if (cb.economicalZoning == EconomicalZoning.UNKNOWN)
            {
                cb.economicalZoning = EconomicalZoning.POOR;
                pm.cb_indPoor.Add(cb);
            }
        }
    }

    public static void S1C_CreateSpecialFeatures(PopulationManager pm)
    {
        ResourceManager rm = pm.gameManager.ResourceManager();
        MapManager mm = pm.gameManager.MapManager();
        CityManager cm = pm.gameManager.CityManager();

        List<LevelObject> featuresCreated = new List<LevelObject>();
        List<CityBlock> remainingCityBlocks = new List<CityBlock>();

        PopulationEditorTools.CreateMultipleBuildings(rm.pref_Hospital, pm.hospitalsToCreate, cm.cb_isSquare, pm.wrap_SpecialFeatures, mm.cliffHeight,
            out featuresCreated, out remainingCityBlocks);
        pm.co_specialFeatures.AddRange(featuresCreated);

        PopulationEditorTools.CreateMultipleBuildings(rm.pref_PoliceStation, pm.policeStationsToCreate, remainingCityBlocks, pm.wrap_SpecialFeatures, mm.cliffHeight,
            out featuresCreated, out remainingCityBlocks);
        pm.co_specialFeatures.AddRange(featuresCreated);

        PopulationEditorTools.CreateMultipleBuildings(rm.pref_Church, pm.churchesToCreate, remainingCityBlocks, pm.wrap_SpecialFeatures, mm.cliffHeight,
            out featuresCreated, out remainingCityBlocks);
        pm.co_specialFeatures.AddRange(featuresCreated);

        PopulationEditorTools.CreateMultipleBuildings(rm.pref_School, pm.schoolsToCreate, remainingCityBlocks, pm.wrap_SpecialFeatures, mm.cliffHeight,
            out featuresCreated, out remainingCityBlocks);
        pm.co_specialFeatures.AddRange(featuresCreated);

        PopulationEditorTools.CreateMultipleBuildings(rm.pref_SoccerStadium, pm.soccerStadiumsToCreate, remainingCityBlocks, pm.wrap_SpecialFeatures, mm.cliffHeight,
            out featuresCreated, out remainingCityBlocks);
        pm.co_specialFeatures.AddRange(featuresCreated);
    }

    public static void S1D_CreateCommonFeatures(PopulationManager pm)
    {
        ResourceManager rm = pm.gameManager.ResourceManager();
        MapManager mm = pm.gameManager.MapManager();
        CityManager cm = pm.gameManager.CityManager();

        List<LevelObject> featuresCreated = new List<LevelObject>();

        PopulationEditorTools.CreateMultipleBuildings(rm.prefs_ResPoor, pm.landUsage_Pct, pm.cb_resPoor, pm.wrap_Residences, mm.cliffHeight,
            out featuresCreated);
        pm.co_residences.AddRange(featuresCreated);

        PopulationEditorTools.CreateMultipleBuildings(rm.prefs_ResMedium, pm.landUsage_Pct, pm.cb_resMedium, pm.wrap_Residences, mm.cliffHeight,
            out featuresCreated);
        pm.co_residences.AddRange(featuresCreated);

        PopulationEditorTools.CreateMultipleBuildings(rm.prefs_ResRich, pm.landUsage_Pct, pm.cb_resRich, pm.wrap_Residences, mm.cliffHeight,
            out featuresCreated);
        pm.co_residences.AddRange(featuresCreated);

        PopulationEditorTools.CreateMultipleBuildings(rm.prefs_ComPoor, pm.landUsage_Pct, pm.cb_comPoor, pm.wrap_Commerces, mm.cliffHeight,
            out featuresCreated);
        pm.co_commerces.AddRange(featuresCreated);

        PopulationEditorTools.CreateMultipleBuildings(rm.prefs_ComMedium, pm.landUsage_Pct, pm.cb_comMedium, pm.wrap_Commerces, mm.cliffHeight,
            out featuresCreated);
        pm.co_commerces.AddRange(featuresCreated);

        PopulationEditorTools.CreateMultipleBuildings(rm.prefs_ComRich, pm.landUsage_Pct, pm.cb_comRich, pm.wrap_Commerces, mm.cliffHeight,
            out featuresCreated);
        pm.co_commerces.AddRange(featuresCreated);

        PopulationEditorTools.CreateMultipleBuildings(rm.prefs_IndPoor, pm.landUsage_Pct, pm.cb_indPoor, pm.wrap_Industries, mm.cliffHeight,
            out featuresCreated);
        pm.co_industries.AddRange(featuresCreated);

        PopulationEditorTools.CreateMultipleBuildings(rm.prefs_IndMedium, pm.landUsage_Pct, pm.cb_indMedium, pm.wrap_Industries, mm.cliffHeight,
            out featuresCreated);
        pm.co_industries.AddRange(featuresCreated);

        PopulationEditorTools.CreateMultipleBuildings(rm.prefs_IndRich, pm.landUsage_Pct, pm.cb_indRich, pm.wrap_Industries, mm.cliffHeight,
            out featuresCreated);
        pm.co_industries.AddRange(featuresCreated);
    }

    public static void S2_ResetPlayerStart(PopulationManager pm)
    {
        foreach (var item in pm.playerStartingLocations)
        {
            foreach (Tile t in item.occupiedTiles)
                t.building = null;
            Object.Destroy(item.gameObject);
        }
        pm.playerStartingLocations.Clear();
    }

    public static void S2_ApplyPlayerStart(PopulationManager pm)
    {
        ResourceManager rm = pm.gameManager.ResourceManager();
        MapManager mm = pm.gameManager.MapManager();
        //CityManager cm = pm.gameManager.CityManager();

        List<LevelObject> featuresCreated = new List<LevelObject>();
        List<CityBlock> remainingCityBlocks = new List<CityBlock>();

        PopulationEditorTools.CreateMultipleBuildings(rm.pref_PlayerStartLocation, pm.numberOfPlayers, pm.cb_notZoned, pm.wrap_SpecialFeatures, mm.cliffHeight,
            out featuresCreated, out remainingCityBlocks);
        pm.playerStartingLocations.AddRange(featuresCreated);
    }
}
