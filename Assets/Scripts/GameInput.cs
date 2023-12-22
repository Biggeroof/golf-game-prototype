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
    public event EventHandler OnSwitchCamsPerformed;
    public event EventHandler<PowerupEventArgs> OnPowerupPerformed;

    private void Awake()
    {
        Instance = this;
        playerInputSystem = new PlayerInputSystem();

        playerInputSystem.Enable();

        playerInputSystem.Main.Shoot.performed += Shoot_performed;
        playerInputSystem.Main.Jump.performed += Jump_performed;
        playerInputSystem.Main.SwitchCams.performed += SwitchCams_performed;
        playerInputSystem.Main.Powerup1.performed += Powerup1_performed;
        playerInputSystem.Main.Powerup2.performed += Powerup2_performed;
        playerInputSystem.Main.Powerup3.performed += Powerup3_performed;
    }

    private void Powerup3_performed(InputAction.CallbackContext obj)
    {
        OnPowerupPerformed?.Invoke(this, new PowerupEventArgs(2));
    }

    private void Powerup2_performed(InputAction.CallbackContext obj)
    {
        OnPowerupPerformed?.Invoke(this, new PowerupEventArgs(1));
    }

    private void Powerup1_performed(InputAction.CallbackContext obj)
    {
        OnPowerupPerformed?.Invoke(this, new PowerupEventArgs(0));
    }

    private void SwitchCams_performed(InputAction.CallbackContext obj)
    {
        OnSwitchCamsPerformed?.Invoke(this, EventArgs.Empty);
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

public class PowerupEventArgs
{
    public int idx;

    public PowerupEventArgs(int idx)
    {
        this.idx = idx;
    }
}
