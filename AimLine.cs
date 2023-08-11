using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    [SerializeField]
    private float stopVelocity = .05f;

    [SerializeField]
    private float shotPower;

    [SerializeField]
    private LineRenderer lineRenderer;

    private bool ballIsIdle;
    private bool isAiming;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        isAiming = false;
        ballIsIdle = false;
        lineRenderer.enabled = false;
    }

    private void FixedUpdate()
    {
        Debug.Log(rb.velocity.magnitude);
        if (rb.velocity.magnitude < stopVelocity)
        {
            Stop();
        }
        else
        {
            ballIsIdle = false;
        }
        processAimVectorVisual();
    }

    private void OnMouseDown()
    {
        if (ballIsIdle)
        {
            isAiming = true;
        }
    }

    private void processAimVectorVisual()
    {
        if (!ballIsIdle || !isAiming)
        {
            return;
        }
        Vector3? worldPoint = RayCastMouseClick();
        Debug.Log(worldPoint);
        if (!worldPoint.HasValue)
        {
            return;
        }
        else
        {
            DrawLine(worldPoint.Value);

            //check if click is raised
            if (Input.GetMouseButtonUp(0))
            {
                Shoot((Vector3)worldPoint);
            }
        }
    }

    private void Shoot(Vector3 worldPoint)
    {
        isAiming = false;
        ballIsIdle = false;
        lineRenderer.enabled = false;
        Vector3 horizontalMovementVector = new Vector3(
            worldPoint.x,
            transform.position.y,
            worldPoint.z
        );
        Vector3 direction = (horizontalMovementVector - transform.position).normalized;
        float magnitude = Vector3.Distance(transform.position, horizontalMovementVector);

        rb.AddForce(direction * magnitude * shotPower);
    }

    private void DrawLine(Vector3 worldPoint)
    {
        Vector3[] positions = { transform.position, worldPoint };
        lineRenderer.SetPositions(positions);
        lineRenderer.enabled = true;
    }

    private void Stop()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        ballIsIdle = true;
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
        Debug.Log("mouse far" + worldMousePosFar);
        Debug.Log("mouse near" + worldMousePosNear);
        RaycastHit hit;
        if (
            Physics.Raycast(
                worldMousePosNear,
                worldMousePosFar - worldMousePosNear,
                out hit,
                float.PositiveInfinity
            )
        )
        {
            return hit.point;
        }
        else
        {
            return null;
        }
    }
}
