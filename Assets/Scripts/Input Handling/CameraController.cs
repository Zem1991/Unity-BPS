using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameManager gm;
    public CameraHolder cameraHolder;
    public GameObject cameraTarget;

    public Vector3 initialCameraHolderPosition;
    public Quaternion initialCameraHolderRotation;
    public Vector3 initialCameraTargetPosition;
    public Quaternion initialCameraTargetRotation;

    // Use this for initialization
    public virtual void Start () {
        gm = FindObjectOfType<GameManager>();
        cameraHolder = GetComponentInChildren<CameraHolder>();
        cameraTarget = gameObject;

        cameraHolder.transform.LookAt(cameraTarget.transform);

        initialCameraHolderPosition = cameraHolder.transform.position;
        initialCameraHolderRotation = cameraHolder.transform.rotation;
        initialCameraTargetPosition = cameraTarget.transform.position;
        initialCameraTargetRotation = cameraTarget.transform.rotation;
    }

    // Update is called once per frame
    public virtual void Update () {
        WithinLevelBounds();
	}

    private void WithinLevelBounds()
    {
        if (gm.fullInitializationDone)
        {
            cameraTarget.transform.position = gm.levelPlayingArea.ClosestPoint(cameraTarget.transform.position);
            //TODO FIND Y_COORD OF TILE, THEN SET Y_POS TO IT
        }
    }

    public virtual void CameraControl(InputManager im)
    {
        int moveUp = Convert.ToInt32(im.arrowUpPress) + Convert.ToInt32(im.mouseOnScreenTop);
        int moveLeft = Convert.ToInt32(im.arrowLeftPress) + Convert.ToInt32(im.mouseOnScreenLeft);
        int moveDown = Convert.ToInt32(im.arrowDownPress) + Convert.ToInt32(im.mouseOnScreenBottom);
        int moveRight = Convert.ToInt32(im.arrowRightPress) + Convert.ToInt32(im.mouseOnScreenRight);
        int zoomingOut = Convert.ToInt32(im.pgUpPress) + Convert.ToInt32(im.mouseAxisScrollWheel > 0);
        int zoomingIn = Convert.ToInt32(im.pgDownPress) + Convert.ToInt32(im.mouseAxisScrollWheel < 0);
        int rotClock = Convert.ToInt32(im.np07Press);
        int rotCounter = Convert.ToInt32(im.np09Press);
        bool shift = im.shiftPress;

        cameraMovement(
            (shift ? 0 : im.cameraMovementSpeed),
            moveDown, moveUp,
            moveLeft, moveRight
            );
        cameraZoom(
            im.cameraZoomSpeed,
            zoomingIn, zoomingOut
            );
        cameraRotationH(
            im.cameraRotationSpeed,
            rotClock, rotCounter
            );
        cameraRevolutionV(
            (shift ? im.cameraRevolutionSpeed : 0),
            moveDown, moveUp
            );
        cameraRevolutionH(
            (shift ? im.cameraRevolutionSpeed : 0),
            moveLeft, moveRight
            );
    }

    public virtual void CameraHotkeys(InputManager im)
    {
        if (im.endDown)
            cameraDefaultRotation();
    }

    public void cameraMovement(int cameraMovementSpeed,
        int moveBackward, int moveForward,
        int moveLeft, int moveRight)
    {
        Vector3 originalPos = cameraTarget.transform.position;

        Vector3 movement = new Vector3();
        movement.z = cameraMovementSpeed * (-moveBackward + moveForward);
        movement.x = cameraMovementSpeed * (-moveLeft + moveRight);
        cameraTarget.transform.Translate(movement, Space.Self);

        if (!gm.pointInsideLevelBounds(cameraTarget.transform.position))
        {
            cameraTarget.transform.position = originalPos;
        }
    }

    public void cameraZoom(int cameraZoomSpeed,
        int zoomingIn, int zoomingOut)
    {
        Vector3 movement = new Vector3();
        movement.z = cameraZoomSpeed * (-zoomingIn + zoomingOut);
        cameraHolder.transform.Translate(movement, Space.Self);
    }

    public void cameraRotationH(int cameraRotationSpeed,
        int rotClock, int rotCounter)
    {
        int speed = cameraRotationSpeed * (-rotClock + rotCounter);
        cameraTarget.transform.RotateAround(cameraHolder.transform.position, Vector3.up, speed);
    }

    public void cameraRevolutionV(int cameraRevolutionSpeed,
        int revClock, int revCounter)
    {
        int speed = cameraRevolutionSpeed * (-revClock + revCounter);
        cameraHolder.transform.RotateAround(transform.position, transform.right, speed);
    }

    public void cameraRevolutionH(int cameraRevolutionSpeed,
        int revClock, int revCounter)
    {
        Vector3 originalPos = cameraTarget.transform.position;

        int speed = cameraRevolutionSpeed * (-revClock + revCounter);
        cameraHolder.transform.RotateAround(transform.position, transform.up, speed);

        if (!gm.pointInsideLevelBounds(cameraTarget.transform.position))
        {
            cameraTarget.transform.position = originalPos;
            cameraRotationH(cameraRevolutionSpeed / 2, revClock, revCounter);
        }
    }

    public void cameraDefaultRotation()
    {
        //cameraHolder.transform.position = initialCameraPosition;
        cameraHolder.transform.rotation = initialCameraHolderRotation;
        cameraTarget.transform.rotation = initialCameraTargetRotation;
    }
}
