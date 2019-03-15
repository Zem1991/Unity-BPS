﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUIComponent_Window : GUIComponent
{

	// Use this for initialization
	public override void Start () {
        base.Start();
	}

    // Update is called once per frame
    public override void Update () {
        base.Update();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (screenManager)
            screenManager.gameManager.InputManager().PointerEnter_Window(this);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (screenManager)
            screenManager.gameManager.InputManager().PointerExit_Window(this);
    }
}
