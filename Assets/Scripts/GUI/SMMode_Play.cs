using BPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMMode_Play : SMMode {
    //public enum LevelEditorMode
    //{
    //    NO_MODE,
    //    MAP_EDIT,
    //    CITY_EDIT,
    //    ZONING_EDIT,
    //    POPULATION_EDIT
    //}
    //public LevelEditorMode levelEditorMode;

    [Header("Panels")]
    public GUIPlPan_ImportantButtons guiPlPan_ImportantButtons;
    public GUIPlPan_PlayerInformation guiPlPan_PlayerInformation;
    public GUIPlPan_Timer guiPlPan_Timer;
    public GUIPlPan_Statistics guiPlPan_Statistics;
    public GUIPlPan_PlayerSelection guiPlPan_PlayerSelection;
    public GUIPlPan_QuickSelections guiPlPan_QuickSelections;
    public GUIPlPan_Minimap guiPlPan_Minimap;

    [Header("Windows")]
    public GUIPlWin_Menu guiPlWin_Menu;
    public GUIPlWin_Help guiPlWin_Help;
    public GUIPlWin_PoliticalCampaign guiPlWin_PoliticalCampaign;
    public GUIPlWin_Buildings guiPlWin_Buildings;
    public GUIPlWin_Units guiPlWin_Units;
    public GUIPlWin_Undesirables guiPlWin_Undesirables;
    public GUIPlWin_Corruption guiPlWin_Corruption;
    public GUIPlWin_Finances guiPlWin_Finances;
    public GUIPlWin_Events guiPlWin_Events;

    [Header("Feedback")]
    public SMMode_Play_Feedback smPlFeedback;

    [Header("Execution Status")]
    public WindowCodes_Play activeWindowCode;

    public PlayerManager playerManager;

    // Use this for initialization
    public override void Start () {
        base.Start();
        CloseAllWindows();
        smPlFeedback.ClearEverything();

        playerManager = screenManager.gameManager.PlayerManager();
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();

        UpdateSelectionPanels();
    }

    public override bool CloseAllWindows()
    {
        guiPlWin_Menu.gameObject.SetActive(false);
        guiPlWin_Help.gameObject.SetActive(false);
        guiPlWin_PoliticalCampaign.gameObject.SetActive(false);
        guiPlWin_Buildings.gameObject.SetActive(false);
        guiPlWin_Units.gameObject.SetActive(false);
        guiPlWin_Undesirables.gameObject.SetActive(false);
        guiPlWin_Corruption.gameObject.SetActive(false);
        guiPlWin_Finances.gameObject.SetActive(false);
        guiPlWin_Events.gameObject.SetActive(false);

        currentWindow = null;
        GameManager gm = screenManager.gameManager;
        if (gm)
            gm.InputManager().focusedWindow = null;
        return true;
    }

    public override bool OpenWindow(GUIComponent_Window window, bool closeIfAlreadyOpen)
    {
        //Doesn't need to do anything if the desired window is already open. Maybe just close it.
        if (currentWindow == window)
        {
            if (closeIfAlreadyOpen)
                CloseAllWindows();
            return true;
        }

        //Yes, close ALL windows. I know there will be only one active window at any time, but now I am lazy.
        CloseAllWindows();

        if (window == guiPlWin_Menu)
        {
            guiPlWin_Menu.gameObject.SetActive(true);
            currentWindow = guiPlWin_Menu;
            return true;
        }
        if (window == guiPlWin_Help)
        {
            guiPlWin_Help.gameObject.SetActive(true);
            currentWindow = guiPlWin_Help;
            return true;
        }
        if (window == guiPlWin_PoliticalCampaign)
        {
            guiPlWin_PoliticalCampaign.gameObject.SetActive(true);
            currentWindow = guiPlWin_PoliticalCampaign;
            return true;
        }
        if (window == guiPlWin_Buildings)
        {
            guiPlWin_Buildings.gameObject.SetActive(true);
            currentWindow = guiPlWin_Buildings;
            return true;
        }
        if (window == guiPlWin_Units)
        {
            guiPlWin_Units.gameObject.SetActive(true);
            currentWindow = guiPlWin_Units;
            return true;
        }
        if (window == guiPlWin_Undesirables)
        {
            guiPlWin_Undesirables.gameObject.SetActive(true);
            currentWindow = guiPlWin_Undesirables;
            return true;
        }
        if (window == guiPlWin_Corruption)
        {
            guiPlWin_Corruption.gameObject.SetActive(true);
            currentWindow = guiPlWin_Corruption;
            return true;
        }
        if (window == guiPlWin_Finances)
        {
            guiPlWin_Finances.gameObject.SetActive(true);
            currentWindow = guiPlWin_Finances;
            return true;
        }
        if (window == guiPlWin_Events)
        {
            guiPlWin_Events.gameObject.SetActive(true);
            currentWindow = guiPlWin_Events;
            return true;
        }
        return false;
    }

    private void UpdateSelectionPanels()
    {
        Player p = playerManager.localPlayer;
        if (!p)
        {
            guiPlPan_PlayerSelection.ClearInformations();
            return;
        }

        List<PlayerObject> pObjects = p.pSelection.selectedPlayerObjects;
        List<PlayerObject> relevantPOs = p.pSelection.relevantSelectedPOs;
        CityObject cObject = p.pSelection.selectedCityObject;

        if (pObjects.Count > 0 && relevantPOs.Count > 0)
        {
            guiPlPan_PlayerSelection.ChangeInformations(pObjects, relevantPOs[0], p);
        }
        else if (cObject)
        {
            guiPlPan_PlayerSelection.ChangeInformations(cObject);
        }
        else
        {
            guiPlPan_PlayerSelection.ClearInformations();
        }
    }
}
