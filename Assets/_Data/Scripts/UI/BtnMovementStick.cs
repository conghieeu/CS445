
using CuaHang;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnMovementStick : GameBehavior, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] CameraControl cameraControl => ObjectsManager.Instance.CameraControl; 

    public void OnPointerDown(PointerEventData eventData)
    {
        cameraControl.IsMoveStick = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        cameraControl.IsMoveStick = false;
    }
}