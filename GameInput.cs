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

    private void Awake()
    {
        Instance = this; 
        playerInputSystem = new PlayerInputSystem();

        playerInputSystem.Enable();
    }
    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputSystem.Main.Move.ReadValue<Vector2>();

        return inputVector;
    }

}
