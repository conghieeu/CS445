using UnityEngine;

namespace CuaHang
{
    public class ItemStats : ObjectStats
    {
        [Header("ITEM STATS")]
        [SerializeField] ItemData _itemData;

        public ItemData ItemData { get => _itemData; set => _itemData = value; }

        // Lay du lieu cua chinh cai nay de save
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

        public void LoadData()
        {
            GetComponent<Item>().SetVariables(ItemData);
        }

        /// <summary> Item Pooler gọi để tải và load dử liệu </summary>
        public override void OnSetData<T>(T data)
        {
            ItemData = data as ItemData;
            LoadData();
        }

        public override void OnLoadData() { }
        protected override void LoadNewData() { }
        protected override void LoadNewGame() { }
        protected override void SaveData() { }
    }
}
