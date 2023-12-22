using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handle ball movement logic ONLY (isIdle, stop, void, jumps)
public class BallLogic : MonoBehaviour
{
    private const float STOP_TIME = 0.5f;

    private float stopVelocity = 0.1f;
    public float delayAfterShoot = 0.3f;

    private float timeSinceForceApplied;
    private bool forceApplied;

    [SerializeField]
    private float shotPower = 100;

    [SerializeField]
    private float fastShotPower = 200;

    [SerializeField]
    private LayerMask floorLayerMask;

    [SerializeField]
    private AimLine aimLine;

    [SerializeField]
    private ShotRefPlane referencePlane;

    [SerializeField]
    private GravitationalPull hole;

    private bool ballIsIdle;
    private Vector3 lastIdlePos;
    private float jumpPower;
    private int jumpLimit;
    private Rigidbody rb;
    private float belowThresHoldDuration;

    private bool freeJumpActive;
    private bool fastBallActive;

    public EventHandler OnShootPerformed;

    private void Ball_OnJumpPerformed(object sender, EventArgs e)
    {
        if (GoalManager.Instance.getInHole() == false)
        {
            Jump(freeJumpActive);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ballIsIdle = true;
        jumpPower = 2;
        jumpLimit = 3;
        belowThresHoldDuration = 0f;
    }

    private void Start()
    {
        GameInput.Instance.OnJumpPerformed += Ball_OnJumpPerformed;
        lastIdlePos = transform.position;
        PowerupManager.instance.OnFastBallActivate += Instance_OnFastBallActivate;
        PowerupManager.instance.OnFreeJumpActivate += Instance_OnFreeJumpActivate;
    }

    private void Instance_OnFreeJumpActivate(object sender, EventArgs e)
    {
        freeJumpActive = true;
    }

    private void Instance_OnFastBallActivate(object sender, EventArgs e)
    {
        fastBallActive = true;
    }

    void FixedUpdate()
    {
        if (GoalManager.Instance.getInHole() == false)
        {
            if (CheckVoidPos(rb.velocity))
            {
                gameObject.transform.position = lastIdlePos;

                Stop(false);
            }
            if (!ballIsIdle)
            {
                if (forceApplied)
                {
                    timeSinceForceApplied += Time.deltaTime;
                    if (timeSinceForceApplied > delayAfterShoot)
                    {
                        forceApplied = false;
                        timeSinceForceApplied = 0f;
                    }
                }

                //start count for time that ball is below the speed threshold
                if (rb.velocity.magnitude <= stopVelocity && !forceApplied)
                {
                    belowThresHoldDuration += Time.deltaTime;
                }

                if (
                    rb.velocity.magnitude <= stopVelocity
                    && !forceApplied
                    && belowThresHoldDuration > STOP_TIME
                )
                {
                    belowThresHoldDuration = 0f;
                    Stop(false);
                }
                else
                {
                    ballIsIdle = false;
                }

                if (rb.velocity.magnitude > stopVelocity)
                {
                    belowThresHoldDuration = 0f;
                }
                if (hole != null)
                {
                    hole.Attract(rb);
                }
            }
        }
    }

    public void Shoot(Vector3 dir)
    {
        jumpLimit = 3;
        ballIsIdle = false;
        Vector3 horizontalMovementVector = new Vector3(dir.x, transform.position.y, dir.z);
        Vector3 direction = (horizontalMovementVector - transform.position).normalized;
        float magnitude = Vector3.Distance(transform.position, horizontalMovementVector);

        Vector3 appliedForce =
            fastBallActive == true
                ? direction * magnitude * fastShotPower
                : direction * magnitude * shotPower;

        if (appliedForce.magnitude > 600f)
        {
            float multiplier = 600f / appliedForce.magnitude;
            appliedForce = appliedForce * multiplier;
        }

        //reverse direction of vector
        appliedForce.z = -appliedForce.z;
        appliedForce.x = -appliedForce.x;

        rb.AddForce(appliedForce);
        forceApplied = true;
        timeSinceForceApplied = 0f;
        aimLine.setIsAiming(false);
        referencePlane.ToggleEnable();
        OnShootPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void Stop(bool ignoreConds)
    {
        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), 1f) || ignoreConds)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            ballIsIdle = true;
            lastIdlePos = gameObject.transform.position;
            referencePlane.ToggleEnable();
            referencePlane.UpdateLocation(GetBottomTransform.getBottomForSphere(this.gameObject));
            //shot after powerups are activated, disable them
            freeJumpActive = false;
            fastBallActive = false;
        }
    }

    private bool CheckVoidPos(Vector3 velocity)
    {
        if (
            (velocity.y < -12.87 && ballIsIdle)
            || Physics.Raycast(transform.position, new Vector3(0, -1, 0), 0.1f, floorLayerMask)
        )
        {
            return true;
        }
        return false;
    }

    private void Jump(bool ignoreFloor)
    {
        if (!ignoreFloor)
        {
            if (
                jumpLimit != 0
                && Physics.Raycast(transform.position, new Vector3(0, -1, 0), 0.1f)
                && !ballIsIdle
            )
            {
                Vector3 jumpForce = new Vector3(
                    (float)(rb.velocity.x * 0.05),
                    50f * jumpPower,
                    (float)(rb.velocity.z * 0.05)
                );
                rb.AddForce(jumpForce);
                jumpLimit -= 1;
            }
        }
        else
        {
            if (jumpLimit != 0 && !ballIsIdle)
            {
                Vector3 jumpForce = new Vector3(
                    (float)(rb.velocity.x * 0.05),
                    50f * jumpPower,
                    (float)(rb.velocity.z * 0.05)
                );
                rb.AddForce(jumpForce);
                jumpLimit -= 1;
            }
        }
    }

    public int getJumpsLeft()
    {
        return jumpLimit;
    }

    public Vector3 getLocation()
    {
        return transform.position;
    }

    public bool getIsStopped()
    {
        return ballIsIdle;
    }
}
