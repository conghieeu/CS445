
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
        [SerializeField] Transform _camshaft; // trục xoay cam
        [SerializeField] Transform _cameraHolder; // vị trí cam
        [SerializeField] float _rotationSpeed;
        [SerializeField] float _zoomCamSpeed = 0.2f;

        [SerializeField] Item _itemEdit;
        [SerializeField] Item _itemSelect;
        [SerializeField] Item _itemFollow;

        Coroutine _zoomCoroutine;
        Camera _cam => Camera.main;
        ModuleDragItem _itemDrag => RaycastCursor.Instance.ItemDrag;
        InputImprove _input => InputImprove.Instance;
        PlayerCtrl _playerCtrl => PlayerCtrl.Instance;

        public bool IsMoveStick { get => _isMoveStick; set => _isMoveStick = value; }
        public bool IsTouchRotationArea { get => _isTouchRotationArea; set => _isTouchRotationArea = value; }
        public Quaternion CamshaftRotation { get => _camshaft.rotation; set => _camshaft.rotation = value;}

        private void Start()
        {
            SetFollowPlayer();
        }

        private void OnEnable()
        {
            GameSettings.ActionDataChange += OnGameSettingChange;

            RaycastCursor.ActionEditItem += SetItemEdit;
            RaycastCursor.ActionFollowItem += SetFollowItem;
            RaycastCursor.ActionSelectItem += OnItemSelected;

            _input.SecondTouchContactStart += PinchStart;
            _input.SecondTouchContactCancel += PinchEnd;
        }

        private void OnDisable()
        {
            GameSettings.ActionDataChange -= OnGameSettingChange;

            RaycastCursor.ActionEditItem -= SetItemEdit;
            RaycastCursor.ActionFollowItem -= SetFollowItem;
            RaycastCursor.ActionSelectItem += OnItemSelected;

            _input.SecondTouchContactStart -= PinchStart;
            _input.SecondTouchContactCancel -= PinchEnd;
        }

        private void FixedUpdate()
        {
            // save cam rotation
            GameSettings.Instance.CamRotation = _camshaft.rotation;
        }

        private void Update()
        {
            CamController();
        }

        /// <summary> Điều khiển cam </summary>
        void CamController()
        {
            if (_objectFollow && _itemEdit == null)
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
        void PinchStart(InputAction.CallbackContext ctx)
        {
            _zoomCoroutine = StartCoroutine(ZoomDetection());
        }

        void PinchEnd(InputAction.CallbackContext ctx)
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

        private void OnItemSelected(Item item)
        {
            _itemSelect = item;
        }

        void OnGameSettingChange(GameSettings data)
        {
            if (_camshaft)
            {
                _camshaft.rotation = data.CamRotation;
            }
        }

        /// <summary> Nhìn vào player </summary>
        void SetFollowPlayer()
        {
            if (_itemDrag.gameObject.activeInHierarchy || !_objectFollow)
            {
                SetObjectFollow(_playerCtrl.transform);
            }
        }

        /// <summary> Người chơi nhần F Để cam nhìn vào character follow </summary>
        void SetFollowItem(Item item)
        {
            _itemFollow = item;
            SetObjectFollow(item.transform);
        }

        /// <summary> cam sẽ nhìn vào cái gì </summary>
        void SetObjectFollow(Transform objectFollow)
        {
            if (objectFollow)
            {
                _objectFollow = objectFollow;
                _cam.orthographicSize = _camSizeDefault;
            }
        }

        void SetItemEdit(Item item)
        {
            if (item) // zoom in
            {
                item.CamHere.SetCamFocusHere(_cam);
            }
            else // zoom out
            {
                if (_objectFollow) SetObjectFollow(_objectFollow.transform);
            }

            _itemEdit = item;
        }


    }
}