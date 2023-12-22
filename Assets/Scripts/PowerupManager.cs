using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    [SerializeField]
    private BallLogic ball;

    [SerializeField]
    private PowerupScriptableObject[] possiblePowerups;

    public static PowerupManager instance { get; private set; }

    private ArrayList powerups = new ArrayList();

    public event EventHandler OnFreeJumpActivate;
    public event EventHandler OnFastBallActivate;
    public event EventHandler OnHoleMagnetActivate;

    void Awake()
    {
        instance = this;
        Powerup.onContact += Powerup_onContact;
        GameInput.Instance.OnPowerupPerformed += Instance_OnPowerupPerformed;
    }

    private void Instance_OnPowerupPerformed(object sender, PowerupEventArgs e)
    {
        //only able to activate powerups when ball is stopped
        if (ball.getIsStopped() && !GoalManager.Instance.getInHole())
        {
            Debug.Log(e.idx);
            Debug.Log(powerups.Count);
            if (powerups.Count - 1 >= e.idx)
            {
                PowerupScriptableObject powerupSO = (PowerupScriptableObject)powerups[e.idx];
                switch (powerupSO.powerupName)
                {
                    case "Freejump":
                        OnFreeJumpActivate?.Invoke(this, EventArgs.Empty);
                        powerups.RemoveAt(e.idx);
                        break;
                    case "Fastball":
                        OnFastBallActivate?.Invoke(this, EventArgs.Empty);
                        powerups.RemoveAt(e.idx);
                        break;
                    case "Holemagnet":
                        OnHoleMagnetActivate?.Invoke(this, EventArgs.Empty);
                        powerups.RemoveAt(e.idx);
                        break;
                }
                //update ui after this to reflect the removal of the powerup
                //either by calling an event or using the update function
            }
        }
    }

    private void Powerup_onContact(object sender, EventArgs e)
    {
        if (powerups.Count != 3)
        {
            powerups.Add(possiblePowerups[UnityEngine.Random.Range(0, possiblePowerups.Length)]);

            foreach (PowerupScriptableObject item in powerups)
            {
                Debug.Log(item.powerupName);
            }
        }
    }

    //handle powerup use (get input from user), and then fire events that get caught by the ball's logic handler
    //and toggle booleans in that file that change things
}
