using CuaHang;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIRotationCam : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] CameraControl cameraControl;

    private void Start()
    {
        cameraControl = ObjectsManager.Instance.CameraControl;
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

