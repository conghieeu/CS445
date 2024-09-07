using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputImprove
{
    private GameActionInput _input;
 
    private InputImprove()
    {
        _input = new GameActionInput();
        _input.Enable();
    }

    public static InputImprove Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new InputImprove();
            }
            return _instance;
        }
    }

    public void EnableActionInput() => _input.Enable();
    public void DisableActionInput() => _input.Disable();

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

    public event Action<InputAction.CallbackContext> DragItem
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

    public event Action<InputAction.CallbackContext> SecondTouchContactStart
    {
        add
        {
            _input.UI.SecondTouchContact.started += value;
        }
        remove
        {
            _input.UI.SecondTouchContact.started -= value;
        }
    }

    public event Action<InputAction.CallbackContext> SecondTouchContactCancel
    {
        add
        {
            _input.UI.SecondTouchContact.canceled += value;
        }
        remove
        {
            _input.UI.SecondTouchContact.canceled -= value;
        }
    }

    public Vector2 PrimaryFingerPosition()
    {
        return _input.UI.PrimaryFingerPosition.ReadValue<Vector2>();
    }

    public Vector2 SecondFingerPosition()
    {
        return _input.UI.SecondFingerPosition.ReadValue<Vector2>();
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

    public bool MouseRightClick()
    {
        return _input.UI.RightClick.IsPressed();
    }

    public bool DoubleTouchScreen()
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

}

