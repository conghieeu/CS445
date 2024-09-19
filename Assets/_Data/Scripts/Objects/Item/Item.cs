using UnityEngine;
using CuaHang.Pooler;
using System.Collections.Generic;
using TMPro;
using System;
using CuaHang.AI;

namespace CuaHang
{
    /// <summary> Là item, có khả năng drag </summary>
    public class Item : ObjectPool
    {
        [Header("ITEM")]
        [Header("Attributes")]
        [SerializeField] ItemSO _SO; // SO chỉ được load một lần
        [SerializeField] float _price;

        [Header("Variables")]
        [SerializeField] bool _isCanDrag = true;  // có thằng nhân vật nào đó đang bưng bê cái này
        [SerializeField] bool _isCanSell; // có thể bán được không 
        [SerializeField] bool _isBlockPrice;
        [SerializeField] bool _isSamePrice; // muốn đặt giá tiền các item trong kệ sẽ ngan item cha không
        [SerializeField] Transform _thisParent; // là cha của item này
        [SerializeField] ItemSlot _itemSlot; // Có cái này sẽ là item có khả năng lưu trử các item khác
        [SerializeField] ObjectPool _objectParent; // item đang giữ item này
        [SerializeField] ItemStats _itemStats;
        [SerializeField] Transform _waitingPoint;
        [SerializeField] Transform _models;

        CamHere _camHere;
        BoxCollider _coll;
        StaffPooler _staffPooler => StaffPooler.Instance;
        ItemPooler _itemPooler => ItemPooler.Instance;

        public BoxCollider Coll { get => _coll; }
        public CamHere CamHere { get => _camHere; }
        public ItemSO SO { get => _SO; }
        public float Price { get => _price; set => _price = value; }
        public bool IsCanDrag { get => _isCanDrag; set => _isCanDrag = value; }
        public bool IsCanSell { get => _isCanSell; set => _isCanSell = value; }
        public bool IsSamePrice { get => _isSamePrice; set => _isSamePrice = value; }
        public Transform ThisParent { get => _thisParent; set => _thisParent = value; }
        public ItemSlot ItemSlot { get => _itemSlot; set => _itemSlot = value; }
        public ObjectPool ObjectParent { get => _objectParent; set => _objectParent = value; }
        public ItemStats ItemStats { get => _itemStats; set => _itemStats = value; }
        public Transform WaitingPoint { get => _waitingPoint; set => _waitingPoint = value; }
        public Transform Models { get => _models; set => _models = value; }

        protected virtual void Awake()
        {
            _coll = GetComponent<BoxCollider>();
            _itemSlot = GetComponentInChildren<ItemSlot>();
            _camHere = GetComponentInChildren<CamHere>();
            _itemStats = GetComponentInChildren<ItemStats>();

            // Set properties
            _name = SO._name;
            _typeID = SO._typeID;
            _type = SO._type;
            _price = SO._priceDefault;
            _isCanSell = SO._isCanSell;
            _isBlockPrice = SO._isBlockPrice;
        }

        void OnDisable()
        {
            _isCanDrag = true;
            _isRecyclable = false;
            _thisParent = null;
        }

        public void SetParent(Transform thisParent, ObjectPool objectPoolParent, bool isCanDrag)
        {
            if (thisParent)
            {
                transform.SetParent(thisParent);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
            else
            {
                transform.SetParent(ItemPooler.Instance.transform);
            }

            _thisParent = thisParent;
            _objectParent = objectPoolParent;
            _isCanDrag = isCanDrag;
        }

        /// <summary> Set Properties with Item Data </summary>
        public virtual void SetVariables(ItemData data)
        {
            if (data == null) return;

            ID = data.Id;
            Price = data.Price;
            TypeID = data.TypeID;
            transform.position = data.Position;
            transform.rotation = data.Rotation;

            // tìm item cha và tự chui vào đó
            if (_itemPooler.GetObjectByID(data.IdItemParent))
            {
                _itemPooler.GetObjectByID(data.IdItemParent).GetComponentInChildren<ItemSlot>().TryAddItemToItemSlot(this, true);
            }
            else if (_staffPooler.GetObjectByID(data.IdItemParent))// trường hợp cha là nhân viên
            { 
                _staffPooler.GetObjectByID(data.IdItemParent).GetComponent<Staff>().SetHeldItem(this); 
            }
        }

        /// <summary> Tạo item trong item slot với itemsData </summary>
        public void CreateItemInSlot(List<ItemData> itemsData)
        {
            ItemSlot itemSlot = GetComponentInChildren<ItemSlot>();

            // tái tạo items data
            foreach (var itemData in itemsData)
            {
                // tạo
                ObjectPool item = ItemPooler.Instance.GetOrCreateObjectPool(itemData.TypeID);

                item.GetComponent<ItemStats>().OnSetData(itemData);
                if (itemSlot)
                {
                    itemSlot.TryAddItemToItemSlot(item.GetComponent<Item>(), true);
                }
            }
        }

        /// <summary> Tạo item trong itemSlot </summary>
        public void CreateItemInSlot(List<ItemSO> items)
        {
            for (int i = 0; i < ItemSlot._itemsSlots.Count && i < items.Count; i++)
            {
                if (items[i])
                {
                    Item item = ItemPooler.Instance.GetOrCreateObjectPool(items[i]._typeID).GetComponent<Item>();

                    if (ItemSlot.TryAddItemToItemSlot(item, false) && IsSamePrice)
                    {
                        item.Price = Price;
                    }
                }
            }
        }

        public void SetRandomPos()
        {
            float size = 2f;
            float rx = UnityEngine.Random.Range(-size, size);
            float rz = UnityEngine.Random.Range(-size, size);

            Vector3 p = ItemPooler.Instance.ItemSpawnerPoint.position;

            transform.position = new Vector3(p.x + rx, p.y, p.z + rz);
        }

        public void SetPrice(float price)
        {
            if (_isBlockPrice) return;

            if (!SO)
            {
                Debug.LogWarning("Lỗi item này không có ScriptableObject", transform);
                return;
            }

            float newPrice = Price + price;

            if (newPrice > SO._priceMarketMax) Debug.Log("Cảnh báo bạn đang bị ảo giá");
            if (newPrice < SO._priceMarketMin) newPrice = SO._priceMarketMin;
            Price = newPrice;
        }

        public virtual void SetDragState(bool active)
        {
            Coll.enabled = !active;
            _isCanDrag = !active;

            if (ObjectParent && ObjectParent.GetComponentInChildren<ItemSlot>())
            {
                ObjectParent.GetComponentInChildren<ItemSlot>().RemoveItemInList(this);
            }
        }

        public virtual void DropItem(Transform location)
        {
            Coll.enabled = true;
            SetParent(null, null, true);
            if (location)
            {
                transform.position = location.position;
                transform.rotation = location.rotation;
            }
        }

    }
}