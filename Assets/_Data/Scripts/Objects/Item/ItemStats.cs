using UnityEngine;

namespace CuaHang
{
    public class ItemStats : ObjectStats
    {
        [Header("ITEM STATS")]
        [SerializeField] ItemData _itemData;

        public ItemData ItemData { get => _itemData; set => _itemData = value; }

        /// <summary> Pooler lấy để save list item </summary>
        public ItemData GetData()
        {
            Item item = GetComponent<Item>();

            string idItemParent = "";
            if (item.ObjectParent) idItemParent = item.ObjectParent.ID;

            ItemData = new ItemData(
                item.ID,
                idItemParent,
                item.TypeID,
                item.Price,
                item.transform.position,
                item.transform.rotation);

            return ItemData;
        }

        /// <summary> Item Pooler gọi để tải và load dử liệu </summary>
        public override void OnSetData<T>(T data)
        {
            if (data is ItemData)
            {
                ItemData = data as ItemData;
            }
        }

        public override void OnLoadData()
        {
            if (GetGameData()._gamePlayData.IsInitialized)
            {
                GetComponent<Item>().SetVariables(ItemData);
            }
        }

        protected override void SaveData() { }
    }
}
