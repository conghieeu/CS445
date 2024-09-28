using System.Collections;
using CuaHang;
using CuaHang.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PointDragItem : UIPanel
{
    [Header("POINT DRAG ITEM")]
    [SerializeField] bool _isPointerDown = false;
    [SerializeField] InputActionReference _inputMousePos;
    ModuleDragItem _moduleDragItem;
    RaycastCursor _raycastCursor;

    private void Start()
    {
        _moduleDragItem = ObjectsManager.Instance.ModuleDragItem;
        _raycastCursor = ObjectsManager.Instance.RaycastCursor;
    }

    private void Update()
    {
        if (_isPointerDown)
        {
            Vector3 mousePos = _inputMousePos.action.ReadValue<Vector2>();
            RaycastHit raycastHit = _raycastCursor.GetMouseRaycastHit(mousePos);
            Vector3 hitPoint = raycastHit.transform.position;

            transform.position = mousePos;
            _moduleDragItem.DragItem(hitPoint);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPointerDown = false;
    }
}

