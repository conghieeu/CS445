using System.Collections.Generic;
using Core;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class ItemPoolerStats : ObjectStats
    {
        [Header("ITEM POOLER STATS")]
        [SerializeField] List<ItemData> _itemsData;

        /// <summary> tạo các root đầu tiên </summary>
        public override void LoadData<T>(T data)
        {
            _itemsData = (data as GameData)._gamePlayData.ItemsData;

            GetComponent<ItemPooler>().SetProperties(_itemsData);
        }

        /// <summary> bắt tính hiệu save </summary>
        protected override void SaveData()
        {
            GetGameData()._gamePlayData.ItemsData = GetItemsData();
        }

        protected override void LoadNewGame()
        {
            SaveData();
        }

        protected override void LoadNewData()
        {
            SaveData();
        }

        /// <summary> Tìm và lọc item từ root data </summary>
        private List<ItemData> GetItemsData()
        {
            List<ItemData> itemsData = new List<ItemData>();

            foreach (var pool in GetComponent<ItemPooler>()._ObjectPools)
            {
                if (pool && pool._ID != "" && pool.gameObject.activeInHierarchy)
                {
                    itemsData.Add(pool.GetComponent<ItemStats>().GetData());
                }
            }

            return itemsData;
        }
    }
}