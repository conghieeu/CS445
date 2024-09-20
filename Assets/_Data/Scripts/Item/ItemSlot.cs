using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CuaHang.Pooler;
using System;
using System.Xml.Schema;
using TMPro;

namespace CuaHang
{
    public class ItemSlot : MonoBehaviour
    {
        /// <summary> đại diện cho mỗi phần tử của danh sách slot </summary>
        [Serializable]
        public class ItemsSlots
        {
            public Item _item; // object đang gáng trong boolingObject đó
            public Transform _slot;


            // Constructor với tham số
            public ItemsSlots(Item item, Transform slot)
            {
                _item = item;
                _slot = slot;
            }
        }


        [Header("Item Slots")]
        public Item _item;
        public List<ItemsSlots> _itemsSlots;

        void Awake()
        {
            _item = GetComponentInParent<Item>();
            LoadSlots();
        }

        private void LoadSlots()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                ItemsSlots newSlot = new ItemsSlots(null, transform.GetChild(i));
                if (_itemsSlots.Count < transform.childCount) _itemsSlots.Add(newSlot);
            }
        }

        /// <summary> Trong List item slot thì chỉnh tất cả các bool drag item </summary>
        public void SetDragItems(bool active)
        {
            foreach (var i in _itemsSlots)
            {
                if (i._item)
                {
                    i._item.SetDragState(active);
                }
            }
        }

        public bool IsContentItem(Item item)
        {
            foreach (var i in _itemsSlots) if (i._item == item) return true;
            return false;
        }

        /// <summary> Có slot nào đang trống không </summary>
        public bool IsHasSlotEmpty()
        {
            foreach (var slot in _itemsSlots)
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
            foreach (var i in _itemsSlots) if (i._item != null) return true;
            return false;
        }

        /// <summary> Thêm 1 item vào danh sách </summary>
        public bool TryAddItemToItemSlot(Item item, bool isCanDrag)
        {
            foreach (var i in _itemsSlots)
            {
                if (i._item == null)
                {
                    i._item = item;
                    item.SetParent(i._slot, GetComponentInParent<Item>(), isCanDrag);
                    return true;
                }
            }
            return false;
        }

        /// <summary> Xoá item ra khỏi danh sách và ẩn item nó ở thế giới </summary>
        public bool RemoveItemInWorld(Item item)
        {
            for (int i = _itemsSlots.Count - 1; i >= 0; i--)
            {
                if (_itemsSlots[i]._item == item && _itemsSlots[i]._item != null)
                {
                    ItemPooler.Instance.RemoveObjectFromPool(item);
                    _itemsSlots[i]._item = null;
                    return true;
                }
            }
            return false;
        }

        public bool RemoveItemInList(Item item)
        {
            for (int i = _itemsSlots.Count - 1; i >= 0; i--)
            {
                if (_itemsSlots[i]._item == item && _itemsSlots[i]._item != null)
                {
                    _itemsSlots[i]._item = null;
                    return true;
                }
            }
            return false;
        }

        /// <summary> Lấy toàn bộ item từ sender đang có nạp vào _itemSlot này </summary>
        public virtual void ReceiverItems(ItemSlot sender, bool isCanDrag)
        {
            for (int i = 0; i < sender._itemsSlots.Count; i++)
            {
                if (sender._itemsSlots[i]._item && IsHasSlotEmpty())
                {
                    TryAddItemToItemSlot(sender._itemsSlots[i]._item, isCanDrag);
                    sender.RemoveItemInList(sender._itemsSlots[i]._item);
                }
            }
        }

    }
}