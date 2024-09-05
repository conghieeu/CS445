using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CuaHang
{
    public class CameraControl : Singleton<CameraControl>
    {
        [SerializeField] float _camSizeDefault = 5;
        [SerializeField] float _rotationSpeed;
        [SerializeField] float _moveSpeed;
        [SerializeField] Transform _itemFollow; // là đối tượng cam theo dỏi
        [SerializeField] Item _itemEditing; // item zoom vào
        [SerializeField] Transform _camshaft; // trục xoay cam
        [SerializeField] Transform _cameraHolder; // vị trí cam

        RaycastCursor _raycastCursor;
        Camera _cam;
        ItemDrag _itemDrag;
        InputImprove _input;

        public Transform _ItemFollow { get => _itemFollow; private set => _itemFollow = value; }
        public Item _ItemEditing { get => _itemEditing; private set => _itemEditing = value; }

        public static event Action<Item> _EventOnEditItem;

        protected override void Awake()
        {
            base.Awake();
            _raycastCursor = RaycastCursor.Instance;
            _itemDrag = SingleModuleManager.Instance._itemDrag;
            _input = new();
            _cam = Camera.main;
        }

        private void OnEnable()
        {
            GameSettingStats._OnDataChange += OnSettingLoad;
            _input.EditItem += EditItem;
            _input.Cancel += CancelFollowItem;
            _input.FollowItem += CamFollowItem;
        }

        private void OnDisable()
        {
            GameSettingStats._OnDataChange -= OnSettingLoad;
            _input.EditItem -= EditItem;
            _input.Cancel -= CancelFollowItem;
            _input.FollowItem -= CamFollowItem;
        }

        private void FixedUpdate()
        {
            // save cam rotation
            GameSettings.Instance._CamRotation = _camshaft.rotation;
            CamFollowPlayer();
        }

        private void Update()
        {
            CamController();
        }

        /// <summary> Điều khiển cam </summary>
        void CamController()
        {
            if (_ItemEditing == false)
            {
                // Move follow Object
                _camshaft.position = Vector3.MoveTowards(_camshaft.position, _ItemFollow.position, _moveSpeed * Time.deltaTime);
                _cam.transform.position = _cameraHolder.position;
                _cam.transform.rotation = _cameraHolder.rotation;

                // Lấy giá trị delta của chuột (sự thay đổi vị trí chuột) 
                if (_input.DoubleTouchScreen() || _input.MouseRightClick())
                {
                    // Xoay đối tượng quanh trục Y dựa trên giá trị delta của chuột
                    _camshaft.Rotate(Vector3.up, _input.MouseAxisX() * _rotationSpeed, Space.Self);
                }
            }
        }

        void OnSettingLoad(GameSettingsData data)
        {
            _camshaft.rotation = data._camRotation;
        }

        /// <summary> Nhìn vào player </summary>
        void CamFollowPlayer()
        {
            if (_itemDrag.gameObject.activeInHierarchy || !_ItemFollow)
            {
                SetObjectFollow(PlayerCtrl.Instance.transform);
            }
        }

        void CancelFollowItem(InputAction.CallbackContext context)
        {
            if (_ItemEditing)
            {
                _ItemEditing.SetEditMode(false);
                _ItemEditing = null;
            }

            _EventOnEditItem?.Invoke(null);
            SetObjectFollow(PlayerCtrl.Instance.transform);
        }

        /// <summary> Để cam nhìn vào character follow </summary>
        void CamFollowItem(InputAction.CallbackContext context)
        {
            Transform item = _raycastCursor._ItemSelect;

            // F để tập trung vào đối tượng
            if (item && item.GetComponent<Item>())
            {
                _ItemFollow = item;
            }
        }

        /// <summary> cam sẽ nhìn vào cái gì </summary>
        void SetObjectFollow(Transform objectF)
        {
            _ItemFollow = objectF;
            _EventOnEditItem?.Invoke(null);
            _cam.orthographicSize = _camSizeDefault;

            if (_ItemEditing)
            {
                _ItemEditing.SetEditMode(false);
                _ItemEditing = null;
            }
        }

        /// <summary> cam tập trung vaò item edit </summary>
        private void EditItem(InputAction.CallbackContext context)
        {
            Item item = _raycastCursor._ItemSelect.GetComponentInChildren<Item>();

            if (item && item._camHere && !_ItemEditing)
            {
                _ItemEditing = item;
                _ItemEditing.SetEditMode(true);
                _EventOnEditItem?.Invoke(item);
                return;
            }

            if (_ItemEditing)
            {
                SetObjectFollow(_ItemFollow);
            }
        }


    }
}