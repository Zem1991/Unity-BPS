using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlWin_Units : GUIComponent_Window
{
    public Button btn_Return;

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

    public void BTN_Return()
    {
        SMMode_Play smmp = screenManager.currentSMMode as SMMode_Play;
        smmp.CloseAllWindows();
    }
}
