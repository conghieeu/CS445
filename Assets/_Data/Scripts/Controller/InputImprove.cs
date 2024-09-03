using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputImprove
{
    private GameActionInput _input;

    public InputImprove()
    {
        _input = new();
        _input.Enable();
    }

    #region Input Action
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

    public event Action<InputAction.CallbackContext> DragPerformed
    {
        add
        {
            _input.Player.Drag.performed += value;
            CallDrag += value;
        }
        remove
        {
            _input.Player.Drag.performed -= value;
            CallDrag -= value;
        }
    }

    public event Action<InputAction.CallbackContext> ShowInfo
    {
        add
        {
            _input.Player.ShowInfo.performed += value;
            AShowInfo += value;
        }
        remove
        {
            _input.Player.ShowInfo.performed -= value;
            AShowInfo -= value;
        }
    }

    public Vector2 MovementInput()
    {
        return _input.Player.Move.ReadValue<Vector2>();
    }

    #endregion

    #region Call Input
    static event Action<InputAction.CallbackContext> AShowInfo;
    public static void CallShowInfo() => AShowInfo?.Invoke(new InputAction.CallbackContext());

    static event Action<InputAction.CallbackContext> CallDrag;
    public static void CallDragPerformed() => CallDrag?.Invoke(new InputAction.CallbackContext());


    #endregion


}

