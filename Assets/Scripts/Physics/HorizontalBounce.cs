using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HorizontalBounce : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;

        if (rb != null)
        {
            Vector3 velocity = rb.velocity;
            velocity.y = 0;

            rb.velocity = velocity;
        }
    }
}
