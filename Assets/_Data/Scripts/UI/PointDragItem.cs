using System.Collections;
using CuaHang;
using CuaHang.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PointDragItem : GameBehavior, IPointerDownHandler, IPointerUpHandler
{
    [Header("POINT DRAG ITEM")]
    [SerializeField] bool isPointerDown = false;
    
    InputImprove m_InputImprove;

    private void Awake()
    {
        m_InputImprove = FindAnyObjectByType<InputImprove>();

    }

    private void Update()
    {
        if (isPointerDown)
        {
            transform.position = m_InputImprove.MousePosition.action.ReadValue<Vector2>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
    }
}

