using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIEdPan_EdOp_CityEd_BaseValues : GUIComponent_Panel
{
    public InputField inp_CityName;
    public InputField inp_RoadsOffset;
    public InputField inp_CityBlockSize;
    public Button btn_RandomName;
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
        inp_CityName.text = cm.cityName;
        inp_RoadsOffset.text = cm.roadsOffset.ToString();
        inp_CityBlockSize.text = cm.cityBlockSize.ToString();
    }

    public void WriteParameters()
    {
        CityManager cm = screenManager.gameManager.CityManager();
        cm.cityName = inp_CityName.text;
        cm.roadsOffset = int.Parse(inp_RoadsOffset.text);
        cm.cityBlockSize = int.Parse(inp_CityBlockSize.text);

        cm.CheckParameters();
        ReadParameters();
    }

    public void BTN_RandomName()
    {
        Debug.Log("FEATURE NOT YET IMPLEMENTED!");
    }

    public void BTN_Apply()
    {
        CityManager cm = screenManager.gameManager.CityManager();
        cm.Step1_ApplyBaseValues();
    }

    public void BTN_Reset()
    {
        CityManager cm = screenManager.gameManager.CityManager();
        cm.Step1_ResetBaseValues();
    }
}
