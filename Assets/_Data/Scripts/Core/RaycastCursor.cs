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
        [SerializeField] LayerMask _layerMask;
        [SerializeField] UIRaycastChecker _uIRaycastChecker;
        [SerializeField] Transform _dragPoint; // là tâm của đối tượng giao diện
        [SerializeField] Item _itemEdit; // item dang editing
        [SerializeField] Item _itemFollow; // item dang follow
        [SerializeField] Item _itemSelect; // item dang target
        [SerializeField] ModuleDragItem _itemDrag;

        InputImprove _input => InputImprove.Instance;
        Camera _cam => Camera.main;

        public ModuleDragItem ItemDrag { get => _itemDrag;}
        public Item ItemSelect
        {
            get => _itemSelect;
            private set
            {
                if (_itemSelect && _itemSelect != value) SetOutlines(_itemSelect.transform, false);

                _itemSelect = value;

                if (value) SetOutlines(value.transform, value);

                ActionSelectItem?.Invoke(value);
            }
        }
        public Item ItemEdit
        {
            get => _itemEdit; set
            {
                _itemEdit = value;
                ActionEditItem?.Invoke(value);
            }
        }
        public Item ItemFollow
        {
            get => _itemFollow; set
            {
                _itemFollow = value;
                ActionFollowItem?.Invoke(value);
            }
        }

        public static event Action<Item> ActionSelectItem;
        public static event Action<Item> ActionEditItem;
        public static event Action<Item> ActionFollowItem;

        private void OnEnable()
        {
            _input.EditItem += SetItemEdit;
            _input.DragItem += SetItemDrag;
            _input.Click += SetItemSelect;
            _input.FollowItem += SetFollowItem;
            _input.Cancel += ItemCancel;
        }

        private void OnDisable()
        {
            _input.EditItem -= SetItemEdit;
            _input.DragItem -= SetItemDrag;
            _input.Click -= SetItemSelect;
            _input.FollowItem -= SetFollowItem;
            _input.Cancel -= ItemCancel;
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
        private void ItemCancel(InputAction.CallbackContext ctx)
        {
            if (ItemEdit)  // thoát item dang edit
            {
                ItemEdit = null;
            }
            else  // thoát item follow
            {
                ItemSelect = null;
            }
        }

        /// <summary> Tìm outline trong đối tượng và bật tắt viền của nó </summary>
        private void SetOutlines(Transform model, bool isOn)
        {
            foreach (Outline outline in model.GetComponentsInChildren<Outline>())
            {
                if (isOn) outline.enabled = true;
                else outline.enabled = false;
            }
        }

        /// <summary> Bật item drag với item được _Hit chiếu</summary>
        private void SetItemDrag(InputAction.CallbackContext ctx)
        {
            if (!ItemDrag || ItemDrag._isDragging || !ItemSelect) return;

            Item item = ItemSelect.transform.GetComponent<Item>();

            if (item && item.IsCanDrag)
            {
                ItemEdit = null;
                item.SetDragState(true);
                ItemDrag.PickUpItem(item);
            }
        }

        /// <summary> Tạo viền khi click vào item de select </summary>
        private void SetItemSelect(InputAction.CallbackContext ctx)
        {
            if (!_uIRaycastChecker.IsPointerOverUI() && !ItemDrag._itemDragging)
            {
                Transform hit = GetRayMouseHit().transform;
                if (hit)
                {
                    ItemSelect = hit.GetComponent<Item>();
                }
            }
        }

        private void SetFollowItem(InputAction.CallbackContext ctx)
        {
            if (ItemSelect != null) 
            {
                ItemFollow = ItemSelect;
            }
        }

        private void SetItemEdit(InputAction.CallbackContext ctx)
        {
            if (ItemSelect && ItemSelect.CamHere)
            {
                ItemEdit = ItemSelect;
            }
            else
            {
                ItemEdit = null;
            }
        }


        
    }
}
