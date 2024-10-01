
using System.Collections.Generic;
using UnityEngine;
using CuaHang.Pooler;
using System;

namespace CuaHang
{
    public class ItemSlot : GameBehavior
    {
        /// <summary> đại diện cho mỗi phần tử của danh sách slot </summary>
        [Serializable]
        public class ItemsSlot
        {
            public Item _item; // object đang gáng trong boolingObject đó
            public Transform _slot;


            // Constructor với tham số
            public ItemsSlot(Item item, Transform slot)
            {
                _item = item;
                _slot = slot;
            }
        }


        [Header("Item Slots")]
        public Item _item;
        public List<ItemsSlot> _itemsSlot;

        ItemPooler m_ItemPooler;

        public event Action<Item> ActionAddItem;

        void Awake()
        {
            _item = GetComponentInParent<Item>();
            LoadSlots();
        }

        private void LoadSlots()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                ItemsSlot newSlot = new ItemsSlot(null, transform.GetChild(i));
                if (_itemsSlot.Count < transform.childCount) _itemsSlot.Add(newSlot);
            }
        }

        /// <summary> Trong List item slot thì chỉnh tất cả các bool drag item </summary>
        public void SetDragItems(bool active)
        {
            foreach (var i in _itemsSlot)
            {
                if (i._item)
                {
                    i._item.SetDragState(active);
                }
            }
        }

        public bool IsContentItem(Item item)
        {
            foreach (var i in _itemsSlot) if (i._item == item) return true;
            return false;
        }

        /// <summary> Có slot nào đang trống không </summary>
        public bool IsHasSlotEmpty()
        {
            foreach (var slot in _itemsSlot)
            {
                if (slot._item == null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary> Có item nào có trong slot không </summary>
        public bool IsAnyItem()
        {
            foreach (var i in _itemsSlot) if (i._item != null) return true;
            return false;
        }

        /// <summary> Thêm 1 item vào danh sách </summary>
        public bool TryAddItemToItemSlot(Item item, bool isCanDrag)
        {
            foreach (var slot in _itemsSlot)
            {
                if (!slot._item)
                {
                    slot._item = item;
                    item.SetParent(slot._slot, GetComponentInParent<Item>(), isCanDrag);
                    ActionAddItem?.Invoke(item);
                    return true;
                }
            }
            return false;
        }

        /// <summary> Xoá item ra khỏi danh sách và ẩn item nó ở thế giới </summary>
        public bool RemoveItemInWorld(Item item)
        {
            for (int i = _itemsSlot.Count - 1; i >= 0; i--)
            {
                if (_itemsSlot[i]._item == item && _itemsSlot[i]._item != null)
                {
                    m_ItemPooler.RemoveEntityFromPool(item);
                    _itemsSlot[i]._item = null;
                    return true;
                }
            }
            return false;
        }

        public bool RemoveItemInList(Item item)
        {
            for (int i = _itemsSlot.Count - 1; i >= 0; i--)
            {
                if (_itemsSlot[i]._item == item && _itemsSlot[i]._item != null)
                {
                    _itemsSlot[i]._item = null;
                    return true;
                }
            }
            return false;
        }

        /// <summary> Lấy toàn bộ item từ sender đang có nạp vào _itemSlot này </summary>
        public virtual void ReceiverItems(ItemSlot sender, bool isCanDrag)
        {
            for (int i = 0; i < sender._itemsSlot.Count; i++)
            {
                if (sender._itemsSlot[i]._item && IsHasSlotEmpty())
                {
                    TryAddItemToItemSlot(sender._itemsSlot[i]._item, isCanDrag);
                    sender.RemoveItemInList(sender._itemsSlot[i]._item);
                }
            }
        }

    }
}