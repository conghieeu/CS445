using System.Collections;
using CuaHang;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointDragItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] bool _isPointerDown = false;
 
    InputImprove _inputImprove;
    Coroutine _movementCoroutine;

    private void Start()
    { 
        _inputImprove = InputImprove.Instance;
    }

    private IEnumerator MoveWithMouse()
    {
        while (_isPointerDown)
        {
            transform.position = _inputImprove.MousePosition(); // chuot di chuyen voi chuot click 
            yield return null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
        if (_movementCoroutine == null)
        {
            _movementCoroutine = StartCoroutine(MoveWithMouse());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPointerDown = false;
        if (_movementCoroutine != null)
        {
            StopCoroutine(_movementCoroutine);
            _movementCoroutine = null;
        }
    }
}

