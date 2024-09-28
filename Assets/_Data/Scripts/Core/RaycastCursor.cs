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

        ModuleDragItem _moduleDragItem;
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

        private void Start()
        {
            _moduleDragItem = ObjectsManager.Instance.ModuleDragItem;
            _cam = Camera.main;
            _moduleDragItem.gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            _inputEditItem.action.performed += SetItemEdit;
            _inputDragItem.action.performed += SetItemDrag;
            _inputClick.action.performed += SetItemSelect;
            _inputFollowItem.action.performed += SetFollowItem;
            _inputCancel.action.performed += ExitFollowItem;
        }

        private void OnDisable()
        {
            _inputEditItem.action.performed -= SetItemEdit;
            _inputDragItem.action.performed -= SetItemDrag;
            _inputClick.action.performed -= SetItemSelect;
            _inputFollowItem.action.performed -= SetFollowItem;
            _inputCancel.action.performed -= ExitFollowItem;
        }

        /// <summary> Lấy thông tin va chạm của tia ray từ vị trí chuột trên màn hình </summary>
        public RaycastHit GetMouseRaycastHit(Vector2 screenPoint)
        {
            RaycastHit hit = new();
            if (_enableRaycast)
            {
                Ray ray = _cam.ScreenPointToRay(screenPoint);
                Physics.Raycast(ray, out hit, 100, _layerMask);
            }
            return hit;
        }

        /// <summary> Thoát không muốn cam tập trung nhìn tối tượng item này nữa </summary>
        private void ExitFollowItem(InputAction.CallbackContext ctx)
        {
            if (ItemEdit)  // thoát item dang edit
            {
                ItemSelect = ItemEdit;
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
            if (ItemSelect && ItemSelect.IsCanDrag && _moduleDragItem && !_moduleDragItem.IsDragging)
            {
                ItemEdit = null;
                ItemSelect.SetDragState(true);
                _moduleDragItem.PickUpItem(ItemSelect);
                ActionDragItem?.Invoke(ItemSelect);
            }
        }

        /// <summary> Tạo viền khi click vào item de select </summary>
        private void SetItemSelect(InputAction.CallbackContext ctx)
        {
            if (!_uIRaycastChecker.IsPointerOverUI() && !_moduleDragItem.ItemDragging)
            {
                Transform hit = GetMouseRaycastHit(_inputMousePos.action.ReadValue<Vector2>()).transform;
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
                ItemSelect = null;
            }
            else
            {
                ItemEdit = null;
            }
        }



    }
}
