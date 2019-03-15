using BPS.Population;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction : MonoBehaviour {
    public string factionName;
    public Color factionColor;
    public PoliticalPosition politicalPosition;
    //public Material factionMaterialMain;
    //public Material factionMaterialAux;

    [Header("Buildings")]
    public PO_Building bldg_basic_hq;
    public PO_Building bldg_advanced_hq;
    public PO_Building bldg_printShop;
    //public PO_Building bldg_undesirable_basic;
    //public PO_Building bldg_undesirable_adv1;
    //public PO_Building bldg_undesirable_adv2;

    [Header("Units")]
    public PO_Unit unit_militant;
    public PO_Unit unit_voteHunter;
    public PO_Unit unit_henchman;
    public PO_Unit unit_lawyer;

    //[Header("Undesirables")]

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
