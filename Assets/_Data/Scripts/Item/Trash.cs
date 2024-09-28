using System;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    [Serializable]
    public class ParcelTrash
    {
        public float _time = 0;
        public Item _item;
    }

    public class Trash : Item
    {
        [Header("TRASH")]
        public float _timeDelete; // thời gian để xoá đi đối tượng bênh trong kho
        public List<ParcelTrash> _listTrash; // thời gian để xoá đi đối tượng bênh trong kho

        private void OnEnable()
        {
            ItemSlot.ActionAddItem += AddItemToTrash;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ItemSlot.ActionAddItem -= AddItemToTrash;
        }

        private void FixedUpdate()
        {
            CountDownRemove();
        }

        /// <summary> Thêm item rác vào thùng rác </summary>
        public void AddItemToTrash(Item item)
        {
            ParcelTrash trash = new ParcelTrash();
            trash._time = _timeDelete;
            trash._item = item;
            _listTrash.Add(trash);
        }

        /// <summary> thùng rác đếm ngược về 0 sẽ xoá item </summary>
        private void CountDownRemove()
        {
            for (int i = 0; i < _listTrash.Count; i++)
            {
                // đếm ngược
                if (_listTrash[i]._time > 0f)
                {
                    _listTrash[i]._time -= Time.fixedDeltaTime;
                }

                Item item = _listTrash[i]._item;

                // xoá item
                if (_listTrash[i]._time <= 0f && item)
                {
                    item.SetParent(null, null, false);
                    ItemSlot.RemoveItemInWorld(_listTrash[i]._item);
                    _listTrash[i]._item = null;
                }
            }
        }
    }
}