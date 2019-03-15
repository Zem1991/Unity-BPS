using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIMMPan_MM_Singleplayer : GUIComponent_Panel
{
    public Button btn_Overview;
    public Button btn_Tutorials;
    public Button btn_HelpTopics;

    public GUIMMPan_MainMenu mainMenu;

    private HowToPlay_Overview gui_overview;
    private HowToPlay_Tutorials gui_tutorials;
    private HowToPlay_HelpTopics gui_helpTopics;

    // Use this for initialization
    public override void Start () {
        base.Start();

        gui_overview = GetComponentInChildren<HowToPlay_Overview>();
        gui_tutorials = GetComponentInChildren<HowToPlay_Tutorials>();
        gui_helpTopics = GetComponentInChildren<HowToPlay_HelpTopics>();

        button_Overview();
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
    }

    public void button_Overview()
    {
        gui_overview.gameObject.SetActive(true);
        gui_tutorials.gameObject.SetActive(false);
        gui_helpTopics.gameObject.SetActive(false);
        btn_Overview.interactable = false;
        btn_Tutorials.interactable = true;
        btn_HelpTopics.interactable = true;
    }

    public void button_Tutorials()
    {
        gui_overview.gameObject.SetActive(false);
        gui_tutorials.gameObject.SetActive(true);
        gui_helpTopics.gameObject.SetActive(false);
        btn_Overview.interactable = true;
        btn_Tutorials.interactable = false;
        btn_HelpTopics.interactable = true;
    }

    public void button_HelpTopics()
    {
        gui_overview.gameObject.SetActive(false);
        gui_tutorials.gameObject.SetActive(false);
        gui_helpTopics.gameObject.SetActive(true);
        btn_Overview.interactable = true;
        btn_Tutorials.interactable = true;
        btn_HelpTopics.interactable = false;
    }

    public void button_Return()
    {
        mainMenu.cameraZoomOut();
    }
}
