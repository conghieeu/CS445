using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CuaHang
{
    public class CameraControl : Singleton<CameraControl>
    {
        [SerializeField] bool _isTouchRotationArea;
        [SerializeField] bool _isMoveStick; 
        [SerializeField] float _camSizeDefault = 5; 
        [SerializeField] float _moveSpeed;
        [SerializeField] Transform _itemFollow; // là đối tượng cam theo dỏi
        [SerializeField] Item _itemEditing; // item zoom vào
        [SerializeField] Transform _camshaft; // trục xoay cam
        [SerializeField] Transform _cameraHolder; // vị trí cam

        [Header("Rotation cam")]
        [SerializeField] float _rotationSpeed;

        [Header("Zoom Cam")]
        [SerializeField] float _zoomCamSpeed = 0.2f;

        Coroutine zoomCoroutine;
        RaycastCursor _raycastCursor;
        Camera _cam;
        ItemDrag _itemDrag;
        InputImprove _input;

        public Transform ItemFollow { get => _itemFollow; private set => _itemFollow = value; }
        public Item ItemEditing { get => _itemEditing; private set => _itemEditing = value; }
        public bool IsMoveStick { get => _isMoveStick; set => _isMoveStick = value; }
        public bool IsTouchRotationArea { get => _isTouchRotationArea; set => _isTouchRotationArea = value; }

        public static event Action<Item> _EventOnEditItem;

        protected override void Awake()
        {
            base.Awake();
            _raycastCursor = RaycastCursor.Instance;
            _itemDrag = SingleModuleManager.Instance._itemDrag;
            _input = InputImprove.Instance;
            _cam = Camera.main;
        }

        private void OnEnable()
        {
            GameSettingStats._OnDataChange += OnSettingLoad;
            _input.EditItem += EditItem;
            _input.Cancel += CancelFollowItem;
            _input.FollowItem += CamFollowItem;
            _input.SecondTouchContactStart += _ => PinchStart();
            _input.SecondTouchContactCancel += _ => PinchEnd();
        }

        private void OnDisable()
        {
            GameSettingStats._OnDataChange -= OnSettingLoad;
            _input.EditItem -= EditItem;
            _input.Cancel -= CancelFollowItem;
            _input.FollowItem -= CamFollowItem;
            _input.SecondTouchContactStart -= _ => PinchStart();
            _input.SecondTouchContactCancel -= _ => PinchEnd();
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
            if (ItemEditing == false && ItemFollow != null)
            {
                // Move follow Object
                _camshaft.position = Vector3.MoveTowards(_camshaft.position, ItemFollow.position, _moveSpeed * Time.deltaTime);
                _cam.transform.position = _cameraHolder.position;
                _cam.transform.rotation = _cameraHolder.rotation;

                // Xoay cam  
                if (GameSystem.CurrentPlatform == Platform.Standalone && _input.MouseRightClick())
                {
                    _camshaft.Rotate(Vector3.up, _input.MouseAxisX() * _rotationSpeed, Space.Self);
                }
                else if (GameSystem.CurrentPlatform == Platform.Android && _isTouchRotationArea && _input.MouseLeftClick())
                {
                    _camshaft.Rotate(Vector3.up, _input.MouseAxisX() * _rotationSpeed, Space.Self);
                }
            }
        }

        #region Zoom Detection
        void PinchStart()
        {
            zoomCoroutine = StartCoroutine(ZoomDetection());
        }

        void PinchEnd()
        {
            StopCoroutine(zoomCoroutine);
        }

        IEnumerator ZoomDetection()
        {
            float previousDistance = 0f, distance = 0f;
            while (true)
            {
                if (_isMoveStick) break;

                distance = Vector2.Distance(_input.PrimaryFingerPosition(), _input.SecondFingerPosition());

                // Detection
                // Zoom out
                if (distance > previousDistance)
                {
                    // Thực hiện hành động zoom out
                    float newSize = _cam.orthographicSize - _zoomCamSpeed;
                    if (newSize < 1) newSize = 1;

                    _cam.orthographicSize = newSize;

                }
                // Zoom in
                else if (distance < previousDistance)
                {
                    // Thực hiện hành động zoom in
                    float newSize = _cam.orthographicSize + _zoomCamSpeed;
                    if (newSize > 5) newSize = 5;

                    _cam.orthographicSize = newSize;
                }

                // Keep track of previous distance for next loop
                previousDistance = distance;

                yield return null; // Đợi đến khung hình tiếp theo
            }
        }
        #endregion

        void OnSettingLoad(GameSettingsData data)
        {
            _camshaft.rotation = data._camRotation;
        }

        /// <summary> Nhìn vào player </summary>
        void CamFollowPlayer()
        {
            if (_itemDrag.gameObject.activeInHierarchy || !ItemFollow)
            {
                SetObjectFollow(PlayerCtrl.Instance.transform);
            }
        }

        void CancelFollowItem(InputAction.CallbackContext context)
        {
            if (ItemEditing)
            {
                ItemEditing.SetEditMode(false);
                ItemEditing = null;
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
                ItemFollow = item;
            }
        }

        /// <summary> cam sẽ nhìn vào cái gì </summary>
        void SetObjectFollow(Transform objectF)
        {
            ItemFollow = objectF;
            _EventOnEditItem?.Invoke(null);
            _cam.orthographicSize = _camSizeDefault;

            if (ItemEditing)
            {
                ItemEditing.SetEditMode(false);
                ItemEditing = null;
            }
        }

        /// <summary> cam tập trung vaò item edit </summary>
        private void EditItem(InputAction.CallbackContext context)
        {
            Item item = _raycastCursor._ItemSelect.GetComponentInChildren<Item>();

            if (item && item._camHere && !ItemEditing)
            {
                ItemEditing = item;
                ItemEditing.SetEditMode(true);
                _EventOnEditItem?.Invoke(item);
                return;
            }

            if (ItemEditing)
            {
                SetObjectFollow(ItemFollow);
            }
        }


    }
}