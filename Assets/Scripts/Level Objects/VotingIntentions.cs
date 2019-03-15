using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VotingIntentions : MonoBehaviour {
    public List<Player> allPlayers;
    public float[] ratings;
    public int stress;

	// Use this for initialization
	void Start () {
        PlayerManager pm = FindObjectOfType<PlayerManager>();
        allPlayers = new List<Player>(pm.allPlayers);
        ratings = new float[allPlayers.Count];
        stress = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float getRating(Player player)
    {
        int index = allPlayers.IndexOf(player);
        return ratings[index];
    }

    public void setRating(Player player, float amount)
    {
        int index = allPlayers.IndexOf(player);
        amount = Mathf.Clamp(amount, 0, 100);
        ratings[index] = amount;
    }

    public void setStress(int amount)
    {
        int value = stress + amount;
        value = Mathf.Clamp(value, 0, 100);
        stress = value;
    }
}
