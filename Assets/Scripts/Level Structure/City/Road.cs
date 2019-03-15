using BPS.Map;
using BPS.Population;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour {
    public List<Tile> tiles = new List<Tile>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        foreach (var item in tiles)
        {
            Gizmos.DrawWireCube(item.transform.position, item.transform.lossyScale);
        }
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 1);
    }

    public void centralizePosition()
    {
        Vector3 avgPos = Vector3.zero;
        foreach (Tile item in tiles)
        {
            avgPos += item.transform.position;
        }
        avgPos /= tiles.Count;
        transform.position = avgPos;
    }
}
