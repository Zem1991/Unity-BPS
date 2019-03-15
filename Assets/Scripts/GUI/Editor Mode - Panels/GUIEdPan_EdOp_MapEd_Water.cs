using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIEdPan_EdOp_MapEd_Water : GUIComponent_Panel
{
    public InputField inp_RiversToCreate;
    public InputField inp_MaxRiverLength;
    public InputField inp_MaxRiverTiles;
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
        inp_RiversToCreate.text = mm.riversToCreate.ToString();
        inp_MaxRiverLength.text = mm.maxRiverLength.ToString();
        inp_MaxRiverTiles.text = mm.maxRiverTiles.ToString();
    }

    public void WriteParameters()
    {
        MapManager mm = screenManager.gameManager.MapManager();
        mm.riversToCreate = int.Parse(inp_RiversToCreate.text);
        mm.maxRiverLength = int.Parse(inp_MaxRiverLength.text);
        mm.maxRiverTiles = int.Parse(inp_MaxRiverTiles.text);

        mm.CheckParameters();
        ReadParameters();
    }

    public void BTN_Reset()
    {
        MapManager mm = screenManager.gameManager.MapManager();
        mm.Step3_ResetWater();
    }

    public void BTN_Apply()
    {
        MapManager mm = screenManager.gameManager.MapManager();
        mm.Step3_ApplyWater();
    }
}
