using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CuaHang
{
    public class PlayerPlanting : GameBehavior
    {
        [Header("PLAYER PLANTING")]
        [Header("Input Action")]
        [SerializeField] InputActionReference inputActionSendItem;
        PlayerCtrl _playerCtrl;
        ModuleDragItem _moduleDragItem;

        public static event Action ActionSenderItem;

        private void Awake()
        {
            _playerCtrl = GetComponent<PlayerCtrl>();
        }

        private void Start()
        {
            _moduleDragItem = ObjectsManager.Instance.ModuleDragItem;
        }

        private void OnEnable()
        {
            inputActionSendItem.action.performed += SendItem;
        }

        private void OnDisable()
        {
            inputActionSendItem.action.performed -= SendItem;
        }

        private void FixedUpdate()
        {
            if (_moduleDragItem.IsDragging) TempAiming();
        }

        /// <summary> chạm vào kệ, người chơi có thể truyền item từ parcel sang table đó </summary>
        private void SendItem(InputAction.CallbackContext ctx)
        {
            // có item ở cảm biến
            Item shelf = _playerCtrl._sensorForward.GetItemTypeHit(Type.Shelf);
            Item trash = _playerCtrl._sensorForward.GetItemTypeHit(Type.Trash);
            Item storage = _playerCtrl._sensorForward.GetItemTypeHit(Type.Storage);
            Item itemHold = _moduleDragItem.ItemDragging;

            if (!itemHold) return;

            if (shelf && !itemHold.IsCanSell) // gửi các item trong parcel sang kệ
            {
                shelf.ItemSlot.ReceiverItems(itemHold.ItemSlot, true);
            }
            else if (shelf && itemHold.IsCanSell && shelf.ItemSlot.IsHasSlotEmpty()) // để item lênh kệ
            {
                _moduleDragItem.OnDropItem();
                shelf.ItemSlot.TryAddItemToItemSlot(itemHold, true);
                ActionSenderItem?.Invoke();
            }
            else if (trash && itemHold.Type == Type.Parcel && trash.ItemSlot.IsHasSlotEmpty()) // de parcel vao thung rac
            {
                _moduleDragItem.OnDropItem();
                trash.ItemSlot.TryAddItemToItemSlot(itemHold, true);
                ActionSenderItem?.Invoke();
            }
            else if (storage && itemHold.Type == Type.Parcel && storage.ItemSlot.IsHasSlotEmpty()) // de parcel vao kho
            {
                _moduleDragItem.OnDropItem();
                storage.ItemSlot.TryAddItemToItemSlot(itemHold, true);
                ActionSenderItem?.Invoke();
            }
        }

        /// <summary> Khi mà drag object Temp thì player sẽ hướng về object Temp </summary>
        private void TempAiming()
        {
            var direction = _moduleDragItem.transform.position - transform.position;
            direction.y = 0;
            transform.forward = direction;
        }

    }
}
