using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace CuaHang
{
    public class PlayerPlanting : HieuBehavior
    {
        PlayerCtrl _ctrl;
        InputImprove _input => InputImprove.Instance;
        ItemDrag _itemDrag => RaycastCursor.Instance.ItemDrag;

        public static event Action ActionSenderItem;

        private void Awake()
        {
            _ctrl = GetComponent<PlayerCtrl>();
        }

        private void OnEnable()
        {
            _input.SenderItem += SenderParcel;
            _input.SenderItem += SenderItemSell;
        }

        private void OnDisable()
        {
            _input.SenderItem -= SenderParcel;
            _input.SenderItem -= SenderItemSell;
        }

        private void FixedUpdate()
        {
            if (_itemDrag._isDragging) TempAiming();
        }

        /// <summary> chạm vào kệ, người chơi có thể truyền item từ parcel sang table đó </summary>
        private void SenderItemSell(InputAction.CallbackContext ctx)
        {
            // có item ở cảm biến
            Item shelf = _ctrl._sensorForward.GetItemTypeHit(Type.Shelf);
            Item itemHold = _itemDrag._itemDragging;

            if (shelf && itemHold && !itemHold.IsCanSell) // gửi các apple trong parcel sang kệ
            {
                ActionSenderItem?.Invoke();
                shelf.ItemSlot.ReceiverItems(itemHold.ItemSlot, true);
            }
            else if (shelf && itemHold && itemHold.IsCanSell) // để item lênh kệ
            {
                In($"Player để quá táo lênh kệ");
                ActionSenderItem?.Invoke();
                _itemDrag.OnDropItem();
                shelf.ItemSlot.TryAddItemToItemSlot(itemHold, true);
            }
        }

        /// <summary> đưa parcel vào thùng rác </summary>
        private void SenderParcel(InputAction.CallbackContext ctx)
        {
            Item trash = _ctrl._sensorForward.GetItemTypeHit(Type.Trash);
            Item parcel = _itemDrag._itemDragging;

            if (trash && parcel && parcel.Type == Type.Parcel)
            {
                ActionSenderItem?.Invoke();
                _itemDrag.OnDropItem();
                trash.ItemSlot.TryAddItemToItemSlot(parcel, true);

            }
        }

        /// <summary> Khi mà drag object Temp thì player sẽ hướng về object Temp </summary>
        private void TempAiming()
        {
            var direction = _itemDrag.transform.position - transform.position;
            direction.y = 0;
            transform.forward = direction;
        }



    }
}
