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
        [SerializeField] float _moveSpeed = 10;
        [SerializeField] Transform _objectFollow; // là đối tượng cam theo dỏi 
        [SerializeField] Item _itemEdit;
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

        Coroutine _zoomCoroutine;

        GameSettings m_GameSettings;
        Camera _cam => Camera.main;
        PlayerCtrl m_PlayerCtrl;
        RaycastCursor m_RaycastCursor;
        GameSystem m_GameSystem;

        public bool IsMoveStick { get => _isMoveStick; set => _isMoveStick = value; }
        public bool IsTouchRotationArea { get => _isTouchRotationArea; set => _isTouchRotationArea = value; }

        private void Start()
        {
            m_RaycastCursor = FindFirstObjectByType<RaycastCursor>();
            m_PlayerCtrl = FindFirstObjectByType<PlayerCtrl>();
            m_GameSettings = FindFirstObjectByType<GameSettings>();
            m_GameSystem = FindFirstObjectByType<GameSystem>();

            m_GameSettings.ActionDataChange += OnGameSettingChange;
            m_RaycastCursor.ActionEditItem += OnEditItem;
            m_RaycastCursor.ActionFollowItem += SetFollowItem;
            secondTouchContact.action.started += ctx => PinchStart();
            secondTouchContact.action.canceled += ctx => PinchEnd();

            // chỉnh lại góc xoay theo setting
            transform.rotation = m_GameSettings.CamRotation;
        }

        private void FixedUpdate()
        {
            // save cam rotation
            m_GameSettings.CamRotation = transform.rotation;
        }

        private void Update()
        {
            CamController();
        }

        /// <summary> Điều khiển cam </summary>
        private void CamController()
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
                if (m_GameSystem.CurrentPlatform == Platform.Standalone && rightClick)
                {
                    transform.Rotate(Vector3.up, mouseAxisX * _rotationSpeed, Space.Self);
                }
                else if (m_GameSystem.CurrentPlatform == Platform.Android && _isTouchRotationArea && leftClick)
                {
                    transform.Rotate(Vector3.up, mouseAxisX * _rotationSpeed, Space.Self);
                }
            }
            else
            {
                _objectFollow = m_PlayerCtrl.transform;
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

        private void OnGameSettingChange(GameSettings data)
        {
            if (this)
            {
                transform.rotation = data.CamRotation;
            }
        }

        /// <summary> cam sẽ nhìn vào cái gì </summary>
        private void SetFollowItem(Item item)
        {
            _objectFollow = item.transform;
            _cam.orthographicSize = _camSizeDefault;
        }

        private void OnEditItem(Item item)
        {
            _itemEdit = item;
            if (item && item.CamHere)
            {
                _itemEdit = item;
                _cam.orthographicSize = item.CamHere._camSize;
                _cam.transform.position = item.CamHere.transform.position;
                _cam.transform.rotation = item.CamHere.transform.rotation;
            }
            else
            {
                _objectFollow = null;
                _cam.orthographicSize = _camSizeDefault;
            }
        }
    }
}