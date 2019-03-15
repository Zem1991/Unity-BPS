using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class GUIComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ScreenManager screenManager;
    public SMMode smMode;

    // Use this for initialization
    public virtual void Start()
    {
        screenManager = GetComponentInParent<ScreenManager>();
        smMode = GetComponentInParent<SMMode>();
    }

    // Update is called once per frame
    public virtual void Update () {
		
	}

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        //Make actual code for this one on the derived classes.
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        //Make actual code for this one on the derived classes.
    }
}
