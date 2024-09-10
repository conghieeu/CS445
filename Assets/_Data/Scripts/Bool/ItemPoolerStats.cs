using System.Collections.Generic;
using Core;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class ItemPoolerStats : ObjectStats
    {
        [Header("ITEM POOLER STATS")]
        [SerializeField] ItemPooler _itemPooler;

        protected override void Start()
        {
            base.Start();
            _itemPooler = GetComponent<ItemPooler>();
        }

        /// <summary> tạo các root đầu tiên </summary>
        public override void LoadData<T>(T data)
        {
            _itemPooler = GetComponent<ItemPooler>();
            List<ItemData> itemsData = (data as GameData)._itemsData;

            // tái tạo items data
            foreach (var itemData in itemsData)
            {
                ObjectPool objectPool = _itemPooler.GetObjectByID(itemData._id);
                // load data những đối tượng đã tồn tại
                if (objectPool)
                {
                    objectPool.GetComponent<ItemStats>().LoadData(itemData);
                }
                else
                {
                    ObjectPool item = _itemPooler.GetOrCreateObjectPool(itemData._typeID);
                    item.GetComponent<ItemStats>().LoadData(itemData);
                }
            }
        }

        /// <summary> bắt tính hiệu save </summary>
        protected override void SaveData()
        {
            GetGameData()._itemsData = GetItemsDataRoot();
        }

        /// <summary> Tìm và lọc item từ root data </summary>
        public List<ItemData> GetItemsDataRoot()
        {
            List<ItemData> itemsData = new List<ItemData>();

            foreach (var pool in _itemPooler._ObjectPools)
            {
                if (pool && pool._ID != "" && !pool.GetComponent<Item>()._thisParent && pool.gameObject.activeInHierarchy) // kiểm tra có phải là root
                {
                    itemsData.Add(pool.GetComponent<ItemStats>().GetData());
                }
            }

            return itemsData;
        }

        protected override void LoadNoData()
        {
            // throw new System.NotImplementedException();
        }
    }
}