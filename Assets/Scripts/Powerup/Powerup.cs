using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public static event EventHandler onContact;

    private void OnTriggerEnter(Collider other)
    {
        onContact?.Invoke(this, EventArgs.Empty);
        //hide the mesh and collider for the powerup for a certain amount of time/permanently
    }
}
