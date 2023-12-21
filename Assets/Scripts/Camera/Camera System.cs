using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera cam;

    [SerializeField]
    private float fieldofViewMax = 55;

    [SerializeField]
    private float fieldofViewMin = 10;

    float moveSpeed = 10f;
    float rotateSpeed = 100f;
    float dragPanSpeed = 0.5f;
    float zoomSpeed = 10f;

    private bool dragPanMoveActive;

    private Vector2 lastMousePos;

    private float targetFOV = 55;

    private Vector3[] defaults;

    private bool returningToDefault = false;

    private float lerpDuration = 1.0f;
    private float lerpProgress = 0.0f;

    private void Start()
    {
        defaults = new Vector3[2];
        defaults[0] = transform.position;
        defaults[1] = transform.eulerAngles;
    }

    void Update()
    {
        if (CameraSwitcher.Instance.getCurrentCamera() == cam && !returningToDefault)
        {
            if (!dragPanMoveActive)
            {
                HandleCameraMovement();
            }
            HandleCameraMovementDragPan();
            HandleCameraRotation();

            HandleCameraZoom();
        }
        HandleCameraReset();
    }

    private void HandleCameraReset()
    {
        if (Input.GetMouseButton(2))
        {
            returningToDefault = true;
        }

        if (returningToDefault && lerpProgress < lerpDuration)
        {
            lerpProgress += Time.deltaTime / lerpDuration;
            transform.position = Vector3.Lerp(transform.position, defaults[0], lerpProgress);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, defaults[1], lerpProgress);
        }
        else
        {
            returningToDefault = false;
            lerpProgress = 0.0f;
        }
    }

    private void HandleCameraZoom()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFOV -= 5;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFOV += 5;
        }

        targetFOV = Mathf.Clamp(targetFOV, fieldofViewMin, fieldofViewMax);

        cam.m_Lens.FieldOfView = Mathf.Lerp(
            cam.m_Lens.FieldOfView,
            targetFOV,
            Time.deltaTime * zoomSpeed
        );
    }

    private void HandleCameraMovement()
    {
        Vector3 inputDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            inputDir.z = 1f;
        if (Input.GetKey(KeyCode.S))
            inputDir.z = -1f;
        if (Input.GetKey(KeyCode.A))
            inputDir.x = -1f;
        if (Input.GetKey(KeyCode.D))
            inputDir.x = 1f;

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleCameraMovementDragPan()
    {
        Vector3 inputDir = Vector3.zero;
        if (Input.GetMouseButtonDown(1))
        {
            dragPanMoveActive = true;
            lastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(1))
        {
            dragPanMoveActive = false;
        }

        if (dragPanMoveActive)
        {
            Vector2 mouseMovementChange = (Vector2)Input.mousePosition - lastMousePos;

            inputDir.x = mouseMovementChange.x * dragPanSpeed;
            inputDir.z = mouseMovementChange.y * dragPanSpeed;

            lastMousePos = Input.mousePosition;
        }
        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
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
