using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CuaHang
{
    /// <summary> ObjectTemp là đối tượng đại diện cho object Plant khi di dời đối tượng </summary>
    public class ItemDrag : MonoBehaviour
    {
        [Space]
        public bool _isDragging;
        public bool _isDistance;

        [Space]
        public Transform _modelsHolding; // là model object temp có màu xanh đang kéo thả
        public Item _itemDragging;
        [SerializeField] string _groundTag = "Ground";
        [SerializeField] Material _green, _red;
        [SerializeField] NavMeshSurface _navMeshSurface;

        [Space]
        [SerializeField] Transform _modelsHolder;
        [SerializeField] SensorCast _sensorAround;
        [SerializeField] SensorCast _sensorGround;

        InputImprove _input;

        private void Awake()
        {
            _input = new();
        }

        private void OnEnable()
        {
            _input.Click += ClickToDropItem;
        }

        private void OnDisable()
        {
            _input.Click -= ClickToDropItem;
        }

        private void FixedUpdate()
        {
            SetMaterial();
        }


        /// <summary> để model temp đang dragging nó hiện giống model đang di chuyển ở thằng Player </summary>
        public void PickUpItem(Item item)
        {
            // Bật object drag
            gameObject.SetActive(true);

            // Tạo model giống otherModel ở vị trí 
            _modelsHolding = Instantiate(item._models, _modelsHolder);
            _modelsHolding.transform.localRotation = item.transform.rotation;

            // Cho item này lênh tay player
            item.SetParent(PlayerCtrl.Instance._posHoldParcel, null, true);

            _isDragging = true;
            _itemDragging = item;
        }

        void ClickToDropItem(InputAction.CallbackContext context)
        { 
            if (IsCanPlant() && _itemDragging && !EventSystem.current.IsPointerOverGameObject())
            {
                OnDropItem();
                _navMeshSurface.BuildNavMesh();
            }
        }

        /// <summary> Huỷ tôi không muốn đặt item nữa </summary>
        public void OnDropItem()
        {
            Destroy(_modelsHolding.gameObject); // Delete model item
            _itemDragging.DropItem(_modelsHolding);
            _itemDragging = null;
            _isDragging = false;
            gameObject.SetActive(false);
        }

        void SetMaterial()
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

        void SetMaterialModel(Material color)
        {
            foreach (Renderer model in _modelsHolder.GetComponentsInChildren<Renderer>())
            {
                model.material = color;
            }
        }

        bool IsCanPlant()
        {
            return _sensorAround._hits.Count == 0 && IsTouchGround() && _isDistance;
        }

        bool IsTouchGround()
        {
            // Làm thế nào để cái sensor check ở dưới
            foreach (var obj in _sensorGround._hits)
            {
                if (obj.CompareTag(_groundTag))
                {
                    return true;
                }
            }
            return false;
        }
    }
}