using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePCInput
{
    private GameActionInput _input = new();

    public GamePCInput()
    {
        _input.Enable(); // Kích hoạt GameInputAction
    }

    /// <summary> Enable snap </summary>
    public event Action<InputAction.CallbackContext> SnapPerformed
    {
        add
        {
            _input.Player.Snap.performed += value;
        }
        remove
        {
            _input.Player.Snap.performed -= value;
        }
    }

    /// <summary> Enable snap </summary>
    public event Action<InputAction.CallbackContext> MenuSettings
    {
        add
        {
            _input.Player.MenuSettings.performed += value;
        }
        remove
        {
            _input.Player.MenuSettings.performed -= value;
        }
    }

    public Vector2 MovementInput()
    {
        return _input.Player.Move.ReadValue<Vector2>();
    }

}

