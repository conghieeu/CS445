using UnityEngine;
using UnityEngine.EventSystems;
using System;

/// <summary> Delay khi an </summary>
public class BtnPressHandler : GameBehavior, IPointerDownHandler, IPointerUpHandler
{
    public event Action ActionButtonDown;

    [SerializeField] bool _isHolding = false;
    [SerializeField] float _timeDelayDefault = 0.7f;
    [SerializeField] float _timeDelay;

    private void OnEnable()
    {
        _isHolding = false;
    }
 
    private void FixedUpdate()
    {
        // countDown 
        if (_isHolding) _timeDelay -= Time.fixedDeltaTime;
        else _timeDelay = _timeDelayDefault;

        if (_timeDelay <= 0)
        {
            ActionButtonDown?.Invoke();
        }
    } 

    public void OnPointerDown(PointerEventData eventData)
    {
        ActionButtonDown?.Invoke();
        _isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isHolding = false;
        _timeDelay = _timeDelayDefault;
    }
}
