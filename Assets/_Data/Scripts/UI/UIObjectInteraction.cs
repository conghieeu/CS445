using System;
using QFSW.QC.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CuaHang.UI
{
    public class UIObjectInteraction : GameBehavior
    {
        [Header("UI OBJECT INTERACTION")]
        [Header("Item Selection")]
        [SerializeField] Item _itemSelect;
        [SerializeField] Button _btnCancelEdit;

        [Header("On Select Item")]
        [SerializeField] RectTransform pointDrag;
        [SerializeField] Button _btnDropItem;
        [SerializeField] Button btnRotationLeft;
        [SerializeField] Button btnRotationRight;
        [SerializeField] UIPanel _panelMenuContext;
        [SerializeField] PointDragItem pointDragItem;

        [Header("On Drag Item")]
        [SerializeField] UIPanel _infoPanel;
        [SerializeField] TextMeshProUGUI _txtContentItem; // hiện nội dung item
        [SerializeField] BtnPressHandler _btnIncreasePrice;
        [SerializeField] BtnPressHandler _btnDiscountPrice;
        [SerializeField] Button _btnSetDrag;
        [SerializeField] Button _btnShowInfo;
        [SerializeField] Button _btnEdit;

        ModuleDragItem _moduleDragItem;


        private void Start()
        {
            _moduleDragItem = ObjectsManager.Instance.ModuleDragItem;
            pointDragItem.EnableCanvasGroup(false);
            OnActionEditItem(null);
            OnActionSelectItem(null);

            _btnDropItem.onClick.AddListener(OnBtnDropItem);
            _btnShowInfo.onClick.AddListener(OnBtnShowInfo);
            btnRotationLeft.onClick.AddListener(OnRollLeft);
            btnRotationRight.onClick.AddListener(OnRollRight);

            RaycastCursor.ActionSelectItem += OnActionSelectItem;
            RaycastCursor.ActionEditItem += OnActionEditItem;
            RaycastCursor.ActionDragItem += OnActionBtnDragItem;

            PlayerPlanting.ActionSenderItem += OnActionPlayerSenderItem;

            _btnIncreasePrice.ActionButtonDown += IncreasePrice;
            _btnDiscountPrice.ActionButtonDown += DiscountPrice;
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

        private void OnRollLeft()
        {
            _moduleDragItem.OnClickRotation(-15);
        }

        private void OnRollRight()
        {
            _moduleDragItem.OnClickRotation(15);
        }

        private void OnActionPlayerSenderItem()
        {
            pointDragItem.EnableCanvasGroup(false);
            OnActionSelectItem(_itemSelect);
        }

        private void OnBtnDropItem()
        {
            if (_moduleDragItem.TryDropItem())
            {
                pointDragItem.EnableCanvasGroup(false);
                OnActionSelectItem(_itemSelect);
            }
            else
            {
                pointDragItem.EnableCanvasGroup(true);
            }
        }

        private void OnActionBtnDragItem(Item item)
        {
            if (GameSystem.CurrentPlatform == Platform.Android && item)
            {
                _panelMenuContext.EnableCanvasGroup(false);
                pointDragItem.EnableCanvasGroup(true);
                pointDragItem.transform.position = _btnSetDrag.transform.position;
            }
        }

        private void OnBtnShowInfo()
        {
            _infoPanel.EnableCanvasGroup(true);
            SetTxtContentItem();
        }

        /// <summary> bật button cancel edit </summary>
        private void OnActionEditItem(Item item)
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
        private void OnActionSelectItem(Item item)
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
            string tmp = "";

            // hiện thị stats
            if (_itemSelect && _itemSelect.SO)
            {
                tmp = $"Name: {_itemSelect.Name} \nPrice: {_itemSelect.Price.ToString("F1")} \n";
                _txtContentItem.text = _itemSelect.SO._isCanSell ? tmp + "Item có thể bán" : tmp + "Item không thể bán";
            }
            else
            {
                _txtContentItem.text = tmp;
            }
        }


    }
}
