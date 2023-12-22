using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    private int numPowerups;

    [SerializeField]
    private BallLogic ball;

    [SerializeField]
    private LayerMask powerupLayer;

    [SerializeField]
    private PowerupScriptableObject[] possiblePowerups;

    public PowerupManager instance { get; private set; }

    private PowerupScriptableObject[] powerups;

    void Start()
    {
        instance = this;
        Powerup.onContact += Powerup_onContact;
        powerups = new PowerupScriptableObject[numPowerups];
    }

    private void Powerup_onContact(object sender, EventArgs e)
    {
        //get a random powerup and add it to the array
    }

    //handle powerup use (get input from user), and then fire events that get caught by the ball's logic handler
    //and toggle booleans in that file that change things

    void Update() { }
}
