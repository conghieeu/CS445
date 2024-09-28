using System;
using CuaHang.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CuaHang
{
    /// <summary> ObjectTemp là đối tượng đại diện cho object Plant khi di dời đối tượng </summary>
    public class ModuleDragItem : GameBehavior
    {
        [Header("MODULE DRAG ITEM")]
        [SerializeField] Item itemDragging;
        [SerializeField] Transform modelsHolding; // là model object temp có màu xanh đang kéo thả
        [SerializeField] bool _isDragging;
        [SerializeField] bool _enableSnapping; // bật chế độ snapping
        [SerializeField] float _rotationSpeed = 0.1f;// Tốc độ xoay
        [SerializeField] float _tileSize = 1; // ô snap tỷ lệ snap
        [SerializeField] Vector3 _tileOffset = Vector3.zero; // tỷ lệ snap + sai số này
        [SerializeField] string _groundTag = "Ground";
        [SerializeField] Material _green, _red;
        [SerializeField] Transform _modelsHolder;
        [SerializeField] UIRaycastChecker _uIRaycastChecker;
        [SerializeField] SensorCast _sensorAround;
        [SerializeField] SensorCast _sensorGround;

        InputImprove _inputImprove;
        RaycastCursor _raycastCursor;
        NavMeshManager _navMeshManager;

        public bool IsDragging { get => _isDragging; set => _isDragging = value; }
        public Item ItemDragging { get => itemDragging; set => itemDragging = value; }
        public Transform ModelsHolding { get => modelsHolding; set => modelsHolding = value; }

        public static event Action ActionDropItem;

        private void OnValidate()
        {
            _inputImprove = InputImprove.Instance;
            _raycastCursor = RaycastCursor.Instance;
            _navMeshManager = NavMeshManager.Instance;
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _inputImprove.Click += InputActionClick;
            _inputImprove.SnapPerformed += InputActionSnap;
        }

        private void OnDisable()
        {
            _inputImprove.Click -= InputActionClick;
            _inputImprove.SnapPerformed -= InputActionSnap;
        }

        private void FixedUpdate()
        {
            SetMaterial();
            Dragging();
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
            ModelsHolding = Instantiate(item.Models, _modelsHolder);
            _modelsHolder.localRotation = item.transform.rotation;

            // Cho item này lênh tay player
            item.SetParent(PlayerCtrl.Instance.PosHoldParcel, null, false);

            IsDragging = true;
            ItemDragging = item;
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

        public void InputActionSnap(InputAction.CallbackContext ctx)
        {
            _enableSnapping = !_enableSnapping;
        }

        /// <summary> set up lại value khi đặt item </summary>
        public void DropItem()
        {
            Destroy(ModelsHolding.gameObject); // Delete model item
            ItemDragging.DropItem(ModelsHolding);
            ItemDragging = null;
            IsDragging = false;
            gameObject.SetActive(false);
            _navMeshManager.RebuildNavMeshes();
            ActionDropItem?.Invoke();
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
            return _sensorAround._hits.Count == 0 && IsTouchGround() && ItemDragging;
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

        /// <summary> Action Event </summary>
        public void InputActionClick(InputAction.CallbackContext context)
        {
            TryDropItem();
        }

        public bool TryDropItem()
        {
            if (IsCanPlant())
            {
                DropItem();
                return true;
            }
            return false;
        }

        private void Dragging()
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
            float rotationAngle = Mathf.Round(_inputImprove.MouseScroll() * _rotationSpeed / 10.0f) * 10.0f;  // Tính toán góc xoay mới dựa trên giá trị cuộn chuột
            float newAngle = roundedAngle + rotationAngle; // Cộng góc xoay mới vào góc xoay đã làm tròn

            _modelsHolder.localRotation = Quaternion.Euler(0, newAngle, 0);
        }

        private RaycastHit GetRayHit() => _raycastCursor.GetRayMouseHit();
        private RaycastHit GetDragPointHit() => _raycastCursor.GetRayDragPointHit();

    }
}