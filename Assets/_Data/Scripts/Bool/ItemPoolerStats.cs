using System.Collections.Generic;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class ItemPoolerStats : ObjectStats
    {
        [Header("ITEM POOLER STATS")]
        [SerializeField] List<ItemData> _itemsData;

        public override void OnSetData<T>(T data)
        {
            if (!(data is GamePlayData)) return;

            _itemsData = (data as GamePlayData).ItemsData;

            GetComponent<ItemPooler>().RecreateItemsData(_itemsData);
        }

        public override void OnLoadData()
        {
            
        }

        /// <summary> bắt tính hiệu save </summary>
        protected override void SaveData()
        {
            GetGameData()._gamePlayData.ItemsData = GetItemsData();
        }

        /// <summary> Tìm và lọc item từ root data </summary>
        private List<ItemData> GetItemsData()
        {
            List<ItemData> itemsData = new List<ItemData>();

            foreach (var pool in GetComponent<ItemPooler>()._ObjectPools)
            {
                if (pool && pool.ID != "" && pool.gameObject.activeInHierarchy)
                {
                    itemsData.Add(pool.GetComponent<ItemStats>().GetData());
                }
            }
            return itemsData;
        }
    }
}