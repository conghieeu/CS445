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
    
    private void Update()
    {
        if (_isPointerDown)
        {
            transform.position = _inputMousePos.action.ReadValue<Vector2>();
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

