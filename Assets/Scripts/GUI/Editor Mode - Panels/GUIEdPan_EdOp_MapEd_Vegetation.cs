using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIEdPan_EdOp_MapEd_Vegetation : GUIComponent_Panel
{
    public InputField inp_VegBlobs;
    public InputField inp_InitialSpread;
    public InputField inp_SpreadDecay;
    public InputField inp_TreeCoveragePct;
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

        MapManager mm = screenManager.gameManager.MapManager();
        inp_VegBlobs.text = mm.vegBlobsToCreate.ToString();
        inp_InitialSpread.text = mm.vegBlobInitialSpread.ToString();
        inp_SpreadDecay.text = mm.vegBlobSpreadDecay.ToString();
        inp_TreeCoveragePct.text = mm.treeCoverage_Pct.ToString();
    }

    public void WriteParameters()
    {
        MapManager mm = screenManager.gameManager.MapManager();
        mm.vegBlobsToCreate = int.Parse(inp_VegBlobs.text);
        mm.vegBlobInitialSpread = int.Parse(inp_InitialSpread.text);
        mm.vegBlobSpreadDecay = int.Parse(inp_SpreadDecay.text);
        mm.treeCoverage_Pct = int.Parse(inp_TreeCoveragePct.text);

        mm.CheckParameters();
        ReadParameters();
    }

    public void BTN_Reset()
    {
        MapManager mm = screenManager.gameManager.MapManager();
        mm.Step4_ResetVegetation();
    }

    public void BTN_Apply()
    {
        MapManager mm = screenManager.gameManager.MapManager();
        mm.Step4_ApplyVegetation();
    }
}
