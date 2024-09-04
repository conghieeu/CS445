using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CuaHang
{
    public class CameraControl : Singleton<CameraControl>
    {
        public float _camSizeDefault = 5;
        public float _rotationSpeed;
        public float _moveSpeed;
        public Transform _itemFollow; // là đối tượng theo giỏi object forcus
        public Item _itemEditing;
        public Transform _camshaft; // trục xoay của cam
        public Transform _cameraHolder;
        RaycastCursor _raycastCursor;
        Camera _cam;
        ItemDrag _itemDrag;
        InputImprove _input;

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

        void FixedUpdate()
        {
            // save cam rotation
            GameSettings.Instance._CamRotation = _camshaft.rotation;
            CamFollowPlayer();
        }

        void Update()
        {
            CamController();
        }

        /// <summary> Điều khiển cam </summary>
        private void CamController()
        {
            if (_itemEditing == false)
            {
                // Move follow Object
                _camshaft.position = Vector3.MoveTowards(_camshaft.position, _itemFollow.position, _moveSpeed * Time.deltaTime);
                _cam.transform.position = _cameraHolder.position;
                _cam.transform.rotation = _cameraHolder.rotation;

                // Lấy giá trị delta của chuột (sự thay đổi vị trí chuột) 
                if (_input.TwoPress())
                {
                    // Xoay đối tượng quanh trục Y dựa trên giá trị delta của chuột
                    _camshaft.Rotate(Vector3.up, _input.MouseAxisX() * _rotationSpeed, Space.Self);
                }
            }
        }

        private void OnSettingLoad(GameSettingsData data)
        {
            _camshaft.rotation = data._camRotation;
        }

        /// <summary> Nhìn vào player </summary>
        private void CamFollowPlayer()
        {
            if (_itemDrag.gameObject.activeInHierarchy || !_itemFollow)
            {
                SetObjectFollow(PlayerCtrl.Instance.transform);
            }
        }

        private void CancelFollowItem(InputAction.CallbackContext context)
        {
            if (_itemEditing)
            {
                _itemEditing.SetEditMode(false);
                _itemEditing = null;
            }

            _EventOnEditItem?.Invoke(null);
            SetObjectFollow(PlayerCtrl.Instance.transform);
        }

        /// <summary> Để cam nhìn vào character follow </summary>
        private void CamFollowItem(InputAction.CallbackContext context)
        {
            Transform item = _raycastCursor._itemFocus;

            // F để tập trung vào đối tượng
            if (item && item.GetComponent<Item>())
            {
                _itemFollow = item;
            }
        }

        /// <summary> cam sẽ nhìn vào cái gì </summary>
        private void SetObjectFollow(Transform objectF)
        {
            _itemFollow = objectF;
            _EventOnEditItem?.Invoke(null);
            _cam.orthographicSize = _camSizeDefault;

            if (_itemEditing)
            {
                _itemEditing.SetEditMode(false);
                _itemEditing = null;
            }
        }

        /// <summary> cam tập trung vaò item edit </summary>
        private void EditItem(InputAction.CallbackContext context)
        {
            Item item = _raycastCursor._itemFocus.GetComponentInChildren<Item>();

            if (item && item._camHere && !_itemEditing)
            {
                _itemEditing = item;
                _itemEditing.SetEditMode(true);
                _EventOnEditItem?.Invoke(item);
                return;
            }

            if (_itemEditing)
            {

                SetObjectFollow(_itemFollow);
            }
        }


    }
}