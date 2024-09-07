using CuaHang;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointDragItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] bool isPointerDown = false;
    ItemDrag _itemDrag;
    InputImprove _inputImprove;

    private void Start()
    {
        _itemDrag = SingleModuleManager.Instance._itemDrag;
        _inputImprove = new InputImprove();
    }

    private void Update()
    {
        if (isPointerDown)
        {
            transform.position = _inputImprove.MousePosition(); // chuot di chuyen voi chuot click 
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

