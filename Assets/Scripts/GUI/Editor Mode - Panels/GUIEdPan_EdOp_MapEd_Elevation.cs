using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIEdPan_EdOp_MapEd_Elevation : GUIComponent_Panel
{
    public InputField inp_BlobsToRaise;
    public InputField inp_BlobsToLower;
    public InputField inp_InitialSpread;
    public InputField inp_SpreadDecay;
    public InputField inp_CliffCoveragePct;
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
        inp_BlobsToRaise.text = mm.elevBlobsToRaise.ToString();
        inp_BlobsToLower.text = mm.elevBlobsToLower.ToString();
        inp_InitialSpread.text = mm.elevBlobInitialSpread.ToString();
        inp_SpreadDecay.text = mm.elevBlobSpreadDecay.ToString();
        inp_CliffCoveragePct.text = mm.cliffCoverage_Pct.ToString();
    }

    public void WriteParameters()
    {
        MapManager mm = screenManager.gameManager.MapManager();
        mm.elevBlobsToRaise = int.Parse(inp_BlobsToRaise.text);
        mm.elevBlobsToLower = int.Parse(inp_BlobsToLower.text);
        mm.elevBlobInitialSpread = int.Parse(inp_InitialSpread.text);
        mm.elevBlobSpreadDecay = int.Parse(inp_SpreadDecay.text);
        mm.cliffCoverage_Pct = int.Parse(inp_CliffCoveragePct.text);

        mm.CheckParameters();
        ReadParameters();
    }

    public void BTN_Reset()
    {
        MapManager mm = screenManager.gameManager.MapManager();
        mm.Step2_ResetElevation();
    }

    public void BTN_Apply()
    {
        MapManager mm = screenManager.gameManager.MapManager();
        mm.Step2_ApplyElevation();
    }
}
