using System.Collections;
using CuaHang;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnMovementStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{ 
    public void OnPointerDown(PointerEventData eventData)
    { 
        CameraControl.Instance.IsMoveStick = true;
    } 

    public void OnPointerUp(PointerEventData eventData)
    { 
        CameraControl.Instance.IsMoveStick = false;
    } 
}