
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

namespace CuaHang.UI
{
    public class UIObjectInteraction : MonoBehaviour
    {
        [Header("Item Selection")]
        [SerializeField] Item _itemSelect;
        [SerializeField] PointDragItem _btnOnDrag;
        [SerializeField] UIPanel _panelMenuContext;
        [SerializeField] Button _btnCancelEdit;
        [SerializeField] Button _btnSetDrag;
        [SerializeField] Button _btnShowInfo;
        [SerializeField] Button _btnDropItem;

        [Header("Object Info Panel")]
        [SerializeField] bool _isShowInfo;
        [SerializeField] UIPanel _infoPanel;
        [SerializeField] TextMeshProUGUI _txtContentItem; // hiện nội dung item
        [SerializeField] string _defaultTmp;
        [SerializeField] BtnPressHandler _btnIncreasePrice;
        [SerializeField] BtnPressHandler _btnDiscountPrice;

        InputImprove _inputImprove => InputImprove.Instance;
        ItemDrag _itemDrag => RaycastCursor.Instance._ItemDrag;

        private void Start()
        {
            _btnOnDrag.EnableCanvasGroup(false);
            _defaultTmp = _txtContentItem.text;
            OnSelectedItem(null);
        }

        private void OnEnable()
        {
            RaycastCursor.OnSelectItem += OnSelectedItem;
            CameraControl.OnEditItem += OnEditItem;

            _inputImprove.DragItem += OnBtnDragItem;
            _btnDropItem.onClick.AddListener(OnBtnDropItem);
            _btnShowInfo.onClick.AddListener(OnBtnShowInfo);

            _btnIncreasePrice.OnButtonDown += IncreasePrice;
            _btnDiscountPrice.OnButtonDown += DiscountPrice;
            _btnIncreasePrice.OnButtonHolding += IncreasePrice;
            _btnDiscountPrice.OnButtonHolding += DiscountPrice;
        }

        private void FixedUpdate()
        {
            OnDragItem();

            // panel context follow item select
            if (_itemSelect)
            {
                Vector3 worldPosition = _itemSelect.transform.position;
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
                _panelMenuContext.transform.position = screenPosition;
            }
        }

        private void OnBtnDropItem()
        {
            if (_itemDrag.DropItem())
            {
                _btnOnDrag.EnableCanvasGroup(false);
            }
            else 
            {
                _btnOnDrag.EnableCanvasGroup(true);
            }
        }

        private void OnBtnDragItem(InputAction.CallbackContext ctx)
        {
            if (_btnOnDrag == null) return;

            _btnOnDrag.EnableCanvasGroup(true);
            _panelMenuContext.EnableCanvasGroup(false);

            // _btnOnDrag position = _btnSetDrag position
            if (GameSystem.CurrentPlatform != Platform.Android) return;
            _btnOnDrag.EnableCanvasGroup(true);
            _btnOnDrag.transform.position = _inputImprove.MousePosition();
            _btnOnDrag.transform.position = _btnSetDrag.transform.position;
        }

        private void OnBtnShowInfo()
        {
            _infoPanel.EnableCanvasGroup(!_infoPanel.IsEnableCanvasGroup());
            SetTxtContentItem();
        }

        /// <summary> bật button cancel edit </summary>
        private void OnEditItem(Item item)
        {
            if (_btnCancelEdit) _btnCancelEdit.gameObject.SetActive(item != null);
        }

        /// <summary> bật button rotation item drag </summary>
        private void OnDragItem()
        {
            if (GameSystem.CurrentPlatform != Platform.Android) return;

            if (_itemDrag.gameObject.activeInHierarchy)
            {
                _itemDrag.MoveItemDragOnAndroid(); // di chuyen item theo point drag
            }
        }

        /// <summary> Hiện option có thể chọn khi click đối tượng item </summary>
        private void OnSelectedItem(Item item)
        {
            _itemSelect = item;

            if (item)
            {
                if (_panelMenuContext) _panelMenuContext.EnableCanvasGroup(true);
            }
            else
            {
                _panelMenuContext.EnableCanvasGroup(false);
                if (_infoPanel) _infoPanel.EnableCanvasGroup(false);
            }
        }

        private void SetTxtContentItem()
        {
            // hiện thị stats
            if (_itemSelect && _itemSelect._SO)
            {
                string x = $"Name: {_itemSelect._name} \nPrice: {_itemSelect._price.ToString("F1")} \n";
                _txtContentItem.text = _itemSelect._SO._isCanSell ? x + "Item có thể bán" : x + "Item không thể bán";
            }
            else
            {
                _txtContentItem.text = _defaultTmp;
            }
        }

        // --------------BUTTON--------------

        public void IncreasePrice()
        {
            if (_itemSelect) _itemSelect.SetPrice(0.1f);
            SetTxtContentItem();
        }

        public void DiscountPrice()
        {
            if (_itemSelect) _itemSelect.SetPrice(-0.1f);
            SetTxtContentItem();
        }

    }
}
