
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
        [SerializeField] Button _btnEdit;

        [Header("Object Info Panel")]
        [SerializeField] bool _isShowInfo;
        [SerializeField] UIPanel _infoPanel;
        [SerializeField] TextMeshProUGUI _txtContentItem; // hiện nội dung item
        [SerializeField] string _defaultTmp;
        [SerializeField] BtnPressHandler _btnIncreasePrice;
        [SerializeField] BtnPressHandler _btnDiscountPrice;

        InputImprove _inputImprove => InputImprove.Instance;
        ModuleDragItem _itemDrag => RaycastCursor.Instance.ItemDrag;

        private void Start()
        {
            _defaultTmp = _txtContentItem.text;
            _btnOnDrag.EnableCanvasGroup(false);

            OnEditItem(null);
            OnItemSelect(null);
        }

        private void OnEnable()
        {
            _btnDropItem.onClick.AddListener(OnBtnDropItem);
            _btnShowInfo.onClick.AddListener(OnBtnShowInfo);

            RaycastCursor.ActionSelectItem += OnItemSelect;
            RaycastCursor.ActionEditItem += OnEditItem;
            PlayerPlanting.ActionSenderItem += OnPlayerSenderItem;

            _inputImprove.DragItem += OnBtnDragItem;

            _btnIncreasePrice.OnButtonDown += IncreasePrice;
            _btnDiscountPrice.OnButtonDown += DiscountPrice;
            _btnIncreasePrice.OnButtonHolding += IncreasePrice;
            _btnDiscountPrice.OnButtonHolding += DiscountPrice;
        }

        private void OnDisable()
        {
            RaycastCursor.ActionSelectItem -= OnItemSelect;
            RaycastCursor.ActionEditItem -= OnEditItem;
            _inputImprove.DragItem -= OnBtnDragItem;

            _btnIncreasePrice.OnButtonDown -= IncreasePrice;
            _btnDiscountPrice.OnButtonDown -= DiscountPrice;
            _btnIncreasePrice.OnButtonHolding -= IncreasePrice;
            _btnDiscountPrice.OnButtonHolding -= DiscountPrice;
        }

        private void FixedUpdate()
        {

            // panel context follow item select
            if (_itemSelect)
            {
                Vector3 worldPosition = _itemSelect.transform.position;
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
                _panelMenuContext.transform.position = screenPosition;
            }
        }

        private void OnPlayerSenderItem()
        {
            _btnOnDrag.EnableCanvasGroup(false);
            OnItemSelect(_itemSelect);
        }

        private void OnBtnDropItem()
        {
            if (_itemDrag.OnBtnDropItem())
            {
                _btnOnDrag.EnableCanvasGroup(false);
                OnItemSelect(_itemSelect);
            }
            else
            {
                _btnOnDrag.EnableCanvasGroup(true);
            }
        }

        private void OnBtnDragItem(InputAction.CallbackContext ctx)
        {
            if (_itemSelect == null) return;

            _panelMenuContext.EnableCanvasGroup(false);

            if (GameSystem.CurrentPlatform == Platform.Android)
            {
                _btnOnDrag.EnableCanvasGroup(true);
                _btnOnDrag.transform.position = _inputImprove.MousePosition();
                _btnOnDrag.transform.position = _btnSetDrag.transform.position;
            }

        }

        private void OnBtnShowInfo()
        {
            _infoPanel.EnableCanvasGroup(true);
            SetTxtContentItem();
        }

        /// <summary> bật button cancel edit </summary>
        private void OnEditItem(Item item)
        {
            if (item)
            {
                _btnCancelEdit.gameObject.SetActive(true);
            }
            else
            {
                _btnCancelEdit.gameObject.SetActive(false);
            }
        }

        /// <summary> Hiện option có thể chọn khi click đối tượng item </summary>
        private void OnItemSelect(Item item)
        {
            if (item)
            {
                _panelMenuContext.EnableCanvasGroup(true);
                _infoPanel.EnableCanvasGroup(false);

                if (item.CamHere)
                {
                    _btnEdit.GetComponent<Image>().enabled = true;
                }
                else
                {
                    _btnEdit.GetComponent<Image>().enabled = false;
                }
            }
            else
            {
                _panelMenuContext.EnableCanvasGroup(false);
                _infoPanel.EnableCanvasGroup(false);
            }

            _itemSelect = item;
        }

        private void SetTxtContentItem()
        {
            // hiện thị stats
            if (_itemSelect && _itemSelect.SO)
            {
                string x = $"Name: {_itemSelect.Name} \nPrice: {_itemSelect.Price.ToString("F1")} \n";
                _txtContentItem.text = _itemSelect.SO._isCanSell ? x + "Item có thể bán" : x + "Item không thể bán";
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
