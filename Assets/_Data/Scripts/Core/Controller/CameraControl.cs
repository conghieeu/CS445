using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace CuaHang
{
    public class CameraControl : GameBehavior
    {
        [Header("CAMERA CONTROL")]
        public Item ItemEdit;

        [SerializeField] bool _isMoveStick;
        [SerializeField] float _camSizeDefault = 5;
        [SerializeField] float _moveSpeed = 10;
        [SerializeField] Transform _objectFollow; // là đối tượng cam theo dỏi 
        [SerializeField] Transform _cameraHolder; // vị trí cam
        [SerializeField] float _rotationSpeed = 1;
        [SerializeField] float _zoomCamSpeed = 0.2f;

        Coroutine _zoomCoroutine;
        GameSettings m_GameSettings;
        PlayerCtrl m_PlayerCtrl;
        RaycastCursor m_RaycastCursor;
        GameSystem m_GameSystem;
        InputImprove m_InputImprove;
        Camera m_Cam;

        public bool IsMoveStick { get => _isMoveStick; set => _isMoveStick = value; }

        private void Start()
        {
            m_RaycastCursor = FindFirstObjectByType<RaycastCursor>();
            m_PlayerCtrl = FindFirstObjectByType<PlayerCtrl>();
            m_GameSettings = FindFirstObjectByType<GameSettings>();
            m_GameSystem = FindFirstObjectByType<GameSystem>();
            m_InputImprove = FindFirstObjectByType<InputImprove>();

            m_GameSettings.ActionDataChange += OnGameSettingChange;
            m_RaycastCursor.ActionEditItem += OnEditItem;
            m_RaycastCursor.ActionFollowItem += SetFollowItem;
            m_InputImprove.SecondTouchContact.action.started += ctx => PinchStart();
            m_InputImprove.SecondTouchContact.action.canceled += ctx => PinchEnd();

            m_Cam = Camera.main;

            // chỉnh lại góc xoay theo setting
            transform.rotation = m_GameSettings.CamRotation;
        }

        private void FixedUpdate()
        {
            // save cam rotation
            m_GameSettings.CamRotation = transform.rotation;

            FollowObjectTarget();
        }

        public void FollowObjectTarget()
        {
            if (ItemEdit) return;
            if (_objectFollow)
            {
                // cam follow Object
                transform.position = Vector3.MoveTowards(transform.position, _objectFollow.position, _moveSpeed * Time.deltaTime);
                m_Cam.transform.position = _cameraHolder.position;
                m_Cam.transform.rotation = _cameraHolder.rotation;
            }
            else
            {
                _objectFollow = m_PlayerCtrl.transform;
            }
        }

        /// <summary> Điều khiển cam </summary>
        public void CamRotation()
        {
            if (ItemEdit) return;

            bool rightClick = m_InputImprove.RightClick.action.IsPressed();
            bool leftClick = m_InputImprove.LeftClick.action.IsPressed();
            float mouseAxisX = m_InputImprove.MouseMoveX.action.ReadValue<float>();

            if (m_GameSystem.CurrentPlatform == Platform.Standalone && rightClick)
            {
                transform.Rotate(Vector3.up, mouseAxisX * _rotationSpeed, Space.Self);
            }
            else if (m_GameSystem.CurrentPlatform == Platform.Android && leftClick)
            {
                transform.Rotate(Vector3.up, mouseAxisX * _rotationSpeed, Space.Self);
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
                if (_isMoveStick || ItemEdit) break;

                Vector2 primaryFingerPosition = m_InputImprove.PrimaryFingerPosition.action.ReadValue<Vector2>();
                Vector2 secondaryFingerPosition = m_InputImprove.PrimaryFingerPosition.action.ReadValue<Vector2>();

                distance = Vector2.Distance(primaryFingerPosition, secondaryFingerPosition);

                // Detection
                // Zoom out
                if (distance > previousDistance)
                {
                    // Thực hiện hành động zoom out
                    float newSize = m_Cam.orthographicSize - _zoomCamSpeed;
                    if (newSize < 1) newSize = 1;

                    m_Cam.orthographicSize = newSize;

                }
                // Zoom in
                else if (distance < previousDistance)
                {
                    // Thực hiện hành động zoom in
                    float newSize = m_Cam.orthographicSize + _zoomCamSpeed;
                    if (newSize > 5) newSize = 5;

                    m_Cam.orthographicSize = newSize;
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
            m_Cam.orthographicSize = _camSizeDefault;
        }

        private void OnEditItem(Item item)
        {
            ItemEdit = item;
            if (item && item.CamHere)
            {
                ItemEdit = item;
                m_Cam.orthographicSize = item.CamHere._camSize;
                m_Cam.transform.position = item.CamHere.transform.position;
                m_Cam.transform.rotation = item.CamHere.transform.rotation;
            }
            else
            {
                _objectFollow = null;
                m_Cam.orthographicSize = _camSizeDefault;
            }
        }
    }
}