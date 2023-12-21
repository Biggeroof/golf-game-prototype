using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotRefPlane : MonoBehaviour
{
    private bool state = true;
    private BoxCollider planeCollider;
    private Quaternion defaultRotation;

    private void Start()
    {
        planeCollider = GetComponent<BoxCollider>();
        defaultRotation = transform.rotation;
    }

    private void Update() { }

    public void UpdateLocation(Vector3 pos)
    {
        transform.position = new Vector3(transform.position.x, pos.y, transform.position.z);
    }

    public void ToggleEnable()
    {
        state = !state;
        planeCollider.enabled = state;
        gameObject.SetActive(state);
        transform.rotation = defaultRotation;
    }

    //elevator mode (use this toggleenable instead of the other one)
    //public void ToggleEnable()
    //{
    //    state = !state;
    //    this.enabled = state;
    //}
}
