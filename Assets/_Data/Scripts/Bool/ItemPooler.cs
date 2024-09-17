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

        /// <summary> Tạo lại đối tượng ItemPool từ ItemsData </summary>
        public void RecreateItemsData(List<ItemData> itemsData)
        {
            foreach (var itemData in itemsData)
            {
                ObjectPool existID = GetObjectByID(itemData.Id);

                if (existID) // object bool da co san
                {
                    existID.GetComponent<ItemStats>().ItemData = itemData;
                }
                else // tao lai object bool
                {
                    ObjectPool item = GetOrCreateObjectPool(itemData.TypeID);
                    item.GetComponent<ItemStats>().ItemData = itemData;
                }
            }
        }

        /// <summary> Tìm item có item Slot và còn chỗ trống </summary>
        public Item GetItemEmptySlot(TypeID typeID)
        {
            foreach (var o in _ObjectPools)
            {
                Item item = o.GetComponent<Item>();
                if (item && item.ItemSlot && item.TypeID == typeID && item.ItemSlot.IsHasSlotEmpty()) return item;
            }
            return null;
        }

        /// <summary> Tìm item có itemSlot có chứa itemProduct cần lấy </summary>
        public virtual Item GetItemContentItem(Item item)
        {
            foreach (var objectBool in _ObjectPools)
            {
                Item i = objectBool.GetComponent<Item>();

                if (i && i.ItemSlot && i.ItemSlot.IsContentItem(item))
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

                if (item && item.ObjectParent && o.TypeID == typeID && item.ObjectParent.Type == Type.Shelf && item.gameObject.activeSelf) return item;
            }

            return null;
        }
    }
}