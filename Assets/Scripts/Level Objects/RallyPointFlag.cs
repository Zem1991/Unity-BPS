using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RallyPointFlag : MonoBehaviour
{
    private List<Renderer> renderers = new List<Renderer>();

    // Use this for initialization
    void Start()
    {
        renderers.AddRange(GetComponentsInChildren<Renderer>());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void show()
    {
        foreach (Renderer renderer in renderers)
            renderer.enabled = true;
    }

    public void hide()
    {
        foreach (Renderer renderer in renderers)
            renderer.enabled = false;
    }
}
