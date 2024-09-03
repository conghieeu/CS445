using System;
using TMPro;
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
            _input.UI.Snap.performed += value;
        }
        remove
        {
            _input.UI.Snap.performed -= value;
        }
    }

    public event Action<InputAction.CallbackContext> MenuSettings
    {
        add
        {
            _input.UI.MenuSettings.performed += value;
        }
        remove
        {
            _input.UI.MenuSettings.performed -= value;
        }
    }

    public event Action<InputAction.CallbackContext> DragPerformed
    {
        add
        {
            _input.UI.Drag.performed += value;
        }
        remove
        {
            _input.UI.Drag.performed -= value;
        }
    }

    public event Action<InputAction.CallbackContext> ShowInfo
    {
        add
        {
            _input.UI.ShowInfo.performed += value;
        }
        remove
        {
            _input.UI.ShowInfo.performed -= value;
        }
    }

    public event Action<InputAction.CallbackContext> Cancel
    {
        add
        {
            _input.UI.Cancel.performed += value;
        }
        remove
        {
            _input.UI.Cancel.performed -= value;
        }
    }

    public event Action<InputAction.CallbackContext> Click
    {
        add
        {
            _input.UI.Click.performed += value;
        }
        remove
        {
            _input.UI.Click.performed -= value;
        }
    }


    public event Action<InputAction.CallbackContext> Sender
    {
        add
        {
            _input.UI.SenderItem.performed += value;
        }
        remove
        {
            _input.UI.SenderItem.performed -= value;
        }
    }

    public event Action<InputAction.CallbackContext> EditItem
    {
        add
        {
            _input.UI.EditItem.performed += value;
        }
        remove
        {
            _input.UI.EditItem.performed -= value;
        }
    }

    public event Action<InputAction.CallbackContext> FollowItem
    {
        add
        {
            _input.UI.FollowItem.performed += value;
        }
        remove
        {
            _input.UI.FollowItem.performed -= value;
        }
    }


    public Vector2 MovementInput()
    {
        return _input.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 MousePosition()
    {
        return _input.UI.Point.ReadValue<Vector2>();
    }

    #endregion

}

