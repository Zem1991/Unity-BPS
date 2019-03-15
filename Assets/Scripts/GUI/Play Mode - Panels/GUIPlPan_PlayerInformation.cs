using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlPan_PlayerInformation : GUIComponent_Panel
{
    public GUIPlPan_PlayerInformation_Assets guiPlPan_PlInfo_Assets;
    public GUIPlPan_PlayerInformation_Corruption guiPlPan_PlInfo_Corruption;
    public GUIPlPan_PlayerInformation_Resources guiPlPan_PlInfo_Resources;

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

    public void BTN_PoliticalCampaign()
    {
        SMMode_Play smmp = screenManager.currentSMMode as SMMode_Play;
        smmp.OpenWindow(smmp.guiPlWin_PoliticalCampaign, true);
    }

    public void BTN_Buildings()
    {
        SMMode_Play smmp = screenManager.currentSMMode as SMMode_Play;
        smmp.OpenWindow(smmp.guiPlWin_Buildings, true);
    }

    public void BTN_Units()
    {
        SMMode_Play smmp = screenManager.currentSMMode as SMMode_Play;
        smmp.OpenWindow(smmp.guiPlWin_Units, true);
    }

    public void BTN_Undesirables()
    {
        SMMode_Play smmp = screenManager.currentSMMode as SMMode_Play;
        smmp.OpenWindow(smmp.guiPlWin_Undesirables, true);
    }

    public void BTN_Corruption()
    {
        SMMode_Play smmp = screenManager.currentSMMode as SMMode_Play;
        smmp.OpenWindow(smmp.guiPlWin_Corruption, true);
    }

    public void BTN_Finances()
    {
        SMMode_Play smmp = screenManager.currentSMMode as SMMode_Play;
        smmp.OpenWindow(smmp.guiPlWin_Finances, true);
    }
}
