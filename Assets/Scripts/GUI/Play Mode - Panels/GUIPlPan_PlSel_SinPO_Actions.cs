using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlPan_PlSel_SinPO_Actions : GUIComponent_Panel
{
    public Button btn_currentAction;
    public Text txt_currentActionProgress;
    public Image img_currentActionProgressBar;
    public Button btn_nextAction01, btn_nextAction02, btn_nextAction03, btn_nextAction04, btn_nextAction05;
    private Button[] actionButtons = new Button[6];

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        actionButtons[0] = btn_currentAction;
        actionButtons[1] = btn_nextAction01;
        actionButtons[2] = btn_nextAction02;
        actionButtons[3] = btn_nextAction03;
        actionButtons[4] = btn_nextAction04;
        actionButtons[5] = btn_nextAction05;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public void SetData(PlayerObject po)
    {
        ResourceManager rm = screenManager.gameManager.ResourceManager();

        PO_Unit loAsUnit = po as PO_Unit;     //if (loAsUnit)     ???
        Action currentAction = po.currentAction;
        List<Action> actionList = po.actionList;

        if (currentAction)
        {
            actionButtons[0].image.sprite = currentAction.img_icon;
            actionButtons[0].interactable = true;

            txt_currentActionProgress.text = "Progess: ???%";
            img_currentActionProgressBar.sprite = null;
        }
        else
        {
            actionButtons[0].image.sprite = rm.icon_NoIcon;
            actionButtons[0].interactable = false;

            txt_currentActionProgress.text = "No activity.";
            img_currentActionProgressBar.sprite = null;
        }

        for (int i = 1; i < actionButtons.Length; i++)
        {
            if (!Equals(actionList, null) && i <= actionList.Count)
            {
                actionButtons[i].image.sprite = actionList[i - 1].img_icon;
                actionButtons[i].interactable = true;
            }
            else
            {
                actionButtons[i].image.sprite = rm.icon_NoIcon;
                actionButtons[i].interactable = false;
            }
        }
    }

    public void ClearData()
    {
        ResourceManager rm = screenManager.gameManager.ResourceManager();

        txt_currentActionProgress.text = null;
        img_currentActionProgressBar.sprite = null;

        for (int i = 0; i < actionButtons.Length; i++)
        {
            actionButtons[i].image.sprite = rm.icon_NoIcon;
            actionButtons[i].interactable = false;
        }
    }
}
