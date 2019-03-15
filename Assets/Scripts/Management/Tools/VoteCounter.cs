using BPS;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VoteCounter : MonoBehaviour {
    public GameManager gm;
    public List<PlayerIntentions> playerIntentions;
    public int maxVotes;

    // Use this for initialization
    void Start () {
        gm = GetComponentInParent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (maxVotes <= 0)
            Debug.LogWarning("maxVotes <= 0 !");

    }

    public void GeneratePlayerIntentions(List<Player> players)
    {
        playerIntentions = new List<PlayerIntentions>();
        for (int i = 0; i < players.Count; i++)
        {
            PlayerIntentions pi = new PlayerIntentions();
            pi.player = players[i];
            pi.playerId = i;
            pi.playerVotes = 0;
            pi.playerPct = 0;

            playerIntentions.Add(pi);
        }
    }

    public void UpdatePlayerIntentions(int[] votesPerPlayer, int maxVotes)
    {
        this.maxVotes = maxVotes;
        for (int i = 0; i < playerIntentions.Count; i++)
        {
            PlayerIntentions pi = playerIntentions[i];
            pi.playerVotes = votesPerPlayer[i];
            pi.playerPct = (float) votesPerPlayer[i] / maxVotes * 100F;
            playerIntentions[i] = pi;
        }
    }

    public List<PlayerIntentions> SortVotesPerPlayer()
    {
        List<PlayerIntentions> result = new List<PlayerIntentions>(playerIntentions);
        //result.Sort((y, x) => x.playerPct.CompareTo(y.playerPct));
        result.Sort(delegate (PlayerIntentions x, PlayerIntentions y)
        {
            if (x.playerPct > y.playerPct) return -1;
            else if (x.playerPct < y.playerPct) return 1;
            else return -1;
        });

        return result;
    }
}
