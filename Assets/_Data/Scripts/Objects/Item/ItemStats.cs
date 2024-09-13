using System.Collections.Generic;
using Core;
using CuaHang.Pooler;
using UnityEngine;

namespace CuaHang
{
    public class ItemStats : ObjectStats
    {
        [Header("ITEM STATS")]
        [SerializeField] ItemData _itemData;

        // Lay du lieu cua chinh cai nay de save
        public ItemData GetData()
        {
            Item item = GetComponent<Item>();

            string idItemParent = "";
            if (item._itemParent) idItemParent = item._itemParent._ID;

            _itemData = new ItemData(
                item._ID,
                idItemParent,
                item._typeID,
                item._price,
                item.transform.position,
                item.transform.rotation);

            return _itemData;
        }

        /// <summary> Item Pooler gọi để tải dử liệu </summary>
        public override void LoadData<T>(T data)
        {
            _itemData = data as ItemData;
            
            if (_itemData == null) return;

            GetComponent<Item>().SetProperties(_itemData);
        }

        protected override void SaveData() { }

        protected override void LoadNewGame() { }

        protected override void LoadNewData() { }
    }
}
