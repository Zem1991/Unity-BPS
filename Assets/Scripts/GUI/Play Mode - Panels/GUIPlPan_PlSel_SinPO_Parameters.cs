using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlPan_PlSel_SinPO_Parameters : GUIComponent_Panel
{
    public Button btn_profile;
    public Text txt_name, txt_hp, txt_mp;
    public Text txt_atkDmg, txt_atkRng, txt_atkSpd;
    public Text txt_defense, txt_sightRng, txt_moveSpd;

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
        PO_Unit loAsUnit = po as PO_Unit;

        btn_profile.image.sprite = po.img_profile;
        txt_name.text = po.levelObjectName;
        txt_hp.text = "HP " + po.hp_current + "/" + po.hp_maximum;
        txt_mp.text = "MP " + po.mp_current + "/" + po.mp_maximum;
        txt_atkDmg.text = "aDmg " + po.atkDamage;
        txt_atkRng.text = "aRan " + po.atkRange;
        txt_atkSpd.text = "aSpd " + po.atkSpeed;
        txt_defense.text = "Defn " + po.defense;
        txt_sightRng.text = "sgtR " + po.sightRange;

        if (loAsUnit)
        {
            txt_moveSpd.gameObject.SetActive(true);
            txt_moveSpd.text = "movS: " + loAsUnit.moveSpeed;
        }
        else
        {
            txt_moveSpd.text = "movS: ";
            txt_moveSpd.gameObject.SetActive(false);
        }
    }

    public void ClearData()
    {
        btn_profile.image.sprite = null;
        txt_name.text = null;
        txt_hp.text = null;
        txt_mp.text = null;
        txt_atkDmg.text = null;
        txt_atkRng.text = null;
        txt_atkSpd.text = null;
        txt_defense.text = null;
        txt_sightRng.text = null;
        txt_moveSpd.text = null;
    }
}
