using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private PlayerInputSystem playerInputSystem;
    public event EventHandler OnShootPerformed;
    public event EventHandler OnJumpPerformed;

    private void Awake()
    {
        Instance = this;
        playerInputSystem = new PlayerInputSystem();

        playerInputSystem.Enable();

        playerInputSystem.Main.Shoot.performed += Shoot_performed;
        playerInputSystem.Main.Jump.performed += Jump_performed;
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        OnJumpPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void Shoot_performed(InputAction.CallbackContext obj)
    {
        OnShootPerformed?.Invoke(this, EventArgs.Empty);
    }
}
