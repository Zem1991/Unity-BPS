using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlPan_ImportantButtons : GUIComponent_Panel
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
        SMMode_Play smmp = screenManager.currentSMMode as SMMode_Play;

        screenManager.gameManager.PAUSE_GAME();
        smmp.OpenWindow(smmp.guiPlWin_Menu, true);
    }

    public void BTN_Help()
    {
        SMMode_Play smmp = screenManager.currentSMMode as SMMode_Play;
        smmp.OpenWindow(smmp.guiPlWin_Help, true);
    }
}
