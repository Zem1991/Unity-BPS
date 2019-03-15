using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIEdPan_EdOp_PopEd_ZoningAndFeatures : GUIComponent_Panel
{
    [Header("[1a] Purpose Zoning")]
    public InputField inp_ZonedBlocksPct;
    public InputField inp_LandUsagePct;
    public InputField inp_ResidentialPct;
    public InputField inp_CommercialPct;
    public InputField inp_IndustrialPct;
    [Header("[1b] Economic Zoning")]
    public InputField inp_ecoPoorPct;
    public InputField inp_ecoMediumPct;
    public InputField inp_ecoRichPct;
    [Header("[1c] Special Features")]
    public InputField inp_healthPct;
    public InputField inp_crimePct;
    public InputField inp_religionPct;
    public InputField inp_educationPct;
    public InputField inp_soccerFanaticismPct;
    public InputField inp_politicalIdeologyPct;
    [Header("Buttons")]
    public Button btn_UseDefaultValues;
    public Button btn_CountFeatures;
    public Button btn_Apply;
    public Button btn_Reset;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public void ReadParameters()
    {
        if (!screenManager)
            screenManager = GetComponentInParent<ScreenManager>();

        PopulationManager pm = screenManager.gameManager.PopulationManager();

        inp_ZonedBlocksPct.text = pm.zonedBlocks_Pct.ToString();
        inp_LandUsagePct.text = pm.landUsage_Pct.ToString();
        inp_ResidentialPct.text = pm.residential_Pct.ToString();
        inp_CommercialPct.text = pm.commercial_Pct.ToString();
        inp_IndustrialPct.text = pm.industrial_Pct.ToString();

        inp_ecoPoorPct.text = pm.ecoPoor_Pct.ToString();
        inp_ecoMediumPct.text = pm.ecoMedium_Pct.ToString();
        inp_ecoRichPct.text = pm.ecoRich_Pct.ToString();

        inp_healthPct.text = pm.health_Pct.ToString();
        inp_crimePct.text = pm.crime_Pct.ToString();
        inp_religionPct.text = pm.religion_Pct.ToString();
        inp_educationPct.text = pm.education_Pct.ToString();
        inp_soccerFanaticismPct.text = pm.soccerFanaticism_Pct.ToString();
        inp_politicalIdeologyPct.text = pm.politicalIdeology_Pct.ToString();
    }

    public void WriteParameters()
    {
        PopulationManager pm = screenManager.gameManager.PopulationManager();

        pm.zonedBlocks_Pct = int.Parse(inp_ZonedBlocksPct.text);
        pm.landUsage_Pct = int.Parse(inp_LandUsagePct.text);
        pm.residential_Pct = int.Parse(inp_ResidentialPct.text);
        pm.commercial_Pct = int.Parse(inp_CommercialPct.text);
        pm.industrial_Pct = int.Parse(inp_IndustrialPct.text);

        pm.ecoPoor_Pct = int.Parse(inp_ecoPoorPct.text);
        pm.ecoMedium_Pct = int.Parse(inp_ecoMediumPct.text);
        pm.ecoRich_Pct = int.Parse(inp_ecoRichPct.text);

        pm.health_Pct = int.Parse(inp_healthPct.text);
        pm.crime_Pct = int.Parse(inp_crimePct.text);
        pm.religion_Pct = int.Parse(inp_religionPct.text);
        pm.education_Pct = int.Parse(inp_educationPct.text);
        pm.soccerFanaticism_Pct = int.Parse(inp_soccerFanaticismPct.text);
        pm.politicalIdeology_Pct = int.Parse(inp_politicalIdeologyPct.text);

        pm.CheckParameters();
        ReadParameters();
    }

    public void BTN_UseDefaultValues()
    {
        Debug.Log("FEATURE NOIT YET IMPLEMENTED!");
    }

    public void BTN_CountFeatures()
    {
        Debug.Log("FEATURE NOIT YET IMPLEMENTED!");
    }

    public void BTN_Apply()
    {
        PopulationManager pm = screenManager.gameManager.PopulationManager();
        pm.Step1_ApplyZoningAndFeatures();
    }

    public void BTN_Reset()
    {
        PopulationManager pm = screenManager.gameManager.PopulationManager();
        pm.Step1_ResetZoningAndFeatures();
    }
}
