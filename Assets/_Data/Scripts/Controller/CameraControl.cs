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
        [SerializeField] Transform _objectFollow; // là đối tượng cam theo dỏi
        [SerializeField] Item _itemEdit; // item zoom vào
        [SerializeField] Transform _camshaft; // trục xoay cam
        [SerializeField] Transform _cameraHolder; // vị trí cam
        [SerializeField] float _rotationSpeed;
        [SerializeField] float _zoomCamSpeed = 0.2f;

        Coroutine _zoomCoroutine;
        Camera _cam => Camera.main;
        ItemDrag _itemDrag => RaycastCursor.Instance._ItemDrag;
        InputImprove _input => InputImprove.Instance;
        PlayerCtrl _playerCtrl => PlayerCtrl.Instance;

        public bool IsMoveStick { get => _isMoveStick; set => _isMoveStick = value; }
        public bool IsTouchRotationArea { get => _isTouchRotationArea; set => _isTouchRotationArea = value; }
        public Quaternion CamshaftRotation { get => _camshaft.rotation; }

        public static event Action<Item> OnEditItem;

        private void Start()
        {
            OnEditItem?.Invoke(_itemEdit);
        }

        private void OnEnable()
        {
            GameSettingStats._OnDataChange += OnSettingLoad;
            RaycastCursor.OnEditItem += SetItemEdit;
            RaycastCursor.OnFollowItem += SetItemFollow;
            _input.Cancel += _ => CancelFollowItem();
            _input.SecondTouchContactStart += _ => PinchStart();
            _input.SecondTouchContactCancel += _ => PinchEnd();
        }

        private void OnDisable()
        {
            GameSettingStats._OnDataChange -= OnSettingLoad;
            RaycastCursor.OnEditItem -= SetItemEdit;
            RaycastCursor.OnFollowItem -= SetItemFollow;
            _input.Cancel -= _ => CancelFollowItem();
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
            if (_objectFollow)
            {
                // cam follow Object
                _camshaft.position = Vector3.MoveTowards(_camshaft.position, _objectFollow.position, _moveSpeed * Time.deltaTime);
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
            _zoomCoroutine = StartCoroutine(ZoomDetection());
        }

        void PinchEnd()
        {
            if (_zoomCoroutine != null) StopCoroutine(_zoomCoroutine);
        }

        IEnumerator ZoomDetection()
        {
            float previousDistance = 0f, distance = 0f;
            while (true)
            {
                if (_isMoveStick || _itemEdit) break;

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
            if (_camshaft)
            {
                _camshaft.rotation = data.CamRotation;
            }
        }

        /// <summary> Nhìn vào player </summary>
        void CamFollowPlayer()
        {
            if (!_itemDrag.gameObject.activeInHierarchy || !_objectFollow)
            {
                SetObjectFollow(_playerCtrl.transform);
            }
        }

        void CancelFollowItem()
        {
            if (_itemEdit)
            {
                _itemEdit.SetEditMode(false);
                _itemEdit = null;
            }

            SetObjectFollow(_playerCtrl.transform);
            OnEditItem(_itemEdit);
        }

        /// <summary> Người chơi nhần F Để cam nhìn vào character follow </summary>
        void SetItemFollow(Item item)
        {
            if (item)
            {
                _objectFollow = item.transform;
            }
        }

        /// <summary> cam sẽ nhìn vào cái gì </summary>
        void SetObjectFollow(Transform objectFollow)
        {
            _objectFollow = objectFollow;
            _cam.orthographicSize = _camSizeDefault;
        }

        /// <summary> cam tập trung vaò item edit </summary>
        void SetItemEdit(Item item)
        {
            if (_itemEdit)
            {
                _itemEdit.SetEditMode(false);
                _itemEdit = null;
            }

            _itemEdit = item;

            if (_itemEdit)
            {
                _itemEdit.SetEditMode(true);
                _itemEdit = item;
            }

            OnEditItem(item);
        }

    }
}