using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public const float IN_HOLE_DURATION = 1f;
    public static GoalManager Instance { get; private set; }
    public event EventHandler OnGoalReached;

    [SerializeField]
    private BallLogic ball;

    [SerializeField]
    private LayerMask goalLayerMask;

    private bool inHole;

    private float inHoleDuration;

    private bool eventFired;

    private void Awake()
    {
        Instance = this;
        inHoleDuration = 0f;
        inHole = false;
    }

    void Update()
    {
        //start count
        if (inHole)
        {
            inHoleDuration += Time.deltaTime;
        }
        else
        {
            inHoleDuration = 0f;
        }

        if (inHoleDuration > IN_HOLE_DURATION && inHole)
        {
            if (!eventFired)
            {
                Debug.Log("Goal Event Fired");
                OnGoalReached?.Invoke(this, EventArgs.Empty);
                inHoleDuration = 0f;
                eventFired = true;
            }
        }
        inHole = checkGoal();
    }

    private bool checkGoal()
    {
        return Physics.Raycast(ball.transform.position, new Vector3(0, -1, 0), 0.1f, goalLayerMask);
    }

    public void setInHole(bool inHole)
    {
        this.inHole = inHole;
    }

    public bool getInHole()
    {
        return inHole;
    }
}
