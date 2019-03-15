using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIEdPan_EdOp_PopEd_PlayerStart : GUIComponent_Panel
{
    public InputField inp_numberOfPlayers;
    public InputField inp_minDistance;
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
        inp_numberOfPlayers.text = pm.numberOfPlayers.ToString();
        inp_minDistance.text = pm.minDistance.ToString();
    }

    public void WriteParameters()
    {
        PopulationManager pm = screenManager.gameManager.PopulationManager();
        pm.numberOfPlayers = int.Parse(inp_numberOfPlayers.text);
        pm.minDistance = int.Parse(inp_minDistance.text);

        pm.CheckParameters();
        ReadParameters();
    }

    public void BTN_Apply()
    {
        PopulationManager pm = screenManager.gameManager.PopulationManager();
        pm.Step2_ApplyPlayerStart();
    }

    public void BTN_Reset()
    {
        PopulationManager pm = screenManager.gameManager.PopulationManager();
        pm.Step2_ResetPlayerStart();
    }
}
