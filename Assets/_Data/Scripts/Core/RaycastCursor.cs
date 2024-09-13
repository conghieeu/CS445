using System;
using CuaHang.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CuaHang
{
    /// <summary> Dùng raycast và drag các item </summary>
    public class RaycastCursor : Singleton<RaycastCursor>
    {
        [Header("RaycastCursor")]
        [SerializeField] bool _enableRaycast = true;
        [SerializeField] bool _isEditItem;
        [SerializeField] LayerMask _layerMask;
        [SerializeField] UIRaycastChecker _uIRaycastChecker;
        [SerializeField] Transform _dragPoint; // là tâm của đối tượng giao diện
        [SerializeField] Item _itemSelect; // đối tượng target
        [SerializeField] ItemDrag _itemDrag;

        InputImprove _input => InputImprove.Instance;
        Camera _cam => Camera.main;

        public ItemDrag _ItemDrag
        {
            get => _itemDrag;
            private set
            {
                _itemDrag = value;
                OnDragItem?.Invoke(value._itemDragging);
            }
        }

        public Item ItemSelect
        {
            get => _itemSelect;
            private set
            {
                _itemSelect = value;
                OnSelectItem?.Invoke(_itemSelect);
            }
        }

        public static event Action<Item> OnSelectItem;
        public static event Action<Item> OnDragItem;
        public static event Action<Item> OnEditItem;
        public static event Action<Item> OnFollowItem;

        private void OnEnable()
        { 
            _input.EditItem += SetItemEdit;
            _input.DragItem += SetItemDrag;
            _input.Click += SetItemSelect;
            _input.FollowItem += SetFollowItem;
            _input.Cancel += CancelFocus;
        }

        private void OnDisable()
        { 
            _input.EditItem -= SetItemEdit;
            _input.DragItem -= SetItemDrag;
            _input.Click -= SetItemSelect;
            _input.FollowItem -= SetFollowItem;
            _input.Cancel -= CancelFocus;
        }

        /// <summary> Chiếu tia raycast lấy dữ liệu cho _Hit </summary>
        public RaycastHit GetRayMouseHit()
        {
            RaycastHit _hit = new();
            if (_enableRaycast == false) return _hit;

            Ray ray = _cam.ScreenPointToRay(_input.MousePosition());
            Physics.Raycast(ray, out _hit, 100, _layerMask);
            return _hit;
        }

        public RaycastHit GetRayDragPointHit()
        {
            RaycastHit _hit = new();
            if (_enableRaycast == false) return _hit;

            Ray ray = _cam.ScreenPointToRay(_dragPoint.position);
            Physics.Raycast(ray, out _hit, 100, _layerMask);
            return _hit;
        }

        /// <summary> Thoát không muốn cam tập trung nhìn tối tượng item này nữa </summary>
        void CancelFocus(InputAction.CallbackContext ctx)
        {
            if (ItemSelect)
            {
                ItemSelect = null;
            }
        }

        /// <summary> Tìm outline trong đối tượng và bật tắt viền của nó </summary>
        void SetOutlines(Transform model, bool isOn)
        {
            foreach (Outline outline in model.GetComponentsInChildren<Outline>())
            {
                if (isOn) outline.enabled = true;
                else outline.enabled = false;
            }

        }

        /// <summary> Bật item drag với item được _Hit chiếu</summary>
        void SetItemDrag(InputAction.CallbackContext ctx)
        {
            if (!_itemDrag || _itemDrag._isDragging || !ItemSelect) return;

            Item item = ItemSelect.transform.GetComponent<Item>();

            if (item && item._isCanDrag)
            {
                item.SetDragState(true);
                _itemDrag.PickUpItem(item);
            }
        }

        /// <summary> Tạo viền khi click vào item de select </summary>
        void SetItemSelect(InputAction.CallbackContext ctx)
        { 
            if (!_uIRaycastChecker.IsPointerOverUI() && !_itemDrag._itemDragging)
            {
                Transform hit = GetRayMouseHit().transform;
                Item itemHit = null;
                if (hit) itemHit = hit.GetComponent<Item>();

                if (_itemSelect) SetOutlines(_itemSelect.transform, false);
                if (itemHit) SetOutlines(itemHit.transform, true);
                ItemSelect = itemHit;
            }
        }

        private void SetFollowItem(InputAction.CallbackContext ctx)
        {
            if (_itemSelect != null)
            {
                OnFollowItem?.Invoke(_itemSelect);
            }
        }

        void SetItemEdit(InputAction.CallbackContext ctx)
        {
            if (_itemSelect && _itemSelect._camHere && !_isEditItem)
            {
                OnEditItem?.Invoke(_itemSelect);
            }
            else
            {
                OnEditItem?.Invoke(null);
            }
        }
    }
}
