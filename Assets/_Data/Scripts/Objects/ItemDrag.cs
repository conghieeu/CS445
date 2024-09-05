using System;
using System.Collections;
using System.Collections.Generic;
using CuaHang.UI;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CuaHang
{
    /// <summary> ObjectTemp là đối tượng đại diện cho object Plant khi di dời đối tượng </summary>
    public class ItemDrag : MonoBehaviour
    {
        [Header("Drag")]
        public bool _isDragging;
        [SerializeField] bool _enableSnapping; // bật chế độ snapping
        [SerializeField] float _rotationSpeed = 0.1f;// Tốc độ xoay
        [SerializeField] float _snapDistance = 6f; // Khoảng cách cho phép đặt 
        [SerializeField] float _tileSize = 1; // ô snap tỷ lệ snap
        [SerializeField] Vector3 _tileOffset = Vector3.zero; // tỷ lệ snap + sai số này

        [Space]
        public Item _itemDragging;
        public Transform _modelsHolding; // là model object temp có màu xanh đang kéo thả
        [SerializeField] string _groundTag = "Ground";
        [SerializeField] Material _green, _red;
        [SerializeField] Transform _modelsHolder;
        [SerializeField] NavMeshSurface _navMeshSurface;
        [SerializeField] UIRaycastChecker _uIRaycastChecker;
        [SerializeField] SensorCast _sensorAround;
        [SerializeField] SensorCast _sensorGround;

        InputImprove _input;
        RaycastCursor raycastCursor;

        private void Awake()
        {
            _input = new();
            raycastCursor = RaycastCursor.Instance;
        }

        private void OnEnable()
        {
            _input.Click += ClickToDropItem;
            _input.SnapPerformed += SetSnap;
        }

        private void OnDisable()
        {
            _input.Click -= ClickToDropItem;
            _input.SnapPerformed -= SetSnap;
        }

        private void FixedUpdate()
        {
            SetMaterial();
            MoveItemDrag();
        }

        private void Update()
        {
            RotationItemDrag();
        }

        public void RotationItemDrag(float angle)
        {
            if (_modelsHolding != null)
            {
                _modelsHolding.transform.Rotate(Vector3.up, angle);
            }
        }

        /// <summary> để model temp đang dragging nó hiện giống model đang di chuyển ở thằng Player </summary>
        public void PickUpItem(Item item)
        {
            // Bật object drag
            gameObject.SetActive(true);

            // Tạo model giống otherModel ở vị trí 
            _modelsHolding = Instantiate(item._models, _modelsHolder);
            _modelsHolding.transform.localRotation = item.transform.rotation;

            // Cho item này lênh tay player
            item.SetParent(PlayerCtrl.Instance._posHoldParcel, null, true);

            _isDragging = true;
            _itemDragging = item;
        }

        /// <summary> set up lại value khi đặt item </summary>
        public void OnDropItem()
        {
            Destroy(_modelsHolding.gameObject); // Delete model item
            _itemDragging.DropItem(_modelsHolding);
            _itemDragging = null;
            _isDragging = false;
            gameObject.SetActive(false);
        }

        void ClickToDropItem(InputAction.CallbackContext context)
        {
            if (IsCanPlant() && _itemDragging && !_uIRaycastChecker.IsPointerOverUI())
            {
                OnDropItem();
                _navMeshSurface.BuildNavMesh();
            }
        }

        void SetSnap(InputAction.CallbackContext context)
        {
            _enableSnapping = !_enableSnapping;
        }

        void SetMaterial()
        {
            if (IsCanPlant())
            {
                SetMaterialModel(_green);
            }
            else
            {
                SetMaterialModel(_red);
            }
        }

        void SetMaterialModel(Material color)
        {
            foreach (Renderer model in _modelsHolder.GetComponentsInChildren<Renderer>())
            {
                model.material = color;
            }
        }

        bool IsCanPlant()
        {
            return _sensorAround._hits.Count == 0 && IsTouchGround();
        }

        bool IsTouchGround()
        {
            // Làm thế nào để cái sensor check ở dưới
            foreach (var obj in _sensorGround._hits)
            {
                if (obj.CompareTag(_groundTag))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary> Di chuyen item </summary>
        void MoveItemDrag()
        {
            Debug.Log(_uIRaycastChecker.IsPointerOverUI());
            if(_uIRaycastChecker.IsPointerOverUI()) return;

            //  Làm tròn vị trí temp để nó giống snap
            if (_enableSnapping)
            {
                Vector3 hitPoint = GetRayHit().point;

                float sX = Mathf.Round(hitPoint.x / _tileSize) * _tileSize + _tileOffset.x;
                float sZ = Mathf.Round(hitPoint.z / _tileSize) * _tileSize + _tileOffset.z;
                float sY = Mathf.Round(hitPoint.y / _tileSize) * _tileSize + _tileOffset.y;

                Vector3 snappedPosition = new Vector3(sX, sY, sZ);
                transform.position = snappedPosition;
            }
            else
            {
                transform.position = GetRayHit().point;
            }
        }

        /// <summary> Xoay item </summary>
        void RotationItemDrag()
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.up, GetRayHit().normal); // để đối tượng vuông góc với bề mặt va chạm
            float currentAngle = _modelsHolding.eulerAngles.y; // Lấy góc xoay hiện tại của vật thể
            float roundedAngle = Mathf.Round(currentAngle / 10.0f) * 10.0f; // Làm tròn góc xoay hiện tại về hàng chục gần nhất
            float rotationAngle = Mathf.Round(_input.MouseScroll() * _rotationSpeed / 10.0f) * 10.0f;  // Tính toán góc xoay mới dựa trên giá trị cuộn chuột
            float newAngle = roundedAngle + rotationAngle; // Cộng góc xoay mới vào góc xoay đã làm tròn
            _modelsHolding.rotation = Quaternion.Euler(0, newAngle, 0);  // Áp dụng góc xoay mới cho vật thể 
        }

        RaycastHit GetRayHit() => raycastCursor.GetRayHit();
    }
}