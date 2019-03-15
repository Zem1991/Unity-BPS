using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlPan_PlSel_POCC_SelectedAction : GUIComponent_Panel
{
    public Button selAction;
    public Text selActionName;
    public Text selActionInstructions;

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

    public void SetData(Action a)
    {
        selAction.image.sprite = a.img_icon;
        selActionName.text = a.actionName;
        selActionInstructions.text = BPSHelperFunctions.GenerateActionInstructions(a);
    }

    public void ClearData()
    {
        selAction.image.sprite = null;
        selActionName.text = null;
        selActionInstructions.text = null;
    }
}
