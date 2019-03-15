using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseController : MouseController {
    public Player player;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        player = GetComponentInParent<Player>();
        //target = player as GameObject;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
