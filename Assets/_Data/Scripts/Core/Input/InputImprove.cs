using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputImprove : GameBehavior
{
    [Header("Input Action")]
    public InputActionReference MouseScrollX;
    public InputActionReference MouseMoveX;
    public InputActionReference LeftClick;
    public InputActionReference RightClick;
    public InputActionReference SecondTouchContact;
    public InputActionReference PrimaryFingerPosition;
    public InputActionReference SecondaryFingerPosition;
    public InputActionReference SendItem;
    public InputActionReference MousePosition;
    public InputActionReference SnapItem;
    public InputActionReference EditItem;
    public InputActionReference DragItem;
    public InputActionReference FollowItem;
    public InputActionReference Cancel;

    public GameActionInput GameActionInput;

    private void Start()
    {
        GameActionInput = new GameActionInput();
        Debug.Log($"{GameActionInput}");
    }

    public Vector2 GetInputMove()
    {
        GameActionInput.Enable();
        return GameActionInput.Player.Move.ReadValue<Vector2>();
    }

    public void EnableGameActionInput(bool active)
    {
        if (active)
        {
            GameActionInput.Enable();
        }
        else
        {
            GameActionInput.Disable();
        }
    }


}
