using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class ItemPooler : ObjectPooler
    {
        [SerializeField] Transform _itemSpawnerPoint;

        public static ItemPooler Instance;

        public Transform ItemSpawnerPoint { get => _itemSpawnerPoint; }

        protected override void Awake()
        {
            base.Awake();

            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public void SetProperties(List<ItemData> itemsData)
        {
            List<ItemData> items = new List<ItemData>();

            foreach (var itemData in itemsData)
            {
                // tải những dữ liệu cho đối tượng có sẵn
                ObjectPool existItemID = GetObjectByID(itemData.Id);
                ObjectPool existParentItemID = GetObjectByID(itemData.IdItemParent);

                if (itemData.IdItemParent == "" || existParentItemID)
                {
                    if (existItemID)
                    {
                        existItemID.GetComponent<ItemStats>().LoadData(itemData);
                    }
                    else
                    {
                        ObjectPool item = GetOrCreateObjectPool(itemData.TypeID);
                        item.GetComponent<ItemStats>().LoadData(itemData);
                    }
                }
                else
                {
                    items.Add(itemData);
                }
            }
            if (items.Count > 0) SetProperties(items);
        }

        /// <summary> Tìm item có item Slot và còn chỗ trống </summary>
        public Item GetItemEmptySlot(TypeID typeID)
        {
            foreach (var o in _ObjectPools)
            {
                Item item = o.GetComponent<Item>();
                if (item && item._itemSlot && item._typeID == typeID && item._itemSlot.IsHasSlotEmpty()) return item;
            }
            return null;
        }

        /// <summary> Tìm item có itemSlot có chứa itemProduct cần lấy </summary>
        public virtual Item GetItemContentItem(Item item)
        {
            foreach (var objectBool in _ObjectPools)
            {
                Item i = objectBool.GetComponent<Item>();

                if (i && i._itemSlot && i._itemSlot.IsContentItem(item))
                {
                    return i;
                }
            }
            return null;
        }

        /// <summary> Tìm item lần lượt theo mục đang muốn mua </summary>
        public Item ShuffleFindItem(TypeID typeID)
        {
            List<ObjectPool> poolsO = _ObjectPools.ToList();
            poolsO.Shuffle<ObjectPool>();

            foreach (var o in poolsO)
            {
                Item item = o.GetComponent<Item>();

                if (item && item._itemParent && o._typeID == typeID && item._itemParent._type == Type.Shelf && item.gameObject.activeSelf) return item;
            }

            return null;
        }
    }
}