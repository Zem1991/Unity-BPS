using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlPan_PlayerSelection_SingularPlayerObject : GUIComponent_Panel
{
    public GUIPlPan_PlSel_SinPO_Parameters guiPlPan_PlSel_SinPO_Parameters;
    public GUIPlPan_PlSel_SinPO_Actions guiPlPan_PlSel_SinPO_Actions;

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

    public void SetData(PlayerObject po)
    {
        guiPlPan_PlSel_SinPO_Parameters.SetData(po);
        guiPlPan_PlSel_SinPO_Actions.SetData(po);
    }

    public void ClearData()
    {
        guiPlPan_PlSel_SinPO_Parameters.ClearData();
        guiPlPan_PlSel_SinPO_Actions.ClearData();
    }
}
