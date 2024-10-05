using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class ItemPooler : EntityPooler
    { 

        /// <summary> Tìm item có item Slot và còn chỗ trống </summary>
        public Item GetItemEmptySlot(Type type)
        {
            foreach (var o in ListEntity)
            {
                Item item = o.GetComponent<Item>();
                if (item && item.ItemSlot && item.Type == type && item.ItemSlot.IsHasSlotEmpty()) return item;
            }
            return null;
        }

        /// <summary> Tìm item có itemSlot có chứa itemProduct cần lấy </summary>
        public virtual Item GetItemContentItem(Item item)
        {
            foreach (var objectBool in ListEntity)
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
        public Item GetItemByTypeID(TypeID typeID)
        {
            List<Entity> entityList = ListEntity.ToList();

            foreach (var entity in entityList)
            {
                Item item = entity.GetComponent<Item>();

                if (item && item.EntityParent && entity.TypeID == typeID && item.EntityParent.Type == Type.Shelf && item.gameObject.activeSelf)
                {
                    return item;
                }
            }
            return null;
        }

        #region Save Data
        public override void SetVariables<T, V>(T data)
        {
            if (data is GamePlayData gamePlayData)
            {
                base.SetVariables<List<ItemData>, ItemData>(gamePlayData.ItemsData);
            }
        }

        public override void SaveData()
        {
            DataManager.Instance.GameData._gamePlayData.ItemsData = GetData<List<ItemData>, ItemData>();
        }
        #endregion
    }
}