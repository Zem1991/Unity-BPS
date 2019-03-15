using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIEdPan_ImportantButtons : GUIComponent_Panel
{
    public Button btn_Menu;
    public Button btn_Help;

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

    public void BTN_Menu()
    {
        SMMode_Editor smme = screenManager.currentSMMode as SMMode_Editor;

        screenManager.gameManager.PAUSE_GAME();
        smme.OpenWindow(smme.guiEdWin_Menu, true);
    }

    public void BTN_Help()
    {
        Debug.Log("FEATURE NOT YET IMPLEMENTED!");
    }
}
