using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalPull : MonoBehaviour
{
    public float radius = 0.15f;

    public float magnitude = 5f;

    private bool strongHolemagnetActive = false;

    private float strongMagnetTimeMax = 3f;

    private float strongMagnetTime = 0f;

    private void Start()
    {
        PowerupManager.instance.OnHoleMagnetActivate += Instance_OnHoleMagnetActivate;
    }

    private void Update()
    {
        if (strongHolemagnetActive)
        {
            strongMagnetTime += Time.deltaTime;
            if (strongMagnetTime >= strongMagnetTimeMax)
            {
                strongMagnetTime = 0f;
                strongHolemagnetActive = false;
                radius = 0.15f;
                magnitude = 5f;
            }
        }
    }

    private void Instance_OnHoleMagnetActivate(object sender, System.EventArgs e)
    {
        strongHolemagnetActive = true;
        radius = 0.5f;
        magnitude = 10f;
    }

    public void Attract(Rigidbody bodyToAttract)
    {
        Vector3 direction = transform.position - bodyToAttract.position;
        float distance = direction.magnitude;

        if (distance == 0f || distance > radius)
            return;

        //use constant to get ball into hole, don't need to calculate force of gravity
        Vector3 force = direction.normalized * magnitude;

        bodyToAttract.AddForce(force);
    }
}
