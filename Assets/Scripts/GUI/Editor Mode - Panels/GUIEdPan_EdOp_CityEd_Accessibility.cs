using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIEdPan_EdOp_CityEd_Accessibility : GUIComponent_Panel
{
    public InputField inp_rampsPct;
    public InputField inp_brigesPct;
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

        CityManager cm = screenManager.gameManager.CityManager();
        inp_rampsPct.text = cm.rampsCoverage_Pct.ToString();
        inp_brigesPct.text = cm.bridgesCoverage_Pct.ToString();
    }

    public void WriteParameters()
    {
        CityManager cm = screenManager.gameManager.CityManager();
        cm.rampsCoverage_Pct = int.Parse(inp_rampsPct.text);
        cm.bridgesCoverage_Pct = int.Parse(inp_brigesPct.text);

        cm.CheckParameters();
        ReadParameters();
    }

    public void BTN_Apply()
    {
        CityManager cm = screenManager.gameManager.CityManager();
        cm.Step2_ApplyAccessibility();
    }

    public void BTN_Reset()
    {
        CityManager cm = screenManager.gameManager.CityManager();
        cm.Step2_ResetAccessibility();
    }
}
