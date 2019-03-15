using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlPan_PlayerSelection_POCommandCard : GUIComponent_Panel
{
    public GUIPlPan_PlSel_POCC_AvailableActions gUIPlPan_PlSel_POCC_AvailableActions;
    public GUIPlPan_PlSel_POCC_SelectedAction gUIPlPan_PlSel_POCC_SelectedAction;

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

    public void SetData(PlayerObject po, Player p)
    {
        if (!p.candidateAction)
        {
            gUIPlPan_PlSel_POCC_SelectedAction.gameObject.SetActive(false);
            gUIPlPan_PlSel_POCC_SelectedAction.ClearData();

            gUIPlPan_PlSel_POCC_AvailableActions.SetData(po);
            gUIPlPan_PlSel_POCC_AvailableActions.gameObject.SetActive(true);
        }
        else
        {
            gUIPlPan_PlSel_POCC_AvailableActions.gameObject.SetActive(false);
            gUIPlPan_PlSel_POCC_AvailableActions.ClearData();

            gUIPlPan_PlSel_POCC_SelectedAction.SetData(p.candidateAction);
            gUIPlPan_PlSel_POCC_SelectedAction.gameObject.SetActive(true);
        }
    }

    public void ClearData()
    {
        gUIPlPan_PlSel_POCC_AvailableActions.ClearData();
        gUIPlPan_PlSel_POCC_SelectedAction.ClearData();
    }
}
