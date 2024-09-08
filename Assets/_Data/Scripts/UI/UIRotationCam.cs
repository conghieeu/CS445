using CuaHang;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIRotationCam : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{ 
    public void OnPointerDown(PointerEventData eventData)
    {
        CameraControl.Instance.IsTouchRotationArea = true;
    } 

    public void OnPointerUp(PointerEventData eventData)
    {
        CameraControl.Instance.IsTouchRotationArea = false;
    }
}

