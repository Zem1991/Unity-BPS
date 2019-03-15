using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SMMode_Play_Feedback : MonoBehaviour {
    [Header("GUI Elements")]
    public Text txt_contextMsg;

    [Header("GUI Initial Variables")]
    public float contextMsgTimerMax;

    [Header("GUI Dynamic Variables")]
    public float contextMsgTimer;

    // Use this for initialization
    void Start () {
        ClearEverything();
    }
	
	// Update is called once per frame
	void Update () {
        ApplyTimers();
        CheckTimers();
    }

    public void ClearEverything()
    {
        txt_contextMsg.text = "";
        contextMsgTimer = 0;
    }

    public void ApplyTimers()
    {
        if (contextMsgTimer > 0)
            contextMsgTimer -= Time.deltaTime;
    }

    public void CheckTimers()
    {
        if (contextMsgTimer <= 0)
        {
            txt_contextMsg.text = "";
            contextMsgTimer = 0;
        }
    }

    public void ContextMsg(string msg)
    {
        txt_contextMsg.text = msg;
        contextMsgTimer = contextMsgTimerMax;
    }
}
