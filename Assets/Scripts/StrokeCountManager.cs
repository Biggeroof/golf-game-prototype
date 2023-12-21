using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrokeCountManager : MonoBehaviour
{
    public static StrokeCountManager instance { get; private set; }

    [SerializeField]
    BallLogic ball;

    private int par;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        par = 0;
        ball.OnShootPerformed += Ball_OnShootPerformed;
        GoalManager.Instance.OnGoalReached += Ball_OnGoalReached;
    }

    private void Ball_OnGoalReached(object sender, EventArgs e)
    {
        par = 0;
    }

    private void Ball_OnShootPerformed(object sender, EventArgs e)
    {
        par += 1;
    }

    public int getPar()
    {
        return par;
    }
}
