using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIEdWin_Menu : GUIComponent_Window
{
    public Button btn_Resume;
    public Button btn_SaveLevel;
    public Button btn_LoadLevel;
    public Button btn_SaveAndPlaytest;
    public Button btn_QuitEditor;

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

    public void BTN_Resume()
    {
        SMMode_Editor smme = screenManager.currentSMMode as SMMode_Editor;

        screenManager.gameManager.RESUME_GAME();
        smme.CloseAllWindows();
    }

    public void BTN_SaveLevel()
    {
        Debug.Log("FEATURE NOT YET IMPLEMENTED!");
    }

    public void BTN_LoadLevel()
    {
        Debug.Log("FEATURE NOT YET IMPLEMENTED!");
    }

    public void BTN_SaveAndPlaytest()
    {
        Debug.Log("\"BTN_SaveLevel\" NOT YET IMPLEMENTED!");

        screenManager.gameManager.ChangeUseMode(BPS.InGameUseMode.PLAY, true);
    }

    public void BTN_QuitEditor()
    {
        Debug.Log("FEATURE NOT YET IMPLEMENTED!");
    }
}
