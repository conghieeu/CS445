using CuaHang;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;

[RequireComponent(typeof(OnScreenStick))]
public class PlayerMovementStick : GameBehavior, IPointerDownHandler, IPointerUpHandler
{
    CameraControl m_CameraControl;
    GameSystem m_GameSystem;
    RaycastCursor m_RaycastCursor;

    private void Awake()
    {
        m_CameraControl = FindFirstObjectByType<CameraControl>();
        m_GameSystem = FindFirstObjectByType<GameSystem>();
        m_RaycastCursor = FindFirstObjectByType<RaycastCursor>();
    }

    private void Start()
    {
        if (m_GameSystem.CurrentPlatform != Platform.Android)
        {
            gameObject.SetActive(false);
        }

        m_RaycastCursor.ActionEditItem += OnZoomToItemTarget;
    }

    public void OnPointerUp(PointerEventData data)
    {

        m_CameraControl.IsMoveStick = false;
    }

    public void OnPointerDown(PointerEventData data)
    {

        m_CameraControl.IsMoveStick = true;
    }

    private void OnZoomToItemTarget(Item itemZoomIn)
    {
        gameObject.SetActive(!itemZoomIn);
    }
}