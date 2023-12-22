using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalPull : MonoBehaviour
{
    public float radius = 0.15f;

    public float magnitude = 5f;

    public void Attract(Rigidbody bodyToAttract)
    {
        Vector3 direction = transform.position - bodyToAttract.position;
        float distance = direction.magnitude;

        if (distance == 0f || distance > radius)
            return;

        Debug.Log("pull active");
        //use constant to get ball into hole, don't need to calculate force of gravity
        Vector3 force = direction.normalized * magnitude;

        bodyToAttract.AddForce(force);
    }
}
