using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SMMode : MonoBehaviour {
    public ScreenManager screenManager;
    public GUIComponent_Window currentWindow;

	// Use this for initialization
	public virtual void Start () {
        screenManager = GetComponentInParent<ScreenManager>();
    }

    // Update is called once per frame
    public virtual void Update () {
		
	}

    public virtual bool CloseAllWindows()
    {
        //Make actual code for this one on the derived classes.
        return false;
    }

    public virtual bool OpenWindow(GUIComponent_Window window, bool closeIfAlreadyOpen)
    {
        //Make actual code for this one on the derived classes.
        return false;
    }
}
