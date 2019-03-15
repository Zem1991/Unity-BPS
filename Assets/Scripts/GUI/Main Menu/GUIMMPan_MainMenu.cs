using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIMMPan_MainMenu : GUIComponent_Panel
{
    public enum submenu
    {
        NO_SUBMENU,
        HOW_TO_PLAY,
        TESTS
    }

    [Header("Initial UI Stuff")]
    public Button btn_playNow;
    public Button btn_Exit;
    //public Button btn_Overview;
    //public Button btn_Tutorials;
    //public Button btn_HelpTopics;

    [Header("Other Initial Stuff")]
    public new Camera camera;
    public MainMenuTV television;

    //public GUIComponent_Panel panel_GameTitle;
    //public GUIMMPan_MM_HowToPlay panel_HowToPlay;
    //public GUIMMPan_MM_Tests panel_Tests;

    [Header("Execution Parameters")]
    public submenu currentSubmenu = submenu.NO_SUBMENU;
    public int currentCameraFOV = 60;
    public int desiredCameraFOV = 60;
    public float zoomingAmount = 180;
    public bool zoomingCamera = false;
    public bool zoomingIn = false;
    public Vector3 mousePos;
    public float rotationAmount = 2.5F;
    public float rotationAngleLimit = 60;

    private bool playNow = false;

    // Use this for initialization
    public override void Start()
    {
        camera.fieldOfView = desiredCameraFOV;

        //panel_HowToPlay = GetComponentInChildren<GUIPanel_HowToPlay>();
        //panel_HowToPlay.gameObject.SetActive(false);
        //panel_Tests.gameObject.SetActive(false);

        currentSubmenu = submenu.NO_SUBMENU;
    }

    // Update is called once per frame
    public override void Update () {
        if (zoomingCamera)
        {
            camera.fieldOfView += (zoomingIn ? -zoomingAmount : zoomingAmount) * Time.deltaTime;
            camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, Mathf.Min(currentCameraFOV, desiredCameraFOV), Mathf.Max(currentCameraFOV, desiredCameraFOV));
            if (camera.fieldOfView == desiredCameraFOV)
            {
                currentCameraFOV = desiredCameraFOV;
                zoomingCamera = false;

                if (playNow)
                    SceneManager.LoadScene("In Game");

                if (!zoomingIn)
                {
                    television.unpauseVideo(true);
                    //panel_GameTitle.gameObject.SetActive(true);
                    //panel_MainMenu.gameObject.SetActive(true);
                }
                else
                {
                    openSubmenu();
                }
            }
        }

        mousePosition();
        //mouseScroll();
        processInputs();
	}

    public void BTN_PlayNow()
    {
        cameraZoomIn();
        playNow = true;
    }

    public void BTN_Exit()
    {
        //Debug.Log("Application.Quit();"); Is ignored in Editor.
        Application.Quit();
    }

    public void cameraZoomIn()
    {
        television.pauseVideo(true);
        //panel_GameTitle.gameObject.SetActive(false);
        //panel_MainMenu.gameObject.SetActive(false);

        desiredCameraFOV = 25;
        zoomingCamera = true;
        zoomingIn = true;
    }

    public void cameraZoomOut()
    {
        closeSubmenu();

        desiredCameraFOV = 60;
        zoomingCamera = true;
        zoomingIn = false;
    }

    private void openSubmenu()
    {
        switch (currentSubmenu)
        {
            case submenu.NO_SUBMENU:
                break;
            case submenu.HOW_TO_PLAY:
                //panel_HowToPlay.gameObject.SetActive(true);
                //panel_HowToPlay.mainMenu = this;
                break;
            case submenu.TESTS:
                //panel_Tests.gameObject.SetActive(true);
                //panel_Tests.mainMenu = this;
                break;
            default:
                break;
        }
    }

    private void closeSubmenu()
    {
        switch (currentSubmenu)
        {
            case submenu.NO_SUBMENU:
                break;
            case submenu.HOW_TO_PLAY:
                //panel_HowToPlay.gameObject.SetActive(false);
                //panel_HowToPlay.mainMenu = null;
                break;
            case submenu.TESTS:
                //panel_Tests.gameObject.SetActive(false);
                //panel_Tests.mainMenu = null;
                break;
            default:
                break;
        }
        currentSubmenu = submenu.NO_SUBMENU;
    }

    private void mousePosition()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit mouseHit;
        if (Physics.Raycast(mouseRay, out mouseHit))
        {
            mousePos = mouseHit.point;
        }
    }

    private void mouseScroll()
    {
        //TODO NOT WORKING AS INTENDED!

        bool rotUp = (Input.mousePosition.y >= Screen.height - 1);
        bool rotDown = (Input.mousePosition.y <= 0);
        bool rotLeft = (Input.mousePosition.x <= 0);
        bool rotRight = (Input.mousePosition.x >= Screen.width - 1);

        Vector3 rotation = new Vector3();
        if (rotUp && !rotDown) rotation.x = -rotationAmount;
        else if (!rotUp && rotDown) rotation.x = rotationAmount;
        if (rotLeft && !rotRight) rotation.y = -rotationAmount;
        else if (!rotLeft && rotRight) rotation.y = rotationAmount;
        camera.transform.Rotate(rotation, Space.World);
    }

    private void processInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            television.pauseVideo();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            television.startNextVideo();
        }
    }
}
