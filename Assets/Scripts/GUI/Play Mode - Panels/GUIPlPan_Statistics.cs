using BPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlPan_Statistics : GUIComponent_Panel
{
    public Text txt_1stPlaced;
    public Text txt_2ndPlaced;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        UpdateStatistics();
    }

    private void UpdateStatistics()
    {
        VoteCounter vc = screenManager.gameManager.voteCounter;
        List<PlayerIntentions> stats = new List<PlayerIntentions>(vc.SortVotesPerPlayer());
        txt_1stPlaced.color = stats[0].player.playerColor;
        txt_1stPlaced.text = stats[0].player.playerName + ", " + stats[0].playerPct + "%";
        txt_2ndPlaced.color = stats[1].player.playerColor;
        txt_2ndPlaced.text = stats[1].player.playerName + ", " + stats[1].playerPct + "%";
    }
}
