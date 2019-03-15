using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlWin_Buildings : GUIComponent_Window
{
    public Button btn_Return;

    [Header("Building Buttons")]
    public Button btn_hq_or_advHq;
    public Button btn_printShop;
    //public Button btn_hq_or_advHq;
    //public Button btn_hq_or_advHq;
    //public Button btn_hq_or_advHq;
    //public Button btn_hq_or_advHq;
    //public Button btn_hq_or_advHq;
    //public Button btn_hq_or_advHq;
    //public Button btn_hq_or_advHq;
    //public Button btn_hq_or_advHq;
    //public Button btn_hq_or_advHq;
    //public Button btn_hq_or_advHq;
    //public Button btn_hq_or_advHq;

    [Header("Building Descriptors")]
    public Text txt_description;

    [Header("Context Buttons")]
    public Button btn_build_or_demolish;
    public Button btn_goTo;

    [Header("Execution Variables")]
    public Player localPlayer;
    public PO_Building currentBuilding;
    public int currentBuildingId;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (!localPlayer)
        {
            localPlayer = screenManager.gameManager.playerManager.localPlayer;
            BTN_BuildingBtn(0);
        }
    }

    public void BTN_Return()
    {
        SMMode_Play smmp = screenManager.currentSMMode as SMMode_Play;
        smmp.CloseAllWindows();
    }

    public void BTN_BuildingBtn(int id)
    {
        currentBuildingId = id;

        switch (id)
        {
            case 0:
                if (localPlayer.pBelongings.bldg_PartyHeadquarters == localPlayer.faction.bldg_advanced_hq)
                    currentBuilding = localPlayer.faction.bldg_advanced_hq;
                else
                    currentBuilding = localPlayer.faction.bldg_basic_hq;
                break;
            case 1:
                currentBuilding = localPlayer.faction.bldg_printShop;
                break;
        }

        UpdateWithCurrentBuilding();
    }

    public void BTN_BuildOrDemolish()
    {
        SMMode_Play smmp = screenManager.currentSMMode as SMMode_Play;
        smmp.CloseAllWindows();

        GameManager gm = screenManager.gameManager;
        Player localPlayer = gm.playerManager.localPlayer;
        PlayerMouseController pmc = localPlayer.pMouse;
        localPlayer.pBelongings.createTemporaryBuilding(currentBuilding, pmc.mouseScenePosition, gm.defaultRotation, gm.ResourceManager().mat_Construction_Denied);
    }

    public void BTN_GoTo()
    {
        SMMode_Play smmp = screenManager.currentSMMode as SMMode_Play;
        smmp.CloseAllWindows();

        Player localPlayer = screenManager.gameManager.playerManager.localPlayer;
        if (!localPlayer)
            return;

        Debug.Log("NOT IMPLEMENTED!");
    }

    private void UpdateWithCurrentBuilding()
    {
        if (currentBuildingId == 0)
        {
            btn_build_or_demolish.GetComponentInChildren<Text>().text = "Upgrade";
            btn_build_or_demolish.interactable = (currentBuilding == localPlayer.faction.bldg_basic_hq);
            btn_goTo.interactable = true;
        }
        else
        {
            btn_build_or_demolish.GetComponentInChildren<Text>().text = "Build";
            btn_goTo.interactable = false;
            foreach (var item in localPlayer.pBelongings.allBuildings)
            {
                if (currentBuilding == item)
                {
                    btn_build_or_demolish.GetComponentInChildren<Text>().text = "Demolish";
                    btn_goTo.interactable = true;
                    break;
                }
            }
        }

        txt_description.text = currentBuilding.levelObjectName;
    }
}
