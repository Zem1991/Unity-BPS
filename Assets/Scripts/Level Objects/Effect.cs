using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour {
    [Header("Identification and Parameters")]
    public string effectName;
    public Sprite img_icon;
    public float durationTimerMax;
    public float repetitionTimerMax;
    public float stackMax;

    [Header("Execution Parameters")]
    public float durationTimer;
    public float repetitionTimer;
    public float stack;
    public LevelObject target;

    // Use this for initialization
    public virtual void Start () {
        target = GetComponentInParent<LevelObject>();

        if (stackMax < 1)
            stackMax = 1;
        stack = 1;

        if (repetitionTimerMax > durationTimerMax)
            repetitionTimerMax = durationTimerMax;

        repetitionTimer = repetitionTimerMax;
        durationTimer = durationTimerMax;
    }

    // Update is called once per frame
    public virtual void Update () {
        if (stack <= 0)
            Remove();

        HandleTimers();
    }

    private void HandleTimers()
    {
        if (repetitionTimer <= 0)
        {
            repetitionTimer = repetitionTimerMax;
            Apply(target);
        }
        else
        {
            repetitionTimer -= Time.deltaTime;
        }

        if (durationTimer <= 0)
        {
            durationTimer = durationTimerMax;
            stack--;
        }
        else
        {
            durationTimer -= Time.deltaTime;
        }
    }

    public void Increase()
    {
        if (stack < stackMax)
            stack++;

        durationTimer = durationTimerMax;
        repetitionTimer = repetitionTimerMax;
    }

    public void Remove()
    {
        target.activeEffects.Remove(this);
        Destroy(gameObject);
    }

    public virtual bool Apply(LevelObject levelObject)
    {
        if (!target)
            target = levelObject;

        return target;
    }
}
