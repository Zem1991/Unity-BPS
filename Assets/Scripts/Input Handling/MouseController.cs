using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {
    public GameManager gm;
    public MousePointer mousePointer;

    public Vector3 mouseOriginalScreenPos;
    public float mouseDragDirection;
    public RaycastHit mouseHit;
    public RaycastHit mouseHit_GroundLayer;
    public bool didMouseHitSomething;
    public Vector3 mouseScenePosition;
    public Collider mouseHitCollider;

    // Use this for initialization
    public virtual void Start()
    {
        gm = FindObjectOfType<GameManager>();
        mousePointer = GetComponentInChildren<MousePointer>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        //WithinLevelBounds();
    }

    public virtual void MouseHandling(InputManager im)
    {
        bool lmbDown = im.lmbDown;
        bool lmbPress = im.lmbPress;
        bool lmbUp = im.lmbUp;

        if (!lmbDown && !lmbPress && !lmbUp)
        {
            mouseOriginalScreenPos = Input.mousePosition;
            mouseDragDirection = float.NaN;
        }
        else
        {
            Vector3 p1 = mouseOriginalScreenPos;
            Vector3 p2 = Input.mousePosition;
            mouseDragDirection = Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI;
        }

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        didMouseHitSomething = Physics.Raycast(mouseRay, out mouseHit);
        if (didMouseHitSomething)
        {
            if (Physics.Raycast(mouseRay, out mouseHit_GroundLayer, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")))
            {
                mouseScenePosition = mouseHit_GroundLayer.point;
                mouseHitCollider = mouseHit_GroundLayer.collider;
            }
        }

        mousePointer.transform.position = mouseScenePosition;
    }

    //private void WithinLevelBounds()
    //{
    //    if (gm.initializationDone)
    //    {
    //        target.transform.position = gm.levelPlayingArea.ClosestPoint(target.transform.position);
    //        //TODO FIND Y_COORD OF TILE, THEN SET Y_POS TO IT
    //    }
    //}
}
