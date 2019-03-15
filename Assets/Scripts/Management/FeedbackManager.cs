using BPS.InGame.Error;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour {
    public GameManager gameManager;
    public SMMode_Play_Feedback smPlFeedback;

    [Header("Specific Error Messages (Context)")]
    public const string CANNOT_MOVE = "Selected object is not capable of movement!";
    public const string NO_RALLY_POINT = "Selected object does not have an rally point!";

    // Use this for initialization
    void Start () {
        gameManager = FindObjectOfType<GameManager>();

        SMMode_Play smPl = gameManager.ScreenManager().playMode;
        smPlFeedback = smPl.smPlFeedback;
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void ContextMsg_ActionError(string msg)
    {
        smPlFeedback.ContextMsg(msg);
    }
}
