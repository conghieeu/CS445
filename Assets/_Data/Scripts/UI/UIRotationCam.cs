using CuaHang;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIRotationCam : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        CameraControl.Instance.IsTouchRotationArea = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CameraControl.Instance.IsTouchRotationArea = false;
    }
}

