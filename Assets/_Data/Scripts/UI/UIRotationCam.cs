using CuaHang;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIRotationCam : GameBehavior, IPointerDownHandler, IPointerUpHandler
{ 
    public bool IsTouchArea; // có đang chạm vào image này không
    CameraControl cameraControl;

    private void Start()
    {
        cameraControl = FindFirstObjectByType<CameraControl>();
    }

    private void Update()
    {
        if (IsTouchArea)
        {
            cameraControl.CamRotation();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsTouchArea = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsTouchArea = false;
    }
}

