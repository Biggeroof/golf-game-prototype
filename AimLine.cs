using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    [SerializeField]
    private float stopVelocity = .01f;

    [SerializeField]
    private float shotPower;

    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private GameInput gameInput;

    private bool ballIsIdle;
    private bool isAiming;
    private Vector3 lastIdlePos;
    private float jumpPower;

    private Rigidbody rb;

    public event EventHandler<OnToggleBallCanShootArgs> onToggleBallCanShoot;

    public class OnToggleBallCanShootArgs : EventArgs
    {
        public bool canShoot;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        isAiming = false;
        ballIsIdle = false;
        lineRenderer.enabled = false;
        lastIdlePos = Vector3.zero;
        jumpPower = 2;
    }

    private void Start()
    {
        GameInput.Instance.OnShootPerformed += Ball_OnShootPerformed;
        GameInput.Instance.OnJumpPerformed += Ball_OnJumpPerformed;
    }

    private void Ball_OnJumpPerformed(object sender, EventArgs e)
    {
        Jump();
    }

    private void Ball_OnShootPerformed(object sender, EventArgs e)
    {
        processAimVectorVisual(true);
    }

    private void FixedUpdate()
    {
        onToggleBallCanShoot?.Invoke(
            this,
            new OnToggleBallCanShootArgs { canShoot = GetCanShoot() }
        );
        Debug.Log(rb.velocity);
        if (rb.velocity.magnitude < stopVelocity)
        {
            Stop();
        }
        else
        {
            ballIsIdle = false;
        }
        //Debug.Log(rb.velocity);
        if (CheckVoidPos(rb.velocity))
        {
            gameObject.transform.position = lastIdlePos;
            Stop();
        }

        processAimVectorVisual(false);
    }

    private bool GetCanShoot()
    {
        return rb.velocity.magnitude < stopVelocity;
    }

    private void OnMouseDown()
    {
        if (ballIsIdle)
        {
            isAiming = true;
        }
    }

    private void processAimVectorVisual(bool shootInput)
    {
        if (!ballIsIdle || !isAiming)
        {
            return;
        }
        Vector3? worldPoint = RayCastMouseClick();
        if (!worldPoint.HasValue)
        {
            return;
        }
        else
        {
            DrawLine(worldPoint.Value);

            //check if click is raised
            if (shootInput)
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
        //clamping applied force
        Vector3 appliedForce = direction * magnitude * shotPower;
        Debug.Log(appliedForce.magnitude);
        if (appliedForce.magnitude > 1600f)
        {
            float multiplier = 1600f / appliedForce.magnitude;
            appliedForce = appliedForce * multiplier;
        }
        rb.AddForce(appliedForce);
    }

    private void DrawLine(Vector3 worldPoint)
    {
        Vector3[] positions = { transform.position, worldPoint };
        lineRenderer.SetPositions(positions);
        lineRenderer.enabled = true;
    }

    private void Stop()
    {
        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), 1f))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            ballIsIdle = true;
            lastIdlePos = gameObject.transform.position;
        }
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

    private bool CheckVoidPos(Vector3 velocity)
    {
        if (velocity.y < -12.87)
        {
            return true;
        }
        return false;
    }

    private void Jump()
    {
        if (rb.velocity != Vector3.zero)
        {
            Vector3 jumpForce = new Vector3(0, 300f * jumpPower, 0);
            rb.AddForce(jumpForce);
        }
    }
}
