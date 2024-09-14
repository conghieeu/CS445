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

        private void Awake()
        { 
            _ctrl = GetComponent<PlayerCtrl>();
        }

        private void OnEnable()
        {
            _input.SenderItem += _ => SenderParcel();
            _input.SenderItem += _ => SenderItemSell();
        }

        private void OnDisable()
        {
            _input.SenderItem -= _ => SenderParcel();
            _input.SenderItem -= _ => SenderItemSell();
        }

        private void FixedUpdate()
        {
            if (_itemDrag._isDragging) TempAiming();
        }



        /// <summary> chạm vào kệ, người chơi có thể truyền item từ parcel sang table đó </summary>
        private void SenderItemSell()
        {
            // có item ở cảm biến
            Item shelf = _ctrl._sensorForward.GetItemTypeHit(Type.Shelf);
            Item itemHold = _itemDrag._itemDragging;

            if (shelf && itemHold && !itemHold._isCanSell) // gửi các apple từ bưu kiện sang kệ
            {
                shelf._itemSlot.ReceiverItems(itemHold._itemSlot, true);
            }
            else if (shelf && itemHold && itemHold._isCanSell) // để apple lênh kệ
            {
                In($"Player để quá táo lênh kệ");
                _itemDrag.OnDropItem();
                shelf._itemSlot.TryAddItemToItemSlot(itemHold, true);
            }
        }

        /// <summary> đưa parcel vào thùng rác </summary>
        private void SenderParcel()
        {
            Item trash = _ctrl._sensorForward.GetItemTypeHit(Type.Trash);
            Item parcel = _itemDrag._itemDragging;

            if (trash && parcel && parcel._type == Type.Parcel)
            {
                _itemDrag.OnDropItem();
                trash._itemSlot.TryAddItemToItemSlot(parcel, true);

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
