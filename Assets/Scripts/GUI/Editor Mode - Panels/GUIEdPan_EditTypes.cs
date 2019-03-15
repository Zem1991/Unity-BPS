using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIEdPan_EditTypes : GUIComponent_Panel
{
    public Button btn_MapEdit;
    public Button btn_CityEdit;

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

    public void BTN_MapEdit()
    {
        SMMode_Editor smme = screenManager.currentSMMode as SMMode_Editor;
        smme.Mode_MapEdit();
    }

    public void BTN_CityEdit()
    {
        SMMode_Editor smme = screenManager.currentSMMode as SMMode_Editor;
        smme.Mode_CityEdit();
    }

    public void BTN_PopulationEdit()
    {
        SMMode_Editor smme = screenManager.currentSMMode as SMMode_Editor;
        smme.Mode_PopulationEdit();
    }
}
