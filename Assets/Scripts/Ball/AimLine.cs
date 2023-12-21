using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handle the aim line drawing
public class AimLine : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    //[SerializeField]
    //private ShotRefPlane shotRefPlane;

    [SerializeField]
    private BallLogic ball;

    [SerializeField]
    private CinemachineVirtualCamera cam;

    [SerializeField]
    private LayerMask reflayer;

    private bool isAiming;

    private void Awake()
    {
        isAiming = false;
        lineRenderer.enabled = false;
    }

    void Start()
    {
        GameInput.Instance.OnShootPerformed += Ball_OnShootPerformed;
        //shotRefPlane.UpdateLocation(GetBottomTransform.getBottomForSphere(gameObject));

        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;
    }

    private void Ball_OnShootPerformed(object sender, EventArgs e)
    {
        if (GoalManager.Instance.getInHole() == false)
        {
            processAimVectorVisual(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (
            GoalManager.Instance.getInHole() == false
            && CameraSwitcher.Instance.getCurrentCamera() == cam
        )
        {
            processAimVectorVisual(false);
        }
        else
        {
            DrawLine(transform.position);
        }
    }

    private void OnMouseDown()
    {
        if (
            ball.getIsStopped()
            && checkMouseOnBall(RayCastMouseClick())
            && CameraSwitcher.Instance.getCurrentCamera() == cam
        )
        {
            isAiming = true;
        }
    }

    private void processAimVectorVisual(bool shootInput)
    {
        if (!ball.getIsStopped() || !isAiming)
        {
            return;
        }
        //invert the location of the mouseclick so dragging back changes the line, dont need to check if raycasting w terrain and cap visuals length
        Vector3? worldPoint = RayCastMouseClick();
        //cap

        if (!worldPoint.HasValue)
        {
            return;
        }
        else
        {
            /* worldPoint = new Vector3(
                 -worldPoint.Value.x,
                 transform.position.y,
                 -worldPoint.Value.z
             );*/
            Vector3 vec = worldPoint.Value;
            float distance = Math.Abs((vec - transform.position).magnitude);

            if (distance > 1f)
            {
                vec = (vec - transform.position).normalized;
                vec = transform.position + vec;
            }

            DrawLine(vec);

            //check if button is pressed
            if (shootInput)
            {
                ball.Shoot(vec);
                lineRenderer.enabled = false;
            }
        }
    }

    private void DrawLine(Vector3 worldPoint)
    {
        Vector3 v3 = new Vector3(worldPoint.x, transform.position.y, worldPoint.z);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, v3);
        lineRenderer.enabled = true;
    }

    private Vector3? RayCastMouseClick()
    {
        Vector3 screenMousePositionFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane
        );
        Vector3 screenMousePositionNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane
        );

        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePositionFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePositionNear);
        //Debug.Log("mouse far" + worldMousePosFar);
        //Debug.Log("mouse near" + worldMousePosNear);
        RaycastHit hit;
        if (
            Physics.Raycast(
                worldMousePosNear,
                worldMousePosFar - worldMousePosNear,
                out hit,
                float.PositiveInfinity,
                reflayer
            )
        )
        {
            return new Vector3(hit.point.x, 0, hit.point.z);
        }
        else
        {
            return null;
        }
    }

    public bool checkMouseOnBall(Vector3? pos)
    {
        if (pos.HasValue)
        {
            return Math.Abs(pos.Value.x - ball.transform.position.x) < 0.06f
                && Math.Abs(pos.Value.z - ball.transform.position.z) < 0.06f;
        }
        else
        {
            return false;
        }
    }

    public void setIsAiming(bool state)
    {
        isAiming = state;
    }
}
