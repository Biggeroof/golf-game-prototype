using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBottomTransform : MonoBehaviour
{
    public static Vector3 getBottomForSphere(GameObject go)
    {
        Collider collider = go.GetComponent<Collider>();
        if (collider != null)
        {
            Vector3 bottomPosition = collider.bounds.min;

            // Adjust the x and z coordinates to the object's position if needed
            bottomPosition.x = go.transform.position.x;
            bottomPosition.z = go.transform.position.z;

            return bottomPosition;
        }
        //no collider
        return Vector3.negativeInfinity;
    }
}
