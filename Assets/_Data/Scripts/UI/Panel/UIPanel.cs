using UnityEngine;

namespace CuaHang.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanel : GameBehavior
    {
        [Header("UI PANEL")]
        [SerializeField] protected RectTransform _panelContent;

        protected CanvasGroup _canvasGroup;

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void SetActiveContents(bool value)
        {
            if (_panelContent)
            {
                _panelContent.gameObject.SetActive(value);
            }
        }

        public virtual bool IsEnableCanvasGroup()
        {
            return _canvasGroup.interactable;
        }

        public virtual void EnableCanvasGroup(bool isOn)
        {

            if (!_canvasGroup) return;

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