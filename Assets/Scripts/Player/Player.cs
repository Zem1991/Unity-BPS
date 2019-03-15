using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Player Information")]
    public string playerName = "Test Player";
    public LevelObject startingLocation;
    public Color playerColor;                   //TODO maybe use in a PlayerFaction class?
    public Faction faction;
    public int money;

    [Header("Player Handling")]
    public Action candidateAction;

    [Header("Internal Stuff")]
    public PlayerManager playerManager;
    public ResourceManager resourceManager;
    public PlayerCameraController pCamera;
    public PlayerMouseController pMouse;
    public PlayerBelongings pBelongings;
    public PlayerSelection pSelection;

    // Use this for initialization
    void Start () {
        playerManager = GetComponentInParent<PlayerManager>();
        resourceManager = playerManager.gameManager.ResourceManager();

        pCamera = GetComponentInChildren<PlayerCameraController>();
        pMouse = GetComponentInChildren<PlayerMouseController>();
        pBelongings = GetComponentInChildren<PlayerBelongings>();
        pSelection = GetComponentInChildren<PlayerSelection>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI () {
        pSelection.DrawSelection(resourceManager.gameManager, resourceManager);
    }
}
