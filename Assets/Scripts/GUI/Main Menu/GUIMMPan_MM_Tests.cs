using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIMMPan_MM_Tests : GUIComponent_Panel
{
    public enum tests
    {
        NO_TEST,
        CAMERA_CONTROLS,
        SELECTION_AND_MOVEMENT,
        COMBAT,
        CONSTRUCTION_MANAGEMENT,
        LEVEL_GENERATION,
        ELECTIONS_STATISTICS,
        ONE_VS_ONE_WITH_10_MINUTES,
        FREE_BUILD
    }
    public tests selectedTest;

    public Button btn_CameraControls;
    public Button btn_SelectionAndMovement;
    public Button btn_Combat;
    public Button btn_ConstructionManagement;
    public Button btn_CityGeneration;
    public Button btn_ElectionsStatistics;
    public Button btn_1v1With10Minutes;
    public Button btn_FreeBuild;
    public Button btn_Play;

    public GUIMMPan_MainMenu mainMenu;

    // Use this for initialization
    public override void Start () {
        base.Start();

        resetButtons();
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
    }

    public void resetButtons()
    {
        foreach (Button item in GetComponentsInChildren<Button>())
        {
            if (item != btn_Play)
                item.interactable = true;
            else
                item.interactable = false;
        }
        selectedTest = tests.NO_TEST;
    }

    public void button_CameraControls()
    {
        resetButtons();
        selectedTest = tests.CAMERA_CONTROLS;
        btn_CameraControls.interactable = false;
        btn_Play.interactable = true;
    }

    public void button_SelectionAndMovement()
    {
        resetButtons();
        selectedTest = tests.SELECTION_AND_MOVEMENT;
        btn_SelectionAndMovement.interactable = false;
        btn_Play.interactable = true;
    }

    public void button_Combat()
    {
        resetButtons();
        selectedTest = tests.COMBAT;
        btn_Combat.interactable = false;
        btn_Play.interactable = true;
    }

    public void button_ConstructionManagement()
    {
        resetButtons();
        selectedTest = tests.CONSTRUCTION_MANAGEMENT;
        btn_ConstructionManagement.interactable = false;
        btn_Play.interactable = true;
    }

    public void button_CityGeneration()
    {
        resetButtons();
        selectedTest = tests.LEVEL_GENERATION;
        btn_CityGeneration.interactable = false;
        btn_Play.interactable = true;
    }

    public void button_ElectionsStatistics()
    {
        resetButtons();
        selectedTest = tests.ELECTIONS_STATISTICS;
        btn_ElectionsStatistics.interactable = false;
        btn_Play.interactable = true;
    }

    public void button_1v1With10Minutes()
    {
        resetButtons();
        selectedTest = tests.ONE_VS_ONE_WITH_10_MINUTES;
        btn_1v1With10Minutes.interactable = false;
        btn_Play.interactable = true;
    }

    public void button_FreeBuild()
    {
        resetButtons();
        selectedTest = tests.FREE_BUILD;
        btn_FreeBuild.interactable = false;
        btn_Play.interactable = true;
    }

    public void button_Play()
    {
        switch (selectedTest)
        {
            case tests.CAMERA_CONTROLS:
                SceneManager.LoadScene("In Game");
                break;
            case tests.SELECTION_AND_MOVEMENT:
                SceneManager.LoadScene("In Game");
                break;
            case tests.COMBAT:
                SceneManager.LoadScene("In Game");
                break;
            case tests.CONSTRUCTION_MANAGEMENT:
                SceneManager.LoadScene("In Game");
                break;
            case tests.LEVEL_GENERATION:
                SceneManager.LoadScene("In Game");
                break;
            case tests.ELECTIONS_STATISTICS:
                SceneManager.LoadScene("In Game");
                break;
            case tests.ONE_VS_ONE_WITH_10_MINUTES:
                SceneManager.LoadScene("In Game");
                break;
            case tests.FREE_BUILD:
                SceneManager.LoadScene("In Game");
                break;
            default:
                break;
        }
        //TODO!
        Application.Quit();
    }

    public void button_Return()
    {
        mainMenu.cameraZoomOut();
    }
}
