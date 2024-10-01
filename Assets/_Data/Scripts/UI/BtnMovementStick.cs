
using CuaHang;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnMovementStick : GameBehavior, IPointerDownHandler, IPointerUpHandler
{
    CameraControl m_CameraControl;

    private void Start()
    {
        m_CameraControl = FindFirstObjectByType<CameraControl>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_CameraControl.IsMoveStick = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_CameraControl.IsMoveStick = false;
    }
}