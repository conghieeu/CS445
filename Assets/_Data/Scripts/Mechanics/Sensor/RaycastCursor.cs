using System;
using CuaHang.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CuaHang
{
    /// <summary> Dùng raycast và drag các item </summary>
    public class RaycastCursor : GameBehavior
    {
        [Header("RAYCAST CURSOR")]
        [SerializeField] LayerMask _layerMask;
        [SerializeField] UIRaycastChecker _uIRaycastChecker;
        [SerializeField] bool _enableRaycast = true;
        public Transform PointDrag;

        [Header("Input Action")]
        [SerializeField] InputActionReference _inputEditItem;
        [SerializeField] InputActionReference _inputDragItem;
        [SerializeField] InputActionReference _inputClick;
        [SerializeField] InputActionReference _inputFollowItem;
        [SerializeField] InputActionReference _inputCancel;
        [SerializeField] InputActionReference _inputMousePos;

        Item _itemEdit;
        Item _itemFollow;
        Item _itemSelect;

        ModuleDragItem m_ModuleDragItem;
        Camera _cam;

        public Item ItemSelect
        {
            get => _itemSelect;
            private set
            {
                if (_itemSelect && _itemSelect != value)
                {
                    SetOutlines(_itemSelect.transform, false);
                }
                if (value)
                {
                    SetOutlines(value.transform, value);
                }

                _itemSelect = value;

                ActionSelectItem?.Invoke(value);
            }
        }
        public Item ItemEdit
        {
            get => _itemEdit;
            set
            {
                if (value != _itemEdit && _itemEdit)
                {
                    _itemEdit.GetComponent<Collider>().enabled = true;
                }
                if (value)
                {
                    value.GetComponent<Collider>().enabled = false;
                }

                _itemEdit = value;
                ActionEditItem?.Invoke(value);
            }
        }
        public Item ItemFollow
        {
            get => _itemFollow;
            set
            {
                _itemFollow = value;
                ActionFollowItem?.Invoke(value);
            }
        }

        public static event Action<Item> ActionDragItem;
        public static event Action<Item> ActionSelectItem;
        public static event Action<Item> ActionEditItem;
        public static event Action<Item> ActionFollowItem;

        private void Awake()
        {
            m_ModuleDragItem = FindFirstObjectByType<ModuleDragItem>();
            _cam = Camera.main;

            _inputEditItem.action.performed += ctx => SetItemEdit();
            _inputDragItem.action.performed += ctx => SetItemDrag();
            _inputClick.action.performed += ctx => SetItemSelect();
            _inputFollowItem.action.performed += ctx => SetFollowItem();
            _inputCancel.action.performed += ctx => ExitFollowItem();
        }

        /// <summary> Lấy thông tin va chạm của tia ray từ vị trí chuột trên màn hình </summary>
        public RaycastHit GetRaycastHit()
        {
            RaycastHit hit = new();
            if (_enableRaycast == false) return hit;

            Vector2 screenPoint = _inputMousePos.action.ReadValue<Vector2>();

            if (GameSystem.CurrentPlatform == Platform.Android)
            {
                screenPoint = PointDrag.position;
            }

            if (_enableRaycast)
            {
                Ray ray = _cam.ScreenPointToRay(screenPoint);
                Physics.Raycast(ray, out hit, 100, _layerMask);
            }
            return hit;
        }

        /// <summary> Thoát không muốn cam tập trung nhìn tối tượng item này nữa </summary>
        private void ExitFollowItem()
        {
            if (!this) return;
            ItemEdit = null;
            ItemSelect = null;
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
        private void SetItemDrag()
        {
            if (!this) return;
            if (ItemSelect && ItemSelect.IsCanDrag && m_ModuleDragItem && !m_ModuleDragItem.IsDragging)
            {
                ItemEdit = null;
                ItemSelect.SetDragState(true);
                m_ModuleDragItem.PickUpItem(ItemSelect);
                ActionDragItem?.Invoke(ItemSelect);
            }
        }

        /// <summary> Tạo viền khi click vào item de select </summary>
        private void SetItemSelect()
        {
            if (!this) return;
            if (!_uIRaycastChecker.IsPointerOverUI() && !m_ModuleDragItem.ItemDragging)
            {
                Transform hit = GetRaycastHit().transform;
                if (hit)
                {
                    ItemSelect = hit.GetComponent<Item>();
                }
            }
        }

        private void SetFollowItem()
        {
            if (!this) return;
            if (ItemSelect != null)
            {
                ItemFollow = ItemSelect;
            }
        }

        private void SetItemEdit()
        {
            if (!this) return;
            if (ItemSelect && ItemSelect.CamHere)
            {
                ItemEdit = ItemSelect;
                ItemSelect = null;
            }
            else
            {
                ItemEdit = null;
            }
        }

    }
}
