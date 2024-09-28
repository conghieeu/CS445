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

        RaycastCursor _raycastCursor;
        NavMeshManager _navMeshManager;

        [Header("Input Action")]
        [SerializeField] InputActionReference inputMousePosition;
        [SerializeField] InputActionReference inputLeftClick;
        [SerializeField] InputActionReference inputSnap;
        [SerializeField] InputActionReference inputMouseScrollX;

        public bool IsDragging { get => _isDragging; set => _isDragging = value; }
        public Item ItemDragging { get => itemDragging; set => itemDragging = value; }
        public Transform ModelsHolding { get => modelsHolding; set => modelsHolding = value; }

        public event Action ActionDropItem; 

        private void Start()
        { 
            _navMeshManager = ObjectsManager.Instance.NavMeshManager;
            _raycastCursor = ObjectsManager.Instance.RaycastCursor;
        }

        private void OnEnable()
        {
            inputLeftClick.action.performed += InputLeftClick;
            inputSnap.action.performed += InputActiveSnap;
        }

        private void OnDisable()
        {
            inputLeftClick.action.performed -= InputLeftClick;
            inputSnap.action.performed -= InputActiveSnap;
        }

        private void Update()
        {
            SetMaterial();

            if (GameSystem.CurrentPlatform == Platform.Standalone)
            {
                Vector3 mousePos = inputMousePosition.action.ReadValue<Vector2>();
                RaycastHit raycastHit = _raycastCursor.GetMouseRaycastHit(mousePos);
                Vector3 hitPoint = raycastHit.transform.position;

                // di chuyển item trên nền tảng pc , di chuyển theo chuột
                DragItem(hitPoint);

                // xoay item
                RotationItemDrag(raycastHit);
            }
        }

        /// <summary> để model temp đang dragging nó hiện giống model đang di chuyển ở thằng Player </summary>
        public void PickUpItem(Item item)
        { 
            SetActive(true);
            // Tạo model giống otherModel ở vị trí 
            ModelsHolding = Instantiate(item.Models, _modelsHolder);
            _modelsHolder.localRotation = item.transform.rotation;
            // Cho item này lênh tay player
            item.SetParent(PlayerCtrl.Instance.PosHoldParcel, null, false);
            IsDragging = true;
            ItemDragging = item;
        }

        /// <summary> Button quay item drag sẽ gọi </summary>
        public void OnClickRotation(float angle)
        {
            float currentAngle = _modelsHolder.localEulerAngles.y;
            float roundedAngle = Mathf.Round(currentAngle / 10.0f) * 10.0f;
            float rotationAngle = Mathf.Round(angle * _rotationSpeed / 10.0f) * 10.0f;
            float newAngle = roundedAngle + rotationAngle;
            _modelsHolder.localRotation = Quaternion.Euler(0, newAngle, 0);
        }

        public void InputActiveSnap(InputAction.CallbackContext ctx)
        {
            _enableSnapping = !_enableSnapping;
        }

        /// <summary> Action Event </summary>
        public void InputLeftClick(InputAction.CallbackContext context)
        {
            if (GameSystem.Instance._Platform == Platform.Standalone)
            {
                TryDropItem();
            }
        }

        public bool TryDropItem()
        {
            if (IsCanPlant())
            { 
                OnDropItem();
                return true;
            }
            return false;
        }

        /// <summary> set up lại value khi đặt item </summary>
        public void OnDropItem()
        {
            Destroy(ModelsHolding.gameObject); // Delete model item
            gameObject.SetActive(false);
            ItemDragging.DropItem(ModelsHolding);
            ItemDragging = null;
            IsDragging = false;
            _navMeshManager.RebuildNavMeshes();
            ActionDropItem?.Invoke();
        }

        public void DragItem(Vector3 hitPos)
        {
            //  Làm tròn vị trí temp để nó giống snap
            if (_enableSnapping)
            {
                float sX = Mathf.Round(hitPos.x / _tileSize) * _tileSize + _tileOffset.x;
                float sZ = Mathf.Round(hitPos.z / _tileSize) * _tileSize + _tileOffset.z;
                float sY = Mathf.Round(hitPos.y / _tileSize) * _tileSize + _tileOffset.y;
                hitPos = new Vector3(sX, sY, sZ);
            }

            transform.position = hitPos;
        }

        /// <summary> Xoay item </summary>
        private void RotationItemDrag(RaycastHit hit)
        {
            // Transform: lấy vuông góc với bề mặt va chạm
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            // Model holder: lấy góc xoay mới
            float currentAngle = _modelsHolder.localEulerAngles.y;
            float roundedAngle = Mathf.Round(currentAngle / 10.0f) * 10.0f; // Làm tròn góc xoay hiện tại về hàng chục gần nhất
            float rotationAngle = Mathf.Round(inputMouseScrollX.action.ReadValue<float>() * _rotationSpeed / 10.0f) * 10.0f;  // Tính toán góc xoay mới dựa trên giá trị cuộn chuột
            float newAngle = roundedAngle + rotationAngle; // Cộng góc xoay mới vào góc xoay đã làm tròn

            _modelsHolder.localRotation = Quaternion.Euler(0, newAngle, 0);
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
            foreach (var hit in _sensorGround._hits)
            {
                if (hit.CompareTag(_groundTag))
                {
                    return true;
                }
            }
            return false;
        }
    }
}