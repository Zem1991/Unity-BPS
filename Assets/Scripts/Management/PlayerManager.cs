using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public GameManager gameManager;

    public Player localPlayer;
    public List<Player> allPlayers = new List<Player>();

    [Header("Stuff for defaults")]
    public Faction defaultFaction;

    [Header("Execution Options")]
    public bool useBasicColorSchemes;

    [Header("Execution Parameters")]
    public Material[] basicColorSchemes = new Material[GameManager.MAX_PLAYERS];

    // Use this for initialization
    void Start () {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update () {

    }

    public static List<PlayerObject> FindPlayerObjects(PlayerObject functionCaller, Vector3 position, float maxRange, bool ignoreAlive, bool ignoreDead)
    {
        List<PlayerObject> playerObjects = new List<PlayerObject>(FindObjectsOfType<PlayerObject>());
        playerObjects.Remove(functionCaller);

        List<PlayerObject> result = new List<PlayerObject>();
        foreach (var item in playerObjects)
        {
            if (Vector3.Distance(position, item.transform.position) <= maxRange)
            {
                if (!(ignoreAlive && !item.isDead) &&
                    !(ignoreDead && item.isDead))
                    result.Add(item);
            }
        }

        return playerObjects;
    }

    public static List<GroupActionHandler> FindGroupActionHandlers()
    {
        return new List<GroupActionHandler>(FindObjectsOfType<GroupActionHandler>());
    }
}
