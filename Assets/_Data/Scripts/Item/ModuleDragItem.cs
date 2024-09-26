using CuaHang.UI;
using System;
using System.Collections;
using System.Collections.Generic; 
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CuaHang
{
    /// <summary> ObjectTemp là đối tượng đại diện cho object Plant khi di dời đối tượng </summary>
    public class ModuleDragItem : MonoBehaviour
    {
        [Header("Drag")]
        public bool _isDragging;
        [SerializeField] bool _enableSnapping; // bật chế độ snapping
        [SerializeField] float _rotationSpeed = 0.1f;// Tốc độ xoay
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
            _input = InputImprove.Instance;
            raycastCursor = RaycastCursor.Instance;
        }

        private void OnEnable()
        {
            _input.Click += DropItem;
            _input.SnapPerformed += SetSnap;
        }

        private void OnDisable()
        {
            _input.Click -= DropItem;
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

        /// <summary> để model temp đang dragging nó hiện giống model đang di chuyển ở thằng Player </summary>
        public void PickUpItem(Item item)
        {
            // Bật object drag
            gameObject.SetActive(true);

            // Tạo model giống otherModel ở vị trí 
            _modelsHolding = Instantiate(item.Models, _modelsHolder);
            _modelsHolder.localRotation = item.transform.rotation;

            // Cho item này lênh tay player
            item.SetParent(PlayerCtrl.Instance.PosHoldParcel, null, false);

            _isDragging = true;
            _itemDragging = item;
        }

        /// <summary> Button quay item drag sẽ gọi </summary>
        public void _ClickRotation(float angle)
        {
            float currentAngle = _modelsHolder.localEulerAngles.y;
            float roundedAngle = Mathf.Round(currentAngle / 10.0f) * 10.0f;
            float rotationAngle = Mathf.Round(angle * _rotationSpeed / 10.0f) * 10.0f;
            float newAngle = roundedAngle + rotationAngle;

            _modelsHolder.localRotation = Quaternion.Euler(0, newAngle, 0);
        }

        public void SetSnap(InputAction.CallbackContext ctx)
        {
            _enableSnapping = !_enableSnapping;
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

        /// <summary> Dành cho nút nhấn drop item </summary>
        public bool OnBtnDropItem()
        {
            if (IsCanPlant() && GameSystem.CurrentPlatform == Platform.Android)
            {
                OnDropItem();
                _navMeshSurface.BuildNavMesh();
                return true;
            }
            return false;
        }

        private void SetMaterial()
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

        private void SetMaterialModel(Material color)
        {
            foreach (Renderer model in _modelsHolder.GetComponentsInChildren<Renderer>())
            {
                model.material = color;
            }
        }

        private bool IsCanPlant()
        {
            return _sensorAround._hits.Count == 0 && IsTouchGround() && _itemDragging;
        }
        
        private bool IsTouchGround()
        { 
            foreach (var obj in _sensorGround._hits)
            {
                if (obj.CompareTag(_groundTag))
                {
                    return true;
                }
            }
            return false;
        }

        private void DropItem(InputAction.CallbackContext context)
        {
            if (IsCanPlant() && GameSystem.CurrentPlatform == Platform.Standalone)
            {
                OnDropItem();
                _navMeshSurface.BuildNavMesh();
            }
        }

        /// <summary> Di chuyen item danh cho pc </summary>
        private void MoveItemDrag()
        {
            Vector3 hitPos = new();
            if (GameSystem.Instance._Platform == Platform.Standalone)
            {
                hitPos = GetRayHit().point;
                hitPos = RoundPos(hitPos, _enableSnapping);
            }
            else if (GameSystem.Instance._Platform == Platform.Android)
            {
                hitPos = GetDragPointHit().point;
                hitPos = RoundPos(hitPos, _enableSnapping);
            }

            transform.position = hitPos;
        }

        private Vector3 RoundPos(Vector3 pos, bool isOn)
        {
            //  Làm tròn vị trí temp để nó giống snap
            if (isOn)
            {
                float sX = Mathf.Round(pos.x / _tileSize) * _tileSize + _tileOffset.x;
                float sZ = Mathf.Round(pos.z / _tileSize) * _tileSize + _tileOffset.z;
                float sY = Mathf.Round(pos.y / _tileSize) * _tileSize + _tileOffset.y;
                pos = new Vector3(sX, sY, sZ);
            }

            return pos;
        }

        /// <summary> Xoay item </summary>
        private void RotationItemDrag()
        {
            RaycastHit hit = new RaycastHit();

            // Transform: lấy vuông góc với bề mặt va chạm
            if (GameSystem.CurrentPlatform == Platform.Android) hit = GetDragPointHit();
            else hit = GetRayHit();
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            // Model holder: lấy góc xoay mới
            float currentAngle = _modelsHolder.localEulerAngles.y;
            float roundedAngle = Mathf.Round(currentAngle / 10.0f) * 10.0f; // Làm tròn góc xoay hiện tại về hàng chục gần nhất
            float rotationAngle = Mathf.Round(_input.MouseScroll() * _rotationSpeed / 10.0f) * 10.0f;  // Tính toán góc xoay mới dựa trên giá trị cuộn chuột
            float newAngle = roundedAngle + rotationAngle; // Cộng góc xoay mới vào góc xoay đã làm tròn

            _modelsHolder.localRotation = Quaternion.Euler(0, newAngle, 0);
        }

        RaycastHit GetRayHit() => raycastCursor.GetRayMouseHit();
        RaycastHit GetDragPointHit() => raycastCursor.GetRayDragPointHit();

    }
}