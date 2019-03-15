using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject_Component : MonoBehaviour {
    public bool applyPlayerColor = true;
    public Color originalColor;
    private LevelObject levelObject;

    // Use this for initialization
    void Start () {
        levelObject = GetComponentInParent<LevelObject>();
        if (GetComponent<Renderer>())
            originalColor = GetComponent<Renderer>().material.color;
        else
            originalColor = Color.gray;
    }

    public Vector3 getPosition()
    {
        return transform.position;
    }

    public Quaternion getRotation()
    {
        return transform.rotation;
    }

    public LevelObject getLevelObject()
    {
        return levelObject;
    }
}
