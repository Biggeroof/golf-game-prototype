using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public static CameraSwitcher Instance { get; private set; }

    [SerializeField]
    CinemachineVirtualCamera[] cams;

    private int currentCam = 0;

    private CinemachineVirtualCamera currentCamera;

    void Start()
    {
        Instance = this;
        GameInput.Instance.OnSwitchCamsPerformed += Instance_OnSwitchCamsPerformed;
        currentCamera = (CinemachineVirtualCamera)cams.GetValue(currentCam);
    }

    private void Instance_OnSwitchCamsPerformed(object sender, System.EventArgs e)
    {
        currentCam += 1;
        currentCam = currentCam % cams.Length;
        currentCamera = (CinemachineVirtualCamera)cams.GetValue(currentCam);
        foreach (CinemachineVirtualCamera cam in cams)
        {
            if (cams.GetValue(currentCam) != cam)
            {
                cam.Priority = 0;
            }
            else
            {
                cam.Priority = 1;
            }
        }
    }

    public CinemachineVirtualCamera getCurrentCamera()
    {
        return currentCamera;
    }
}
