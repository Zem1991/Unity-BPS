using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler_Editor : MonoBehaviour
{
    //[Header("Input Locks")]
    //public bool lock_pauseMenu;

    public void CallFunctions(InputManager im)
    {
        //PanelsAndWindows(im);

        LocalCameraControl(im);
        LocalCameraHotkeys(im);
        LocalMouseHandling(im);

        MouseClick_ElevationChange(im);
    }

    private void LocalCameraControl(InputManager im)
    {
        CameraController cc = im.localCamera;
        cc.CameraControl(im);
    }

    private void LocalCameraHotkeys(InputManager im)
    {
        CameraController cc = im.localCamera;
        cc.CameraHotkeys(im);
    }

    private void LocalMouseHandling(InputManager im)
    {
        if (!im.localMouse || !im.localCamera)
            return;

        MouseController mc = im.localMouse;
        mc.MouseHandling(im);
    }

    private void MouseClick_ElevationChange(InputManager im)
    {
        if (!im.localMouse)
            return;

        MapManager mm = im.gameManager.MapManager();
        ScreenManager sm = im.gameManager.ScreenManager();
        bool shift = Input.GetKey(KeyCode.LeftShift);

        MouseController mc = im.localMouse;

        ////If the player is trying to place a new building, then we can't select or give commands.
        //PlayerBelongings pb = playerManager.getLocalPlayer().getPlayerBelongings();
        //if (pb.isFindingPlacement())
        //    return;

        //If any GUIWindow is open or the mouse is above any GUIPanel, then nothing is done.
        if ((sm.currentSMMode && sm.currentSMMode.currentWindow) || im.focusedPanel)
            return;

        if (mc.didMouseHitSomething)
        {
            Tile tile = mc.mouseHit.collider.gameObject.GetComponent<Tile>();
            //LevelObject lObj = im.mouseHit.collider.gameObject.GetComponent<LevelObject>();
            //LevelObject_Component locw = im.mouseHit.collider.gameObject.GetComponent<LevelObject_Component>();
            //if (!lObj && locw)
            //    lObj = locw.getLevelObject();
            //PlayerObject pObj = lObj as PlayerObject;
            //CityObject cObj = lObj as CityObject;

            if (im.lmbDown)
            {
                if (!shift)
                    MapEditorTools.Tile_Raise(tile, mm.tileHeight);
                else
                    MapEditorTools.Tile_Raise(tile, mm.tileHeight, true, false);
            }
            if (im.rmbDown)
            {
                if (!shift)
                    MapEditorTools.Tile_Lower(tile, mm.tileHeight);
                else
                    MapEditorTools.Tile_Lower(tile, mm.tileHeight, true, false);
            }
        }
    }
}
