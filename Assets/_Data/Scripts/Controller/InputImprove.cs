using System;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
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
            _input.UI.Click.started += value;
        }
        remove
        {
            _input.UI.Click.started -= value;
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

    public float MouseAxisX()
    {
        return _input.UI.MouseX.ReadValue<float>();
    }

    public float MouseScroll()
    {
        return _input.UI.MouseScroll.ReadValue<float>();
    }

    public bool RightPress()
    {
        return _input.UI.RightClick.IsPressed();
    }

    public bool TwoPress()
    {
        // Kiểm tra nếu có ít nhất hai điểm chạm
        if (Touchscreen.current != null && Touchscreen.current.touches.Count >= 2)
        {
            var touch1 = Touchscreen.current.touches[0];
            var touch2 = Touchscreen.current.touches[1];

            // Kiểm tra nếu cả hai điểm chạm đều đang trong quá trình chạm
            if (touch1.isInProgress && touch2.isInProgress)
            {
                return true;
            }
        }
        return false;
    }

    #endregion

}

