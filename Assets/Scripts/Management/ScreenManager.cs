using BPS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour {
    public GameManager gameManager;
    public ResourceManager resourceManager;
    public SMMode_Editor editorMode;
    public SMMode_Play playMode;

    [Header("Initial Parameters")]
    public Rect selectionRect;

    [Header("Execution Status")]
    public SMMode currentSMMode;
    public Texture2D activeCursor;
    public CursorState activeCursorState;
    public Rect cursorPosition;
    public int activeCursorFrame = 0;

    [Header("Execution Flags")]
    //public bool mouseOverHud;
    public bool isUsingSelectionBox;

    [Header("Selection Rectangle stuff")]
    Rect screenSelectionRect;
    Rect unitSelectionRect;

    // Use this for initialization
    void Start () {
        gameManager = FindObjectOfType<GameManager>();
        resourceManager = gameManager.resourceManager;
        editorMode = GetComponentInChildren<SMMode_Editor>();
        playMode = GetComponentInChildren<SMMode_Play>();

        ChangeScreenMode(gameManager.useMode);
        activeCursorState = CursorState.Default;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI()
    {
        if (!resourceManager)
            resourceManager = gameManager.resourceManager;

        if (isUsingSelectionBox)
        {
            MouseController mouse = null;
            switch (gameManager.useMode)
            {
                case InGameUseMode.PLAY:
                    mouse = gameManager.playerManager.localPlayer.pMouse;
                    break;
            }

            if (mouse)
            {
                selectionRect = calculateSelectionBox(mouse.mouseOriginalScreenPos, Input.mousePosition);
                DrawSelectionBox(
                    selectionRect,
                    resourceManager.selectionBoxTexture,
                    resourceManager.selectionBoxFillingColor,
                    resourceManager.selectionBoxBorderColor,
                    resourceManager.selectionBoxBorderThickness);
            }
        }

        //mouseOverHud = !MouseInBounds() && 
        //    activeCursorState != CursorState.PanRight && 
        //    activeCursorState != CursorState.PanUp;
        DrawMouseCursor();

        ////THIS IS USED FOR DEBUGGING - SHOWS THE EXTENTS OF THE SELECTION RECTANGLE
        //if (!screenSelectionRect.Equals(null) && !unitSelectionRect.Equals(null))
        //{
        //    GUI.DrawTexture(screenSelectionRect, resourceManager.hpBar.texture);
        //    GUI.DrawTexture(unitSelectionRect, resourceManager.mpBar.texture);
        //}
        ////END OF DEBUGGHING THING

        ////THIS IS USED FOR DEBUGGING - SHOWS THE SPACE OCCUPIED BY SELECTION BOXES OF ALL PLAYEROBJECTS
        //foreach (var selectableObject in FindObjectsOfType<PlayerObject>())
        //{
        //    GUI.DrawTexture(
        //        BPSHelperFunctions.calculateSelectionBox(selectableObject.selectionBounds, gameManager.screenPlayingArea),
        //        resourceManager.selectionBoxTexture);
        //}
        ////END OF DEBUGGHING THING
    }

    public void ChangeScreenMode(InGameUseMode useMode)
    {
        CloseAllWindows();

        editorMode.gameObject.SetActive(false);
        playMode.gameObject.SetActive(false);

        switch (useMode)
        {
            case InGameUseMode.UNDEFINED:
                currentSMMode = null;
                break;
            case InGameUseMode.EDITOR:
                currentSMMode = editorMode;
                break;
            case InGameUseMode.PLAY:
                currentSMMode = playMode;
                break;
            default:
                break;
        }

        if (currentSMMode)
            currentSMMode.gameObject.SetActive(true);
    }

    public void CloseAllWindows()
    {
        if (currentSMMode)
            currentSMMode.CloseAllWindows();
    }

    public bool isLevelObjectInsideSelectionBox(LevelObject lo, Vector3 initialRectPos, Vector3 finalRectPos)
    {
        screenSelectionRect = calculateSelectionBox(initialRectPos, finalRectPos);
        unitSelectionRect = BPSHelperFunctions.calculateSelectionBox(lo.selectionBounds, gameManager.screenPlayingArea);
        return screenSelectionRect.Overlaps(unitSelectionRect);
    }

    private void DrawSelectionBox(Rect rect, Texture2D texture, Color fillingColor, Color borderColor, int borderThickness)
    {
        //if (rect != null)
        GUI.color = fillingColor;
        GUI.DrawTexture(rect, texture);

        GUI.color = borderColor;
        // Top
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, rect.width, borderThickness), texture);
        // Left
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, borderThickness, rect.height), texture);
        // Right
        GUI.DrawTexture(new Rect(rect.xMax - borderThickness, rect.yMin, borderThickness, rect.height), texture);
        // Bottom
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMax - borderThickness, rect.width, borderThickness), texture);

        //Got this stuff from an tutorial. I think this last line resets GUI.color to default...
        GUI.color = Color.white;
    }

    private Rect calculateSelectionBox(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        // Move origin from bottom left to top left
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        // Calculate corners
        Vector3 topLeft = Vector3.Min(screenPosition1, screenPosition2);
        Vector3 bottomRight = Vector3.Max(screenPosition1, screenPosition2);
        // Create Rect
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    private void DrawMouseCursor()
    {
        ResourceManager rm = gameManager.ResourceManager();

        //Cursor.visible = false;   //UNCOMMENT THIS WHEN GAME IS MORE POLISHED
        GUI.skin = rm.mouseCursorSkin;
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
        UpdateCursorAnimation();
        cursorPosition = GetCursorDrawPosition();
        GUI.Label(cursorPosition, activeCursor);
        GUI.EndGroup();
    }

    private Rect GetCursorDrawPosition()
    {
        //set base position for custom cursor image
        float leftPos = Input.mousePosition.x;
        float topPos = Screen.height - Input.mousePosition.y;   //screen draw coordinates are inverted
                                                                //adjust position base on the type of cursor being shown
        if (activeCursorState == CursorState.PanRight) leftPos = Screen.width - activeCursor.width;
        else if (activeCursorState == CursorState.PanDown) topPos = Screen.height - activeCursor.height;
        else if (activeCursorState == CursorState.Move || activeCursorState == CursorState.Select || activeCursorState == CursorState.Ability)
        {
            topPos -= activeCursor.height / 2;
            leftPos -= activeCursor.width / 2;
        }
        return new Rect(leftPos, topPos, activeCursor.width, activeCursor.height);
    }

    private void UpdateCursorAnimation()
    {
        ResourceManager rm = gameManager.ResourceManager();

        //sequence animation for cursor (based on more than one image for the cursor)
        //change once per second, loops through array of images
        Texture2D[] animation = null;
        switch (activeCursorState)
        {
            case CursorState.Default:
                animation = rm.cursorsDefault;
                break;
            case CursorState.PanUp:
                animation = rm.cursorsPanUp;
                break;
            case CursorState.PanLeft:
                animation = rm.cursorsPanLeft;
                break;
            case CursorState.PanDown:
                animation = rm.cursorsPanDown;
                break;
            case CursorState.PanRight:
                animation = rm.cursorsPanRight;
                break;
            case CursorState.Select:
                animation = rm.cursorsSelect;
                break;
            case CursorState.Move:
                animation = rm.cursorsMove;
                break;
            case CursorState.Defend:
                animation = rm.cursorsDefend;
                break;
            case CursorState.Attack:
                animation = rm.cursorsAttack;
                break;
            case CursorState.RallyPoint:
                animation = rm.cursorsRallyPoint;
                break;
            case CursorState.Ability:
                animation = rm.cursorsAbility;
                break;
            default:
                return;
        }

        activeCursorFrame = (int)Time.time % animation.Length;
        activeCursor = animation[activeCursorFrame];
    }
}
