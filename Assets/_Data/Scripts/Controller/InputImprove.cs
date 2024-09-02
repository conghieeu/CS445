using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputImprove : MonoBehaviour
{
    private static GameActionInput _input;

    private void Awake()
    {
        _input = new GameActionInput();
        _input.Enable();
    }

    public static event Action<InputAction.CallbackContext> SnapPerformed
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

    public static event Action<InputAction.CallbackContext> MenuSettings
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

    public static event Action<InputAction.CallbackContext> DragPerformed
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

    public static event Action<InputAction.CallbackContext> ShowInfo
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

    static event Action<InputAction.CallbackContext> AShowInfo;
    public static void CallShowInfo() => AShowInfo?.Invoke(new InputAction.CallbackContext());

    static event Action<InputAction.CallbackContext> CallDrag;
    public static void CallDragPerformed()
    {
        CallDrag?.Invoke(new InputAction.CallbackContext());
    }

    public static Vector2 MovementInput()
    {
        return _input.Player.Move.ReadValue<Vector2>();
    }

}

