using UnityEngine;

namespace CuaHang.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanel : HieuBehavior
    {
        [Header("UI PANEL")]
        [SerializeField] protected CanvasGroup _canvasGroup;
        [SerializeField] protected RectTransform _panelContent;

        protected virtual void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void ShowContents(bool value)
        {
            if (_panelContent)
            {
                _panelContent.gameObject.SetActive(value);
            }
        }

        public virtual void SetActiveCanvasGroup(bool isOn)
        {
            if (isOn)
            {
                _canvasGroup.alpha = 1;
            }
            else
            {
                _canvasGroup.alpha = 0;
            }

            _canvasGroup.interactable = isOn;
            _canvasGroup.blocksRaycasts = isOn;
        }
    }
}