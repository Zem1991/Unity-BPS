using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public GameManager gameManager;

    [Header("Input Handlers")]
    public InputHandler_Editor ihEditor;
    public InputHandler_Play ihPlay;

    [Header("Local Camera and its parameters")]
    public CameraController localCamera;
    public int cameraMovementSpeed = 1;
    public int cameraRotationSpeed = 5;
    public int cameraRevolutionSpeed = 5;
    public int cameraZoomSpeed = 3;

    [Header("Local Mouse and its parameters")]
    public MouseController localMouse;
    public float mouseButtonHoldCounter = 0.5F;

    [Header("Graphical User Interface stuff")]
    public GUIComponent_Panel focusedPanel;
    public GUIComponent_Window focusedWindow;

    [Header("Mouse Input")]
    public float mouseAxisX;
    public float mouseAxisY;
    public float mouseAxisScrollWheel;
    public bool lmbDown, lmbPress, lmbUp;
    public float lmbPressTime;
    public bool rmbDown, rmbPress, rmbUp;
    public float rmbPressTime;
    public bool mmbDown, mmbPress, mmbUp;
    public float mmbPressTime;
    public bool mouseOnScreenTop, mouseOnScreenLeft, mouseOnScreenBottom, mouseOnScreenRight;

    [Header("Keyboard Input")]
    public bool escDown;
    public bool f01Down, f02Down, f03Down, f04Down, f05Down, f06Down, f07Down, f08Down, f09Down, f10Down;
    public bool minusDown, plusDown;
    public bool tabDown;
    public bool a01Down, a02Down, a03Down, a04Down, a05Down, a06Down, a07Down, a08Down, a09Down, a00Down;
    public bool q_Down, w_Down, e_Down, r_Down, t_Down;
    public bool a_Down, s_Down, d_Down, f_Down, g_Down;
    public bool z_Down, x_Down, c_Down, v_Down, b_Down;
    public bool enterDown;
    public bool insertDown, deleteDown;
    public bool homeDown, endDown;
    public bool pgUpPress, pgDownPress;
    public bool arrowUpPress, arrowLeftPress, arrowDownPress, arrowRightPress;
    public bool np01Press, np02Press, np03Press, np04Press, np05Press, np06Press, np07Press, np08Press, np09Press, np00Press;
    public bool shiftPress, ctrlPress, altPress;

    // Use this for initialization
    void Start () {
        gameManager = FindObjectOfType<GameManager>();

        localCamera = GetComponentInChildren<CameraController>();
        localMouse = GetComponentInChildren<MouseController>();
    }
	
	// Update is called once per frame
	void Update () {
        VerifyMouseInput();
        VerifyKeyboardInput();

        switch (gameManager.useMode)
        {
            case BPS.InGameUseMode.UNDEFINED:
                break;
            case BPS.InGameUseMode.EDITOR:
                ihEditor.CallFunctions(this);
                break;
            case BPS.InGameUseMode.PLAY:
                ihPlay.CallFunctions(this, gameManager.PlayerManager());
                break;
            default:
                break;
        }
    }

    public void PointerEnter_Panel(GUIComponent_Panel panel)
    {
        focusedPanel = panel;
    }

    public void PointerExit_Panel(GUIComponent_Panel panel)
    {
        if (focusedPanel == panel)
            focusedPanel = null;
    }

    public void PointerEnter_Window(GUIComponent_Window window)
    {
        focusedWindow = window;
    }

    public void PointerExit_Window(GUIComponent_Window window)
    {
        if (focusedWindow = window)
            focusedWindow = null;
    }

    private void VerifyMouseInput()
    {
        mouseAxisX = Input.GetAxis("Mouse X");
        mouseAxisY = Input.GetAxis("Mouse Y");
        mouseAxisScrollWheel = Input.GetAxis("Mouse ScrollWheel");
        lmbDown = Input.GetKeyDown(KeyCode.Mouse0);
        lmbPress = Input.GetKey(KeyCode.Mouse0);
        lmbUp = Input.GetKeyUp(KeyCode.Mouse0);
        rmbDown = Input.GetKeyDown(KeyCode.Mouse1);
        rmbPress = Input.GetKey(KeyCode.Mouse1);
        rmbUp = Input.GetKeyUp(KeyCode.Mouse1);
        mmbDown = Input.GetKeyDown(KeyCode.Mouse2);
        mmbPress = Input.GetKey(KeyCode.Mouse2);
        mmbUp = Input.GetKeyUp(KeyCode.Mouse2);

        if (lmbDown)
            lmbPressTime = 0;
        if (lmbPress)
            lmbPressTime += Time.deltaTime;
        if (rmbDown)
            rmbPressTime = 0;
        if (rmbPress)
            rmbPressTime += Time.deltaTime;
        if (mmbDown)
            mmbPressTime = 0;
        if (mmbPress)
            mmbPressTime += Time.deltaTime;

        mouseOnScreenTop = (Input.mousePosition.y >= Screen.height);
        mouseOnScreenLeft = (Input.mousePosition.x <= 0);
        mouseOnScreenBottom = (Input.mousePosition.y <= 0);
        mouseOnScreenRight = (Input.mousePosition.x >= Screen.width);
    }

    private void VerifyKeyboardInput()
    {
        escDown = Input.GetKeyDown(KeyCode.Escape);
        f01Down = Input.GetKeyDown(KeyCode.F1);
        f02Down = Input.GetKeyDown(KeyCode.F2);
        f03Down = Input.GetKeyDown(KeyCode.F3);
        f04Down = Input.GetKeyDown(KeyCode.F4);
        f05Down = Input.GetKeyDown(KeyCode.F5);
        f06Down = Input.GetKeyDown(KeyCode.F6);
        f07Down = Input.GetKeyDown(KeyCode.F7);
        f08Down = Input.GetKeyDown(KeyCode.F8);
        f09Down = Input.GetKeyDown(KeyCode.F9);
        f10Down = Input.GetKeyDown(KeyCode.F10);
        minusDown = Input.GetKeyDown(KeyCode.Minus);
        plusDown = Input.GetKeyDown(KeyCode.Plus);
        tabDown = Input.GetKeyDown(KeyCode.Tab);
        a01Down = Input.GetKeyDown(KeyCode.Alpha1);
        a02Down = Input.GetKeyDown(KeyCode.Alpha2);
        a03Down = Input.GetKeyDown(KeyCode.Alpha3);
        a04Down = Input.GetKeyDown(KeyCode.Alpha4);
        a05Down = Input.GetKeyDown(KeyCode.Alpha5);
        a06Down = Input.GetKeyDown(KeyCode.Alpha6);
        a07Down = Input.GetKeyDown(KeyCode.Alpha7);
        a08Down = Input.GetKeyDown(KeyCode.Alpha8);
        a09Down = Input.GetKeyDown(KeyCode.Alpha9);
        a00Down = Input.GetKeyDown(KeyCode.Alpha0);
        q_Down = Input.GetKeyDown(KeyCode.Q);
        w_Down = Input.GetKeyDown(KeyCode.W);
        e_Down = Input.GetKeyDown(KeyCode.E);
        r_Down = Input.GetKeyDown(KeyCode.R);
        t_Down = Input.GetKeyDown(KeyCode.T);
        a_Down = Input.GetKeyDown(KeyCode.A);
        s_Down = Input.GetKeyDown(KeyCode.S);
        d_Down = Input.GetKeyDown(KeyCode.D);
        f_Down = Input.GetKeyDown(KeyCode.F);
        g_Down = Input.GetKeyDown(KeyCode.G);
        z_Down = Input.GetKeyDown(KeyCode.Z);
        x_Down = Input.GetKeyDown(KeyCode.X);
        c_Down = Input.GetKeyDown(KeyCode.C);
        v_Down = Input.GetKeyDown(KeyCode.V);
        b_Down = Input.GetKeyDown(KeyCode.B);
        enterDown = Input.GetKeyDown(KeyCode.Return);
        insertDown = Input.GetKeyDown(KeyCode.Insert);
        deleteDown = Input.GetKeyDown(KeyCode.Delete);
        homeDown = Input.GetKeyDown(KeyCode.Home);
        endDown = Input.GetKeyDown(KeyCode.End);

        pgUpPress = Input.GetKey(KeyCode.PageUp);
        pgDownPress = Input.GetKey(KeyCode.PageDown);
        arrowUpPress = Input.GetKey(KeyCode.UpArrow);
        arrowLeftPress = Input.GetKey(KeyCode.LeftArrow);
        arrowDownPress = Input.GetKey(KeyCode.DownArrow);
        arrowRightPress = Input.GetKey(KeyCode.RightArrow);
        np01Press = Input.GetKey(KeyCode.Keypad1);
        np02Press = Input.GetKey(KeyCode.Keypad2);
        np03Press = Input.GetKey(KeyCode.Keypad3);
        np04Press = Input.GetKey(KeyCode.Keypad4);
        np05Press = Input.GetKey(KeyCode.Keypad5);
        np06Press = Input.GetKey(KeyCode.Keypad6);
        np07Press = Input.GetKey(KeyCode.Keypad7);
        np08Press = Input.GetKey(KeyCode.Keypad8);
        np09Press = Input.GetKey(KeyCode.Keypad9);
        np00Press = Input.GetKey(KeyCode.Keypad0);
        shiftPress = Input.GetKey(KeyCode.LeftShift);
        ctrlPress = Input.GetKey(KeyCode.LeftControl);
        altPress = Input.GetKey(KeyCode.LeftAlt);
    }
}
