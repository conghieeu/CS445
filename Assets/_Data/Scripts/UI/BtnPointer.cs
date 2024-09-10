using UnityEngine;
using UnityEngine.EventSystems;

public class BtnPointer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float _pointerScale = 1.5f;
    [SerializeField] AudioSource _mousePointer;

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * _pointerScale;
        if (_mousePointer) _mousePointer.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }

}

