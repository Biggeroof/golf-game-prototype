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

    private int lastParIndex = -1;
    private int[] prevPars;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        par = 0;
        ball.OnShootPerformed += Ball_OnShootPerformed;
        GoalManager.Instance.OnGoalReached += Ball_OnGoalReached;
        prevPars = new int[99];
    }

    private void Ball_OnGoalReached(object sender, EventArgs e)
    {
        lastParIndex += 1;
        prevPars[lastParIndex] = par;
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

    public int getMostRecentPar()
    {
        return prevPars[lastParIndex];
    }
}
