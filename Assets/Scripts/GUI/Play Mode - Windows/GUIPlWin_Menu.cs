using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlWin_Menu : GUIComponent_Window
{
    public Button btn_Resume;
    public Button btn_Save;
    public Button btn_Load;
    public Button btn_Options;
    public Button btn_Quit;

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
        SMMode_Play smmp = screenManager.currentSMMode as SMMode_Play;

        screenManager.gameManager.RESUME_GAME();
        smmp.CloseAllWindows();
    }

    public void BTN_Save()
    {
        Debug.Log("FEATURE NOT YET IMPLEMENTED!");
    }

    public void BTN_Load()
    {
        Debug.Log("FEATURE NOT YET IMPLEMENTED!");
    }

    public void BTN_Options()
    {
        Debug.Log("FEATURE NOT YET IMPLEMENTED!");
    }

    public void BTN_Quit()
    {
        Debug.Log("FEATURE NOT YET IMPLEMENTED!");

        if (screenManager.gameManager.playTesting)
        {
            screenManager.gameManager.ChangeUseMode(BPS.InGameUseMode.EDITOR, screenManager.gameManager.playTesting);
        }
        else
        {
            screenManager.gameManager.QUIT_GAME();
        }
    }
}
