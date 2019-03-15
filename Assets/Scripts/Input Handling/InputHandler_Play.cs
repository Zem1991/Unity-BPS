using BPS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zem.Directions;

public class InputHandler_Play : MonoBehaviour
{
    [Header("Input Locks")]
    public bool lock_pauseMenu;
    public bool lock_windowOpen;
    public bool lock_usingChat;
    public bool lock_handlingCandidateAction;
    public bool lock_handlingSelectionBox;
    public bool lock_handlingConstruction;

    [Header("Execution Locks")]
    public bool lock_actionFromGUI;
    public bool lock_mouseWasUsed;

    public void CallFunctions(InputManager im, PlayerManager pm)
    {
        PlayerCameraControl(im, pm);
        PlayerCameraHotkeys(im, pm);
        PlayerMouseHandling(im, pm);

        //This one either locks mostly everything else or remove other active locks.
        PauseMenu(im, pm);

        //Those require all but their own locks.
        Windows(im, pm);
        Chat(im, pm);
        CandidateAction(im, pm);
        SelectionBox(im, pm);
        Construction(im, pm);

        //Those require all locks at once.
        QuickSelections(im, pm);
        BasicSelectionAndCommands(im, pm);

        //Execution locks are reset after each Update cycle.
        lock_actionFromGUI = false;
        lock_mouseWasUsed = false;

        //This only updates the look of the mouse cursor.
        UpdateMouseCursor(im, pm);
    }

    private void PlayerCameraControl(InputManager im, PlayerManager pm)
    {
        if (!pm.localPlayer)
            return;

        PlayerCameraController cc = pm.localPlayer.pCamera;
        cc.CameraControl(im);
    }

    private void PlayerCameraHotkeys(InputManager im, PlayerManager pm)
    {
        if (!pm.localPlayer)
            return;

        PlayerCameraController cc = pm.localPlayer.pCamera;
        cc.CameraHotkeys(im);
    }

    private void PlayerMouseHandling(InputManager im, PlayerManager pm)
    {
        if (!pm.localPlayer)
            return;

        MouseController mc = pm.localPlayer.pMouse;
        mc.MouseHandling(im);
    }

    private void PauseMenu(InputManager im, PlayerManager pm)
    {
        ScreenManager sm = im.gameManager.screenManager;
        SMMode_Play smmp = sm.currentSMMode as SMMode_Play;

        if (im.escDown)
        {
            //NO WINDOW IS ACTIVE
            if (smmp.currentWindow == null)
            {
                im.gameManager.PAUSE_GAME();
                smmp.OpenWindow(smmp.guiPlWin_Menu, true);
                lock_pauseMenu = true;
            }
            //MENU WINDOW IS ACTIVE
            else if (smmp.currentWindow == smmp.guiPlWin_Menu)
            {
                smmp.CloseAllWindows();
                im.gameManager.RESUME_GAME();
                lock_pauseMenu = false;
            }
            //ANOTHER WINDOW IS ACTIVE
            else
            {
                smmp.CloseAllWindows();
                lock_pauseMenu = false;
            }
        }
    }

    private void Windows(InputManager im, PlayerManager pm)
    {
        ScreenManager sm = im.gameManager.screenManager;
        SMMode_Play smmp = sm.currentSMMode as SMMode_Play;

        if (CheckLock_WindowOpen())
        {
            //TODO DO SOMETHING ELSE IF LOCK IS ACTIVE?
            return;
        }

        if (im.f01Down)    //HELP
            smmp.OpenWindow(smmp.guiPlWin_Help, true);
        if (im.f02Down)    //POLITICAL CAMPAIGN
            smmp.OpenWindow(smmp.guiPlWin_PoliticalCampaign, true);
        if (im.f03Down)    //BUILDINGS
            smmp.OpenWindow(smmp.guiPlWin_Buildings, true);
        if (im.f04Down)    //UNITS
            smmp.OpenWindow(smmp.guiPlWin_Units, true);
        if (im.f06Down)    //UNDESIRABLES
            smmp.OpenWindow(smmp.guiPlWin_Undesirables, true);
        if (im.f07Down)    //CORRUPTION
            smmp.OpenWindow(smmp.guiPlWin_Corruption, true);
        if (im.f08Down)    //FINANCES
            smmp.OpenWindow(smmp.guiPlWin_Finances, true);
        if (im.f10Down)    //EVENTS
            smmp.OpenWindow(smmp.guiPlWin_Events, true);

        //TODO MINIMAPA AMPLIADO

        if (smmp.currentWindow != null)
            lock_windowOpen = true;
        else
            lock_windowOpen = false;
    }

    private void Chat(InputManager im, PlayerManager pm)
    {
        if (CheckLock_UsingChat())
        {
            //TODO DO SOMETHING ELSE IF LOCK IS ACTIVE?
            return;
        }

        //TODO CHAT
        lock_usingChat = false;
    }

    private void CandidateAction(InputManager im, PlayerManager pm)
    {
        if (CheckLock_HandlingAction())
        {
            //TODO DO SOMETHING ELSE IF LOCK IS ACTIVE?
            return;
        }

        Player localPlayer = pm.localPlayer;
        if (!localPlayer)
            return;
        PlayerSelection pSel = localPlayer.pSelection;

        //Can only send actions if something is selected!
        if (localPlayer.pSelection.selectedPlayerObjects.Count <= 0)
            return;

        bool shift = im.shiftPress;
        MouseController mouse = localPlayer.pMouse;
        LevelObject lObj = null;
        LevelObject_Component locw = null;

        if (mouse.didMouseHitSomething)
        {
            lObj = mouse.mouseHit.collider.gameObject.GetComponent<LevelObject>();
            locw = mouse.mouseHit.collider.gameObject.GetComponent<LevelObject_Component>();
            if (!lObj && locw)
                lObj = locw.getLevelObject();
        }

        /*
         * Here we have two (actually three) options:
         * A:   find new candidate action and apply the lock if such action exists
         * B1:  instantiate the candidate action and remove the lock
         * B2:  cancel the candidate action and remove the lock
         */
        if (!localPlayer.candidateAction)
        {
            int hotkey = -1;
            if (im.q_Down) hotkey = 0;
            if (im.w_Down) hotkey = 1;
            if (im.e_Down) hotkey = 2;
            if (im.r_Down) hotkey = 3;
            if (im.t_Down) hotkey = 4;
            if (im.a_Down) hotkey = 5;
            if (im.s_Down) hotkey = 6;
            if (im.d_Down) hotkey = 7;
            if (im.f_Down) hotkey = 8;
            if (im.g_Down) hotkey = 9;
            if (im.z_Down) hotkey = 10;
            if (im.x_Down) hotkey = 11;
            if (im.c_Down) hotkey = 12;
            if (im.v_Down) hotkey = 13;
            if (im.b_Down) hotkey = 14;

            if (hotkey >= 0)
            {
                CreateCandidateAction(pm, hotkey, lObj, shift);
            }
        }
        else
        {
            if (!lock_actionFromGUI)
            {
                if (im.lmbUp)           //Option: PERFORM the Action
                {
                    pSel.SetNextAction(pSel.selectedPlayerObjects, localPlayer.candidateAction, mouse.mouseScenePosition, lObj, shift);
                    localPlayer.candidateAction = null;
                    lock_handlingCandidateAction = false;
                    lock_mouseWasUsed = true;
                }
                else if (im.rmbUp)      //Option: CANCEL the Action
                {
                    localPlayer.candidateAction = null;
                    lock_handlingCandidateAction = false;
                    lock_mouseWasUsed = true;
                }
            }
        }
    }

    public bool CreateCandidateAction(PlayerManager pm, int index, LevelObject lObj, bool addToQueue)
    {
        Player localPlayer = pm.localPlayer;
        PlayerSelection pSel = localPlayer.pSelection;
        MouseController mouse = localPlayer.pMouse;

        Action candidateAction = pSel.FindAction(pSel.relevantSelectedPOs[0], index);
        if (candidateAction)
        {
            localPlayer.candidateAction = candidateAction;
            lock_handlingCandidateAction = true;

            //HOWEVER, if said action is of type INSTANTANEOUS, then we shall instantiate it right away!
            if (candidateAction.castType == BPS.InGame.CastType.INSTANTANEOUS)
            {
                pSel.SetNextAction(pSel.selectedPlayerObjects, localPlayer.candidateAction, mouse.mouseScenePosition, lObj, addToQueue);
                localPlayer.candidateAction = null;
                lock_handlingCandidateAction = false;
            }
            return true;
        }
        return false;
    }

    private void SelectionBox(InputManager im, PlayerManager pm)
    {
        if (CheckLock_HandlingSelectionBox())
        {
            //TODO DO SOMETHING ELSE IF LOCK IS ACTIVE?
            return;
        }

        Player localPlayer = pm.localPlayer;
        if (!localPlayer)
            return;

        bool lmbPress = im.lmbPress;
        bool lmbUp = im.lmbUp;
        bool shift = im.shiftPress;
        ScreenManager sm = im.gameManager.screenManager;
        PlayerMouseController pmc = localPlayer.pMouse;

        //  If we keep pressed the left mouse button AND drag it around, then begin selectionBox.
        if (lmbPress)
        {
            if (pmc.mouseOriginalScreenPos != Input.mousePosition)
            {
                sm.isUsingSelectionBox = true;
                lock_handlingSelectionBox = true;
            }
            else
            {
                sm.isUsingSelectionBox = false;
                lock_handlingSelectionBox = false;
            }
            return;
        }

        //  If we release the left mouse button AND we are in fact using the selectionBox, then end selectionBox.
        if (lmbUp && sm.isUsingSelectionBox)
        {
            if (pmc.mouseOriginalScreenPos != Input.mousePosition)
            {
                //TODO filter to select only localplayer units
                List<PlayerObject> selectedObjects = new List<PlayerObject>();
                bool hasAnyUnit = false;
                
                foreach (var item in FindObjectsOfType<PlayerObject>())
                {
                    if (item.levelObjectType == BPS.InGame.LevelObjectType.UNIT)
                        hasAnyUnit = true;

                    if (sm.isLevelObjectInsideSelectionBox(item, pmc.mouseOriginalScreenPos, Input.mousePosition))
                        selectedObjects.Add(item);
                }

                List<PlayerObject> finalSelection = new List<PlayerObject>(selectedObjects);
                if (hasAnyUnit)
                {
                    foreach (var item in selectedObjects)
                        if (item.levelObjectType == BPS.InGame.LevelObjectType.BUILDING)
                            finalSelection.Remove(item);
                }

                if (!shift)
                    localPlayer.pSelection.changeSelection(finalSelection);
                else
                    localPlayer.pSelection.addToSelection(finalSelection, false);
            }

            lock_mouseWasUsed = true;
        }

        sm.isUsingSelectionBox = false;
        lock_handlingSelectionBox = false;
    }

    private void Construction(InputManager im, PlayerManager pm)
    {
        if (CheckLock_HandlingConstruction())
        {
            //TODO DO SOMETHING ELSE IF LOCK IS ACTIVE?
            return;
        }

        if (!pm.localPlayer)
            return;

        PlayerBelongings pb = pm.localPlayer.pBelongings;

        //Cannot do anything if the mouse is over an Panel.
        if (im.focusedPanel)
        {
            return;
        }
        //Cancel the placement finding if a Window is suddenly opened.
        if (im.focusedWindow)
        {
            if (pb.isFindingPlacement)
            {
                pb.cancelConstruction();
                lock_handlingConstruction = false;
            }
            return;
        }

        if (pb.isFindingPlacement)
        {
            lock_handlingConstruction = true;

            bool lmbDown = im.lmbDown;
            bool lmbPress = im.lmbPress;
            bool lmbUp = im.lmbUp;
            float lmbPressTime = im.lmbPressTime;
            bool rmbDown = im.rmbDown;
            bool rmbPress = im.rmbPress;
            float rmbPressTime = im.rmbPressTime;
            bool rmbUp = im.rmbUp;
            bool shift = im.shiftPress;

            //First we deal with the rotation and position changes.
            PlayerMouseController pmc = pm.localPlayer.pMouse;
            if (lmbPress)
            {
                if (lmbPressTime >= im.mouseButtonHoldCounter)
                    pb.changeTemporaryBuildingRotation(pmc.mouseScenePosition, true);
            }
            else
            {
                MapManager mm = im.gameManager.MapManager();
                pb.changeTemporaryBuildingPosition(pmc.mouseScenePosition, mm.tileSize);
            }

            //Now, we get all the information required about the current placement and facing direction.
            bool placementWithinBounds, placementWithoutCollisions, placementWithRoadAvailable;
            List<Tile> tilesToBeUsed;
            Road roadToBeUsed;
            float direction = pb.temporaryBuilding.transform.rotation.eulerAngles.y;
            Directions cardinalDir = Directions.NO_DIRECTION;
            DirectionsClass.SnapToCardinalDireciton(direction, out direction, out cardinalDir);
            placementWithinBounds = im.gameManager.canPlaceBuilding(
                pb.temporaryBuilding,
                cardinalDir,
                out placementWithoutCollisions,
                out placementWithRoadAvailable,
                out tilesToBeUsed,
                out roadToBeUsed);
            ResourceManager rm = im.gameManager.ResourceManager();
            if (placementWithinBounds && placementWithoutCollisions && placementWithRoadAvailable)
                pb.temporaryBuilding.applyMaterial(rm.mat_Construction_Allowed, false);
            else
                pb.temporaryBuilding.applyMaterial(rm.mat_Construction_Denied, false);

            //And finally we either confirm or deny (or ignore) the construction.
            //Here we also remove the construction lock.
            if (!lmbPress && lmbUp)
            {
                lock_mouseWasUsed = true;

                if (pb.tempBldgRotationChanged)
                {
                    //Doing this step prevents from immediately placing the building after changing the rotation.
                    pb.tempBldgRotationChanged = false;
                    return;
                }

                if (placementWithinBounds)
                {
                    if (placementWithoutCollisions)
                    {
                        if (placementWithRoadAvailable)
                        {
                            pb.startConstruction(tilesToBeUsed, roadToBeUsed);
                            lock_handlingConstruction = false;
                        }
                        else
                        {
                            pb.noRoadAvailable();
                        }
                    }
                    else
                    {
                        pb.constructionObstructed();
                    }
                }
            }
            else if (rmbPress)
            {
                pb.cancelConstruction();
                lock_handlingConstruction = false;
            }
        }
    }

    private void QuickSelections(InputManager im, PlayerManager pm)
    {
        if (CheckAnyLock())
        {
            //TODO DO SOMETHING ELSE IF LOCK IS ACTIVE?
        }
        else
        {
            if (!pm.localPlayer)
                return;

            PlayerSelection ps = pm.localPlayer.pSelection;
            if (im.a01Down)
                ps.selectedPlayerObjects = ps.qs01;
            if (im.a02Down)
                ps.selectedPlayerObjects = ps.qs02;
            if (im.a03Down)
                ps.selectedPlayerObjects = ps.qs03;
            if (im.a04Down)
                ps.selectedPlayerObjects = ps.qs04;
            if (im.a05Down)
                ps.selectedPlayerObjects = ps.qs05;
            if (im.a06Down)
                ps.selectedPlayerObjects = ps.qs06;
            if (im.a07Down)
                ps.selectedPlayerObjects = ps.qs07;
            if (im.a08Down)
                ps.selectedPlayerObjects = ps.qs08;
            if (im.a09Down)
                ps.selectedPlayerObjects = ps.qs09;
            if (im.a00Down)
                ps.selectedPlayerObjects = ps.qs10;
            
            //TODO MORE?
        }
    }

    private void BasicSelectionAndCommands(InputManager im, PlayerManager pm)
    {
        if (CheckAnyLock() || lock_mouseWasUsed)
        {
            //TODO DO SOMETHING ELSE IF LOCK IS ACTIVE?
            return;
        }

        if (!pm.localPlayer)
            return;

        bool lmbUp = im.lmbUp;
        bool rmbUp = im.rmbUp;
        bool shift = im.shiftPress;

        MouseController mc = pm.localPlayer.pMouse;

        //If the player is trying to place a new building, then we can't select or give commands.
        PlayerBelongings pb = pm.localPlayer.pBelongings;
        if (pb.isFindingPlacement)
            return;

        ////If any HUDWindow is open but we click outside of it, then we just close it without doing anything else.
        //if (!im.focusedWindow && pm.getHUD().getActiveWindowCode() != WindowCode.NO_WINDOW)
        //{
        //    if (lmbDown || rmbDown)
        //    {
        //        pm.getHUD().closeAllWindows();
        //        return;
        //    }
        //}

        //Mouse must be outside of any HUDPanel too.
        if (!im.focusedPanel)
        {
            LevelObject lObj = null;
            LevelObject_Component locw = null;
            PlayerObject pObj = null;
            CityObject cObj = null;
            if (mc.didMouseHitSomething)
            {
                lObj = mc.mouseHit.collider.gameObject.GetComponent<LevelObject>();
                locw = mc.mouseHit.collider.gameObject.GetComponent<LevelObject_Component>();
                if (!lObj && locw)
                    lObj = locw.getLevelObject();
                pObj = lObj as PlayerObject;
                cObj = lObj as CityObject;
            }
            PlayerSelection pSel = pm.localPlayer.pSelection;

            //SELECTION
            if (lmbUp)
            {
                if (!shift)
                {
                    if (!lObj)
                        pSel.clearSelection();
                    else if (pObj)
                        pSel.changeSelection(pObj, false);   //TODO doubleckick for allEquals == true
                    else if (cObj)
                        pSel.changeSelection(cObj);
                    else
                        pSel.clearSelection();
                }
                else
                {
                    //TODO ADD/REMOVE FROM CURRENT SELECTION
                    if (pObj)
                        pSel.addToSelection(pObj, true);     //TODO doubleckick for allEquals == true
                }

                pSel.SortPlayerObjectsByRelevancy();
                lock_mouseWasUsed = true;
            }
            //COMMANDS
            else if (rmbUp)
            {
                pSel.SetNextAction(mc.mouseScenePosition, lObj, shift);
                //pm.localPlayer.pSelection.setNextAction(0, mc.mouseScenePosition, mc.mouseHitCollider, lObj, shift, rmbDown);
                //else if (bldg) ;}
                lock_mouseWasUsed = true;
            }
        }
    }

    //!!    !!    !!    !!    !!    !!    !!    !!    !!    !!    !!    !!    !!    !!    !!    
    //private void commandTargeting(InputManager im)
    //{
    //    return;
    //}

    public bool CheckLock_WindowOpen()
    {
        return
            lock_pauseMenu ||
            lock_usingChat ||
            lock_handlingCandidateAction ||
            lock_handlingSelectionBox ||
            lock_handlingConstruction;
    }

    public bool CheckLock_UsingChat()
    {
        return
            false;
    }

    public bool CheckLock_HandlingAction()
    {
        return
            lock_pauseMenu ||
            lock_windowOpen ||
            lock_usingChat ||
            lock_handlingSelectionBox ||
            lock_handlingConstruction;
    }

    public bool CheckLock_HandlingSelectionBox()
    {
        return
            lock_pauseMenu ||
            lock_windowOpen ||
            lock_usingChat ||
            lock_handlingCandidateAction ||
            lock_handlingConstruction;
    }

    public bool CheckLock_HandlingConstruction()
    {
        return
            lock_pauseMenu ||
            lock_windowOpen ||
            lock_usingChat ||
            lock_handlingCandidateAction ||
            lock_handlingSelectionBox;
    }

    public bool CheckAnyLock()
    {
        return
            lock_pauseMenu ||
            lock_windowOpen ||
            lock_usingChat ||
            lock_handlingCandidateAction ||
            lock_handlingSelectionBox ||
            lock_handlingConstruction;
    }

    private void UpdateMouseCursor(InputManager im, PlayerManager pm)
    {
        if (!pm.localPlayer)
            return;

        ScreenManager sm = im.gameManager.ScreenManager();
        Player p = pm.localPlayer;

        CursorState cs = CursorState.Default;
        if (im.mouseOnScreenTop)
            cs = CursorState.PanUp;
        else if (im.mouseOnScreenLeft)
            cs = CursorState.PanLeft;
        else if (im.mouseOnScreenBottom)
            cs = CursorState.PanDown;
        else if (im.mouseOnScreenRight)
            cs = CursorState.PanRight;
        else if (sm.isUsingSelectionBox)
            cs = CursorState.Select;
        else if (p.candidateAction)
        {
            if (p.candidateAction.actionCommandType == BPS.InGame.ActionCommandType.MOVE)
                cs = CursorState.Move;
            else if (p.candidateAction.actionCommandType == BPS.InGame.ActionCommandType.DEFEND)
                cs = CursorState.Defend;
            else if (p.candidateAction.actionCommandType == BPS.InGame.ActionCommandType.ATTACK)
                cs = CursorState.Attack;
            else if (p.candidateAction.actionCommandType == BPS.InGame.ActionCommandType.RALLY_POINT)
                cs = CursorState.RallyPoint;
            else if (p.candidateAction.actionCommandType == BPS.InGame.ActionCommandType.ABILITY)
                cs = CursorState.Ability;
        }

        sm.activeCursorState = cs;
    }
}
