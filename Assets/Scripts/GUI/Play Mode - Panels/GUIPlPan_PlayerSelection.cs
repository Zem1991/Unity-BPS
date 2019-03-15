using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlPan_PlayerSelection : GUIComponent_Panel
{
    public GUIPlPan_PlayerSelection_SingularPlayerObject guiPlPan_PlSel_SingularPO;
    public GUIPlPan_PlayerSelection_PluralPlayerObject guiPlPan_PlSel_PluralPO;
    public GUIPlPan_PlayerSelection_POCommandCard guiPlPan_PlSel_POCommandCard;
    public GUIPlPan_PlayerSelection_SingularCityObject guiPlPan_PlSel_SingularCO;
    public GUIPlPan_PlayerSelection_COControl guiPlPan_PlSel_COControl;

    private List<PlayerObject> playerObjList;
    private CityObject cityObj;

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

    public void ChangeInformations(List<PlayerObject> pObjects, PlayerObject relevantPO, Player p)
    {
        ClearInformationsCO();

        playerObjList = pObjects;
        cityObj = null;

        if (playerObjList.Count == 1)
            SetInformationsSingularPO(relevantPO, p);
        else if (playerObjList.Count > 1)
            SetInformationsPluralPO(pObjects, relevantPO, p);
        else
            ClearInformationsPO();
    }

    public void ChangeInformations(CityObject cObject)
    {
        ClearInformationsPO();

        playerObjList = null;
        cityObj = cObject;

        if (cityObj)
            SetInformationsCO(cObject);
        else
            ClearInformationsCO();
    }

    public void ClearInformations()
    {
        ClearInformationsPO();
        ClearInformationsCO();
    }

    private void SetInformationsSingularPO(PlayerObject po, Player p)
    {
        guiPlPan_PlSel_PluralPO.gameObject.SetActive(false);
        guiPlPan_PlSel_PluralPO.ClearData();

        guiPlPan_PlSel_SingularPO.SetData(po);
        guiPlPan_PlSel_POCommandCard.SetData(po, p);
        guiPlPan_PlSel_SingularPO.gameObject.SetActive(true);
        guiPlPan_PlSel_POCommandCard.gameObject.SetActive(true);
    }

    private void SetInformationsPluralPO(List<PlayerObject> poList, PlayerObject relevantPO, Player p)
    {
        guiPlPan_PlSel_SingularPO.gameObject.SetActive(false);
        guiPlPan_PlSel_SingularPO.ClearData();

        guiPlPan_PlSel_PluralPO.SetData(poList);
        guiPlPan_PlSel_POCommandCard.SetData(relevantPO, p);
        guiPlPan_PlSel_PluralPO.gameObject.SetActive(true);
        guiPlPan_PlSel_POCommandCard.gameObject.SetActive(true);
    }

    private void SetInformationsCO(CityObject co)
    {
        guiPlPan_PlSel_SingularCO.setData(co);
        guiPlPan_PlSel_COControl.setData(co);
        guiPlPan_PlSel_SingularCO.gameObject.SetActive(true);
        guiPlPan_PlSel_COControl.gameObject.SetActive(true);
    }

    private void ClearInformationsPO()
    {
        guiPlPan_PlSel_SingularPO.gameObject.SetActive(false);
        guiPlPan_PlSel_PluralPO.gameObject.SetActive(false);
        guiPlPan_PlSel_POCommandCard.gameObject.SetActive(false);
        guiPlPan_PlSel_SingularPO.ClearData();
        guiPlPan_PlSel_PluralPO.ClearData();
        guiPlPan_PlSel_POCommandCard.ClearData();
    }

    private void ClearInformationsCO()
    {
        guiPlPan_PlSel_SingularCO.gameObject.SetActive(false);
        guiPlPan_PlSel_COControl.gameObject.SetActive(false);
        guiPlPan_PlSel_SingularCO.clearData();
        guiPlPan_PlSel_COControl.clearData();
    }

    /**
     * DO NOT COMMENT OR ERASE THIS FUNCION
     * Its the reason why the player object profile button in the Informations Panel works.
     */
    public void BTN_LMB_PO_Profile()
    {
        screenManager.gameManager.playerManager.localPlayer.pCamera.cameraOverSelection();
    }

    /**
     * DO NOT COMMENT OR ERASE THIS FUNCION
     * Its the reason why the player object profile button in the Informations Panel works.
     */
    public void BTN_RMB_PO_Profile()
    {
        Debug.Log("hudButton_playerObjectProfile_rmb");
    }

    /**
     * DO NOT COMMENT OR ERASE THIS FUNCION
     * Its the reason why the multiple selection buttons in the Informations Panel works.
     */
    public void BTN_LMB_PO_Selection(int index)
    {
        InputManager im = screenManager.gameManager.inputManager;
        PlayerManager pm = screenManager.gameManager.playerManager;
        PlayerSelection pSel = pm.localPlayer.pSelection;
        PlayerObject po = pSel.selectedPlayerObjects[index];

        //4 possible outcomes can happen based on if either the shift and/or control keys are pressed.
        bool shift = im.shiftPress;
        bool control = im.ctrlPress;

        // 1)   Sets the binded PlayerObject as single selection.
        if (!shift && !control)
            pSel.changeSelection(po, false);

        // 2)   Removes the binded PlayerObject from the current selection.
        if (shift && !control)
            pSel.removeFromSelection(po, false);

        // 3)   Sets a multiple selection of all PlayerObjects of same type as the binded one.
        if (!shift && control)
            pSel.changeSelection(po, true);

        // 4)   Removes all PlayerObjects of same type as the bined one from the current selection.
        if (shift && control)
            pSel.removeFromSelection(po, true);

        pSel.SortPlayerObjectsByRelevancy();
    }

    /**
     * DO NOT COMMENT OR ERASE THIS FUNCION
     * Its the reason why the multiple selection buttons in the Informations Panel works.
     */
    public void BTN_RMB_PO_Selection(int index)
    {
        Debug.Log("hudButton_selection_rmb with index " + index);
    }

    /**
     * DO NOT COMMENT OR ERASE THIS FUNCION
     * Its the reason why the activity buttons in the Informations Panel works.
     */
    public void BTN_LMB_PO_ActionsQueued(int index)
    {
        Debug.Log("hudButton_activity_lmb with index " + index);
    }

    /**
     * DO NOT COMMENT OR ERASE THIS FUNCION
     * Its the reason why the activity buttons in the Informations Panel works.
     */
    public void BTN_RMB_PO_ActionsQueued(int index)
    {
        Debug.Log("hudButton_activity_rmb with index " + index);
    }

    /**
     * DO NOT COMMENT OR ERASE THIS FUNCION
     * Its the reason why the action buttons in the Informations Panel works.
     */
    public void BTN_LMB_PO_ActionFromCard(int index)
    {
        InputManager im = screenManager.gameManager.inputManager;
        PlayerManager pm = screenManager.gameManager.playerManager;

        if (im.ihPlay.CheckLock_HandlingAction())
        {
            //TODO DO SOMETHING ELSE IF LOCK IS ACTIVE?
            return;
        }

        im.ihPlay.CreateCandidateAction(pm, index, null, im.shiftPress);
        im.ihPlay.lock_actionFromGUI = true;
    }

    /**
     * DO NOT COMMENT OR ERASE THIS FUNCION
     * Its the reason why the action buttons in the Informations Panel works.
     */
    public void BTN_RMB_PO_ActionFromCard(int index)
    {
        Debug.Log("hudButton_action_rmb with index " + index);
    }

    
}
