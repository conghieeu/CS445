using System;
using Mono.CSharp;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace CuaHang
{
    public class PlayerPlanting : GameBehavior
    {
        [Header("PLAYER PLANTING")]
        [Header("Input Action")]

        PlayerCtrl m_PlayerCtrl;
        ModuleDragItem m_ModuleDragItem;
        InputImprove m_InputImprove;

        public UnityAction ActionSenderItem;

        private void Awake()
        {
            m_PlayerCtrl = GetComponent<PlayerCtrl>();
            m_ModuleDragItem = FindFirstObjectByType<ModuleDragItem>();

            m_InputImprove = FindAnyObjectByType<InputImprove>();
            m_InputImprove.SendItem.action.performed += ctx => SendItem();
        }

        private void FixedUpdate()
        {
            if (m_ModuleDragItem.IsDragging) TempAiming();
        }

        /// <summary> chạm vào kệ, người chơi có thể truyền item từ parcel sang table đó </summary>
        private void SendItem()
        {
            if (this == null) return;

            // có item ở cảm biến
            Item shelf = m_PlayerCtrl._sensorForward.GetItemTypeHit(Type.Shelf);
            Item trash = m_PlayerCtrl._sensorForward.GetItemTypeHit(Type.Trash);
            Item storage = m_PlayerCtrl._sensorForward.GetItemTypeHit(Type.Storage);
            Item itemHold = m_ModuleDragItem.ItemDragging;

            if (!itemHold) return;

            if (shelf && !itemHold.IsCanSell) // gửi các item trong parcel sang kệ
            {
                shelf.ItemSlot.ReceiverItems(itemHold.ItemSlot, true);
            }
            else if (shelf && itemHold.IsCanSell && shelf.ItemSlot.IsHasSlotEmpty()) // để item lênh kệ
            {
                if (shelf.ItemSlot.IsHasSlotEmpty())
                {
                    m_ModuleDragItem.OnDropItem();
                    shelf.ItemSlot.TryAddItemToItemSlot(itemHold, true);
                    ActionSenderItem?.Invoke();
                    m_PlayerCtrl.IsDragItem = false;
                }
            }
            else if (trash && itemHold.Type == Type.Parcel && trash.ItemSlot.IsHasSlotEmpty()) // de parcel vao thung rac
            {
                m_ModuleDragItem.OnDropItem();
                trash.ItemSlot.TryAddItemToItemSlot(itemHold, true);
                m_PlayerCtrl.IsDragItem = false;
                ActionSenderItem?.Invoke();
            }
            else if (storage && itemHold.Type == Type.Parcel && storage.ItemSlot.IsHasSlotEmpty()) // de parcel vao kho
            {
                m_ModuleDragItem.OnDropItem();
                storage.ItemSlot.TryAddItemToItemSlot(itemHold, true);
                ActionSenderItem?.Invoke();
                m_PlayerCtrl.IsDragItem = false;
            }
        }

        /// <summary> Khi mà drag object Temp thì player sẽ hướng về object Temp </summary>
        private void TempAiming()
        {
            var direction = m_ModuleDragItem.transform.position - transform.position;
            direction.y = 0;
            transform.forward = direction;
        }

    }
}
