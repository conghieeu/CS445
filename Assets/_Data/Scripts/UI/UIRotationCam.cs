using CuaHang;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIRotationCam : GameBehavior, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] CameraControl cameraControl;

    private void Start()
    {
        cameraControl = FindFirstObjectByType<CameraControl>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        cameraControl.IsTouchRotationArea = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        cameraControl.IsTouchRotationArea = false;
    }
}

