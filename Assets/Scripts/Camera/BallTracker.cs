using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTracker : MonoBehaviour
{
    [SerializeField]
    private BallLogic ball;

    [SerializeField]
    private CinemachineVirtualCamera cam;

    float rotateSpeed = 100f;

    void Update()
    {
        transform.position = ball.transform.position;
        if (CameraSwitcher.Instance.getCurrentCamera() == cam)
        {
            HandleCameraRotation();
        }
    }

    private void HandleCameraRotation()
    {
        float rotateDir = 0f;
        if (Input.GetKey(KeyCode.Q))
            rotateDir = 1f;
        if (Input.GetKey(KeyCode.E))
            rotateDir = -1f;

        transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
    }
}
