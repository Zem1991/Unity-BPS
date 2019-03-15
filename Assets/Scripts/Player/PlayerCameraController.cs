using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : CameraController {
    public Player player;

    //public Vector3 initialPlayerPosition;
    //public Quaternion initialPlayerRotation;
    //public Vector3 hqPosition;

    // Use this for initialization
    public override void Start () {
        base.Start();

        player = GetComponentInParent<Player>();
        //target = player as GameObject;

        //initialPlayerPosition = cameraTarget.transform.position;
        //initialPlayerRotation = cameraTarget.transform.rotation;
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
    }

    public override void CameraControl(InputManager im)
    {
        base.CameraControl(im);
    }

    public override void CameraHotkeys(InputManager im)
    {
        base.CameraHotkeys(im);

        if (im.homeDown)
            cameraOverHQ();
    }

    public void cameraOverHQ()
    {
        throw new NotImplementedException();
    }

    public void cameraOverSelection()
    {
        List<PlayerObject> pObjects = new List<PlayerObject>();
        pObjects = player.pSelection.selectedPlayerObjects;
        CityObject cObject = player.pSelection.selectedCityObject;

        if (pObjects.Count > 0)
        {
            //TODO make a midpoint.position from all selected objects if there is more than 1
            cameraTarget.transform.position = pObjects[0].transform.position;
        }
        else if (cObject)
        {
            cameraTarget.transform.position = cObject.transform.position;
        }
    }
}
