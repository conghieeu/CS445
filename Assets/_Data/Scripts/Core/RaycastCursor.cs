using System;
using CuaHang.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace CuaHang
{
    /// <summary> Dùng raycast và drag các item </summary>
    public class RaycastCursor : Singleton<RaycastCursor>
    {
        [Header("RaycastCursor")]
        [SerializeField] Transform _itemSelect; // đối tượng target
        [SerializeField] bool _enableOutline;
        [SerializeField] LayerMask _layerMask;
        [SerializeField] UIRaycastChecker _uIRaycastChecker;

        InputImprove _input;
        ItemDrag _itemDrag;
        Camera _cam;

        public Transform _ItemSelect { get => _itemSelect; private set => _itemSelect = value; }

        protected override void Awake()
        {
            base.Awake(); 
            _input = new();
            _cam = Camera.main;
            _enableOutline = true;
        }

        private void Start()
        {
            _itemDrag = SingleModuleManager.Instance._itemDrag;
        }

        private void OnEnable()
        {
            _input.DragPerformed += SetItemDrag;
            _input.Click += SetItemSelect;
            _input.Cancel += CancelFocus;
        }

        private void OnDisable()
        {
            _input.DragPerformed -= SetItemDrag;
            _input.Click -= SetItemSelect;
            _input.Cancel -= CancelFocus;
        }

        /// <summary> Chiếu tia raycast lấy dữ liệu cho _Hit </summary>
        public RaycastHit GetRayHit()
        {
            RaycastHit _hit;

            Ray ray = _cam.ScreenPointToRay(_input.MousePosition());
            Physics.Raycast(ray, out _hit, 100, _layerMask);

            return _hit;
        }

        public RaycastHit[] GetRayHits()
        {
            RaycastHit[] _hits;

            Ray ray = _cam.ScreenPointToRay(_input.MousePosition());
            _hits = Physics.RaycastAll(ray, 100f, _layerMask);

            return _hits;
        }

        /// <summary> Tạo viền khi click vào item de select </summary>
        void SetItemSelect(InputAction.CallbackContext context)
        {
            if (!_itemDrag._itemDragging && !_uIRaycastChecker.IsPointerOverUI())
            { 
                Transform hit = GetRayHit().transform;

                // hit new 
                if (_ItemSelect != hit)
                {
                    CancelFocus(context);

                    _ItemSelect = hit;
                    SetOutlines(hit, true);
                }
            }
        }

        /// <summary> Thoát không muốn cam tập trung nhìn tối tượng item này nữa </summary>
        void CancelFocus(InputAction.CallbackContext context)
        {
            if (_ItemSelect)
            {
                SetOutlines(_ItemSelect, false);
                _ItemSelect = null;
            }
        }

        /// <summary> Tìm outline trong đối tượng và bật tắt viền của nó </summary>
        void SetOutlines(Transform model, bool isOn)
        {
            if (model)
            {
                foreach (Outline outline in model.GetComponentsInChildren<Outline>())
                {
                    if (_enableOutline && isOn) outline.enabled = true;
                    else outline.enabled = false;
                }
            }
        }

        /// <summary> Bật item drag với item được _Hit chiếu</summary>
        void SetItemDrag(InputAction.CallbackContext context)
        {
            if (!_ItemSelect || _itemDrag._isDragging) return;

            Item item = _ItemSelect.transform.GetComponent<Item>();

            if (item && item._isCanDrag)
            {
                item.DragItem();
                _itemDrag.PickUpItem(item);
            }
        }


    }
}
