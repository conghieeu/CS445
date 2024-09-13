using System.Collections;
using CuaHang.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointDragItem : UIPanel, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] bool _isPointerDown = false;

    InputImprove _inputImprove;
    Coroutine _movementCoroutine;

    protected override void Start()
    {
        base.Start();

        EnableCanvasGroup(GameSystem.CurrentPlatform == Platform.Android);

        _inputImprove = InputImprove.Instance;
    }
 
    /// <summary> di chuyển theo con trỏ </summary>
    private IEnumerator FollowCursor()
    {
        while (_isPointerDown)
        {
            transform.position = _inputImprove.MousePosition();
            yield return null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
        if (_movementCoroutine == null)
        {
            _movementCoroutine = StartCoroutine(FollowCursor());
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

