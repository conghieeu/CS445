using UnityEngine;
using CuaHang.Pooler;
using System.Collections.Generic;
using CuaHang.AI;

namespace CuaHang
{
    /// <summary> Là item, có khả năng drag </summary>
    public class Item : Entity
    {
        [Header("ITEM")]
        [Header("Attributes")]
        [SerializeField] ItemSO _SO; // SO chỉ được load một lần
        [SerializeField] float _price;
        [SerializeField] string _idParent;

        [Header("Variables")]
        [SerializeField] bool _isCanDrag = true;  // có thằng nhân vật nào đó đang bưng bê cái này
        [SerializeField] bool _isCanSell; // có thể bán được không 
        [SerializeField] bool _isBlockPrice;
        [SerializeField] bool _isSamePrice; // muốn đặt giá tiền các item trong kệ sẽ ngan item cha không
        [SerializeField] Transform _thisParent; // là cha của item này
        [SerializeField] ItemSlot _itemSlot; // Có cái này sẽ là item có khả năng lưu trử các item khác
        [SerializeField] Entity _entityParent; // item đang giữ item này 
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
        public Entity EntityParent { get => _entityParent; set => _entityParent = value; }
        public Transform WaitingPoint { get => _waitingPoint; set => _waitingPoint = value; }
        public Transform Models { get => _models; set => _models = value; }
        public string IdParent { get => _idParent; set => _idParent = value; }

        protected virtual void Awake()
        {
            _coll = GetComponent<BoxCollider>();
            _itemSlot = GetComponentInChildren<ItemSlot>();
            _camHere = GetComponentInChildren<CamHere>();

            // Set properties
            _name = SO._name;
            _typeID = SO._typeID;
            _type = SO._type;
            _price = SO._priceDefault;
            _isCanSell = SO._isCanSell;
            _isBlockPrice = SO._isBlockPrice;
        }

        protected virtual void OnDisable()
        {
            _isCanDrag = true;
            _isRecyclable = false;
            _thisParent = null;
        }

        public void SetParent(Transform thisParent, Entity objectPoolParent, bool isCanDrag)
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
            _entityParent = objectPoolParent;
            _isCanDrag = isCanDrag;
        }

        /// <summary> Tạo item trong itemSlot </summary>
        public void CreateItemInSlot(List<ItemSO> items)
        {
            for (int i = 0; i < ItemSlot._itemsSlot.Count && i < items.Count; i++)
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

            if (EntityParent && EntityParent.GetComponentInChildren<ItemSlot>())
            {
                EntityParent.GetComponentInChildren<ItemSlot>().RemoveItemInList(this);
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

        #region SaveData
        /// <summary> Set Properties with Item Data </summary>
        public override void SetVariables<T, V>(T data)
        {
            if (data is ItemData itemData)
            {
                base.SetVariables<T, V>(data);
                Price = itemData.Price;
                IdParent = itemData.IdItemParent;
            }
        }

        public override void LoadVariables()
        { 
            // tìm item cha và tự chui vào đó
            if (_itemPooler.GetObjectByID(IdParent))
            {
                _itemPooler.GetObjectByID(IdParent).GetComponentInChildren<ItemSlot>().TryAddItemToItemSlot(this, true);
            }
            else if (_staffPooler.GetObjectByID(IdParent))// trường hợp cha là nhân viên
            {
                _staffPooler.GetObjectByID(IdParent).GetComponent<Staff>().SetHeldItem(this);
            }
        }

        public override T GetData<T, D>()
        {
            ItemData data = new ItemData(GetEntityData(), EntityParent ? EntityParent.ID : "", Price);
            return (T)(object)(data);
        }
        #endregion
    }
}