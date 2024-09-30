
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CuaHang
{
    public class CameraControl : GameBehavior
    {
        [Header("CAMERA CONTROL")]
        [SerializeField] bool _isTouchRotationArea;
        [SerializeField] bool _isMoveStick;
        [SerializeField] float _camSizeDefault = 5;
        [SerializeField] float _moveSpeed = 1;
        [SerializeField] Transform _objectFollow; // là đối tượng cam theo dỏi 
        [SerializeField] Transform _cameraHolder; // vị trí cam
        [SerializeField] float _rotationSpeed = 1;
        [SerializeField] float _zoomCamSpeed = 0.2f;
        [Header("Input Action")]
        [SerializeField] InputActionReference mouseAxisX;
        [SerializeField] InputActionReference leftClick;
        [SerializeField] InputActionReference rightClick;
        [SerializeField] InputActionReference secondTouchContact;
        [SerializeField] InputActionReference primaryFingerPosition;
        [SerializeField] InputActionReference secondaryFingerPosition;

        Item _itemEdit;
        Item _itemSelect;
        Item _itemFollow;
        Coroutine _zoomCoroutine;

        ObjectsManager objectsManager => ObjectsManager.Instance;
        ModuleDragItem moduleDragItem => objectsManager.ModuleDragItem;
        GameSettings gameSettings => objectsManager.GameSettings;

        Camera _cam => Camera.main;
        PlayerCtrl _playerCtrl => PlayerCtrl.Instance;

        public bool IsMoveStick { get => _isMoveStick; set => _isMoveStick = value; }
        public bool IsTouchRotationArea { get => _isTouchRotationArea; set => _isTouchRotationArea = value; }

        private void Start()
        {
            GameSettings.ActionDataChange += OnGameSettingChange;

            RaycastCursor.ActionEditItem += OnEditItem;
            RaycastCursor.ActionFollowItem += OnFollowItem;
            RaycastCursor.ActionSelectItem += OnItemSelected;

            secondTouchContact.action.started += ctx => PinchStart();
            secondTouchContact.action.started += ctx => PinchEnd();

            SetFollowPlayer();
        }

        private void FixedUpdate()
        {
            // save cam rotation
            gameSettings.CamRotation = transform.rotation;
        }

        private void Update()
        {
            CamController();
        }

        /// <summary> Điều khiển cam </summary>
        void CamController()
        {
            if (_itemEdit) return;

            if (_objectFollow)
            {
                // cam follow Object
                transform.position = Vector3.MoveTowards(transform.position, _objectFollow.position, _moveSpeed * Time.deltaTime);
                _cam.transform.position = _cameraHolder.position;
                _cam.transform.rotation = _cameraHolder.rotation;

                bool rightClick = this.rightClick.action.IsPressed();
                bool leftClick = this.leftClick.action.IsPressed();
                float mouseAxisX = this.mouseAxisX.action.ReadValue<float>();

                // Xoay cam  
                if (GameSystem.CurrentPlatform == Platform.Standalone && rightClick)
                {
                    transform.Rotate(Vector3.up, mouseAxisX * _rotationSpeed, Space.Self);
                }
                else if (GameSystem.CurrentPlatform == Platform.Android && _isTouchRotationArea && leftClick)
                {
                    transform.Rotate(Vector3.up, mouseAxisX * _rotationSpeed, Space.Self);
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

                Vector2 primaryFingerPosition = this.primaryFingerPosition.action.ReadValue<Vector2>();
                Vector2 secondaryFingerPosition = this.secondaryFingerPosition.action.ReadValue<Vector2>();

                distance = Vector2.Distance(primaryFingerPosition, secondaryFingerPosition);

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
            transform.rotation = data.CamRotation;
        }

        /// <summary> Nhìn vào player </summary>
        void SetFollowPlayer()
        {
            if (moduleDragItem.gameObject.activeInHierarchy || !_objectFollow)
            {
                SetObjectFollow(_playerCtrl.transform);
            }
        }

        /// <summary> Người chơi nhần F Để cam nhìn vào character follow </summary>
        void OnFollowItem(Item item)
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

        void OnEditItem(Item item)
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