using BPS.Population;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityObject : LevelObject {
    [Header("CityObject parameters")]
    public PurposeZoning purposeZoniong;
    public EconomicalZoning economicalZoning;
    public PoliticalPosition politicalPosition;
    public ReligionLevel religionLevel;
    public EducationLevel educationLevel;
    public SoccerFanaticism soccerFanaticism;
    public VotingIntentions votingIntentions;
    public Player influentialPlayer;

    protected override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    protected override void Start () {
        base.Start();

        votingIntentions = gameObject.AddComponent<VotingIntentions>();
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
	}

    public void generateDetails(PopulationManager pm)
    {
        Debug.Log("generateDetails(PopulationManager pm)");
        //religionLevel = pm.generateReligionLevel();
        //educationLevel = pm.generateEducationLevel();
        //politicalPosition = pm.generatePoliticalPosition();

        //pm.updateBuildingCounters(economicalClass, religionLevel, educationLevel, politicalPosition);
    }

    public float getRating(Player player)
    {
        return votingIntentions.getRating(player);
    }

    public void setRating(Player player, float amount)
    {
        votingIntentions.setRating(player, amount);
        changeMaterialColors(player);
    }

    public void setStress(int amount)
    {
        votingIntentions.setStress(amount);
    }

    public int selectCandidateFromVotingIntetions()
    {
        int result = -1;        //BLANK OR NULL VOTES!
        float resultPct = 0;

        for (int i = 0; i < votingIntentions.allPlayers.Count; i++)
        {
            //TODO CHECK FOR DRAW CASES
            if (resultPct < votingIntentions.ratings[i])
            {
                result = i;
                resultPct = votingIntentions.ratings[i];
            }
        }

        return result;
    }

    private void changeMaterialColors(Player player)
    {
        float t = votingIntentions.getRating(player) / 100F;
        foreach (LevelObject_Component locw in locws)
        {
            if (locw.applyPlayerColor)
            {
                Color c = Color.Lerp(locw.originalColor, player.playerColor, t);
                locw.GetComponent<Renderer>().material.SetColor("_Color", c);
            }
        }
    }
}
