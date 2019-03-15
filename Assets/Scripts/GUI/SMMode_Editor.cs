using BPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMMode_Editor : SMMode {
    public enum LevelEditorMode
    {
        NO_MODE,
        MAP_EDIT,
        CITY_EDIT,
        POPULATION_EDIT
    }
    public LevelEditorMode levelEditorMode;

    [Header("Panels")]
    public GUIEdPan_ImportantButtons guiEdPan_ImportantButtons;
    public GUIEdPan_LevelInformation guiEdPan_LevelInformation;
    public GUIEdPan_EditTypes guiEdPan_EditTypes;
    public GUIEdPan_EditOptions guiEdPan_EditOptions;
    public GUIEdPan_Minimap guiEdPan_Minimap;

    [Header("Windows")]
    public GUIEdWin_Menu guiEdWin_Menu;

    [Header("Execution Status")]
    public WindowCodes_Editor activeWindowCode;

    // Use this for initialization
    public override void Start () {
        base.Start();
        CloseAllWindows();
        ToogleMapEditor(false);
        ToogleCityEditor(false);
        TooglePopulationEditor(false);
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
    }

    public override bool CloseAllWindows()
    {
        guiEdWin_Menu.gameObject.SetActive(false);

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

        if (window == guiEdWin_Menu)
        {
            guiEdWin_Menu.gameObject.SetActive(true);
            currentWindow = guiEdWin_Menu;
            return true;
        }
        return false;
    }

    private void ToogleMapEditor(bool active)
    {
        guiEdPan_EditOptions.guiEdPan_EdOp_MapEdit.gameObject.SetActive(active);
    }

    private void ToogleCityEditor(bool active)
    {
        guiEdPan_EditOptions.guiEdPan_EdOp_CityEdit.gameObject.SetActive(active);
    }

    private void TooglePopulationEditor(bool active)
    {
        guiEdPan_EditOptions.guiEdPan_EdOp_PopulationEdit.gameObject.SetActive(active);
    }

    public void Mode_MapEdit()
    {
        ToogleCityEditor(false);
        TooglePopulationEditor(false);
        if (levelEditorMode == LevelEditorMode.MAP_EDIT)
        {
            ToogleMapEditor(false);
            levelEditorMode = LevelEditorMode.NO_MODE;
        }
        else
        {
            ToogleMapEditor(true);
            levelEditorMode = LevelEditorMode.MAP_EDIT;
            screenManager.gameManager.MapManager().CheckParameters();
            MapEdit_ReadParameters();
        }
    }

    public void Mode_CityEdit()
    {
        ToogleMapEditor(false);
        TooglePopulationEditor(false);
        if (levelEditorMode == LevelEditorMode.CITY_EDIT)
        {
            ToogleCityEditor(false);
            levelEditorMode = LevelEditorMode.NO_MODE;
        }
        else
        {
            ToogleCityEditor(true);
            levelEditorMode = LevelEditorMode.CITY_EDIT;
            screenManager.gameManager.CityManager().CheckParameters();
            CityEdit_ReadParameters();
        }
    }

    public void Mode_PopulationEdit()
    {
        ToogleMapEditor(false);
        ToogleCityEditor(false);
        if (levelEditorMode == LevelEditorMode.POPULATION_EDIT)
        {
            TooglePopulationEditor(false);
            levelEditorMode = LevelEditorMode.NO_MODE;
        }
        else
        {
            TooglePopulationEditor(true);
            levelEditorMode = LevelEditorMode.POPULATION_EDIT;
            screenManager.gameManager.PopulationManager().CheckParameters();
            PopulationEdit_ReadParameters();
        }
    }

    private void MapEdit_ReadParameters()
    {
        guiEdPan_EditOptions.guiEdPan_EdOp_MapEdit.guiEdPan_EdOp_MapEd_BaseValues.ReadParameters();
        guiEdPan_EditOptions.guiEdPan_EdOp_MapEdit.guiEdPan_EdOp_MapEd_Elevation.ReadParameters();
        guiEdPan_EditOptions.guiEdPan_EdOp_MapEdit.guiEdPan_EdOp_MapEd_Water.ReadParameters();
        guiEdPan_EditOptions.guiEdPan_EdOp_MapEdit.guiEdPan_EdOp_MapEd_Vegetation.ReadParameters();
    }

    private void CityEdit_ReadParameters()
    {
        guiEdPan_EditOptions.guiEdPan_EdOp_CityEdit.guiEdPan_EdOp_CityEd_BaseValues.ReadParameters();
        guiEdPan_EditOptions.guiEdPan_EdOp_CityEdit.guiEdPan_EdOp_CityEd_Accessibility.ReadParameters();
    }

    private void PopulationEdit_ReadParameters()
    {
        guiEdPan_EditOptions.guiEdPan_EdOp_PopulationEdit.guiEdPan_EdOp_PopEd_ZoningAndFeatures.ReadParameters();
        guiEdPan_EditOptions.guiEdPan_EdOp_PopulationEdit.guiEdPan_EdOp_PopEd_PlayerStart.ReadParameters();
    }
}
