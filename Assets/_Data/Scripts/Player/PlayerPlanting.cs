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
        InputImprove _input; 
        ItemDrag _itemDrag;

        private void Awake()
        { 
            _input = InputImprove.Instance;
            _ctrl = GetComponent<PlayerCtrl>();
            _itemDrag = RaycastCursor.Instance._ItemDrag;
        }

        private void OnEnable()
        {
            _input.Sender += SenderParcel;
            _input.Sender += SenderItemSell;

        }

        private void FixedUpdate()
        {
            if (_itemDrag._isDragging) TempAiming();
        }



        /// <summary> chạm vào kệ, người chơi có thể truyền item từ parcel sang table đó </summary>
        private void SenderItemSell(InputAction.CallbackContext context)
        {
            // có item ở cảm biến
            Item shelf = _ctrl._sensorForward.GetItemTypeHit(Type.Shelf);
            Item itemHold = _itemDrag._itemDragging;

            if (shelf && itemHold && !itemHold._isCanSell) // gửi các apple từ bưu kiện sang kệ
            {
                shelf._itemSlot.ReceiverItems(itemHold._itemSlot, false);
            }
            else if (shelf && itemHold && itemHold._isCanSell) // để apple lênh kệ
            {
                In($"Player để quá táo lênh kệ");
                _itemDrag.OnDropItem();
                shelf._itemSlot.TryAddItemToItemSlot(itemHold, false);
            }
        }

        /// <summary> đưa parcel vào thùng rác </summary>
        private void SenderParcel(InputAction.CallbackContext context)
        {
            Item trash = _ctrl._sensorForward.GetItemTypeHit(Type.Storage);
            Item parcel = _itemDrag._itemDragging;

            if (trash && parcel)
            {
                if (parcel._type == Type.Parcel)
                {
                    In($"Player thêm item {parcel} vào trash  {trash}");
                    _itemDrag.OnDropItem();
                    trash._itemSlot.TryAddItemToItemSlot(parcel, true);
                }
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
