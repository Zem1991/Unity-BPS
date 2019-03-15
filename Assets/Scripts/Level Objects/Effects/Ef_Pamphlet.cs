using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ef_Pamphlet : Effect
{
    public float baseEffect;

    // Use this for initialization
    public override void Start () {
        base.Start();

        stackMax = 3;
	}

    // Update is called once per frame
    public override void Update () {
        base.Update();
	}

    public override bool Apply(LevelObject levelObject)
    {
        if (base.Apply(levelObject))
        {
            //NOTHING NEEDS TO BE DONE HERE.
            //THE ACTUAL EFFECT IS APPLIED WHEN THE ABILITY IS USED.
            return true;
        }
        return false;
    }
}
