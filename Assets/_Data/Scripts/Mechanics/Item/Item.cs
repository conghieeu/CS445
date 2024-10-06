using UnityEngine;
using CuaHang.Pooler;
using System.Collections.Generic;
using CuaHang.AI;
using Mono.CSharp;

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
        [SerializeField] Entity _entityParent; // item đang giữ item này 
        [SerializeField] Transform _waitingPoint;
        [SerializeField] Transform _models;

        bool m_IsDestroy;
        CamHere m_CamHere;
        BoxCollider m_BoxCollider;
        ItemPooler m_ItemPooler;
        StaffPooler m_StaffPooler;
        PoolManager m_PoolManager;
        PlayerCtrl m_PlayerCtrl;
        public ItemSlot ItemSlot { get; private set; }

        public BoxCollider Coll { get => m_BoxCollider; }
        public CamHere CamHere { get => m_CamHere; }
        public ItemSO SO { get => _SO; }
        public float Price { get => _price; set => _price = value; }
        public bool IsCanDrag { get => _isCanDrag; set => _isCanDrag = value; }
        public bool IsCanSell { get => _isCanSell; set => _isCanSell = value; }
        public bool IsSamePrice { get => _isSamePrice; set => _isSamePrice = value; }
        public Transform ThisParent { get => _thisParent; set => _thisParent = value; }
        public Entity EntityParent { get => _entityParent; set => _entityParent = value; }
        public Transform WaitingPoint { get => _waitingPoint; set => _waitingPoint = value; }
        public Transform Models { get => _models; set => _models = value; }
        public string IdParent { get => _idParent; set => _idParent = value; }

        protected virtual void Awake()
        {
            Init();
        }

        private void Init()
        {
            m_PlayerCtrl = FindFirstObjectByType<PlayerCtrl>();
            m_PoolManager = FindFirstObjectByType<PoolManager>();
            m_StaffPooler = FindFirstObjectByType<StaffPooler>();
            m_ItemPooler = FindFirstObjectByType<ItemPooler>();
            m_BoxCollider = GetComponent<BoxCollider>();
            m_CamHere = GetComponentInChildren<CamHere>();
            ItemSlot = GetComponentInChildren<ItemSlot>();

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

        public override void PickUpEntity(Entity entity)
        {
            ItemSlot.TryAddItemToItemSlot(entity.GetComponent<Item>());
        }

        public void SetParent(Transform setParent, Entity entityParent, bool isCanDrag)
        {
            if (setParent)
            {
                transform.SetParent(setParent);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
            else
            {
                transform.SetParent(m_ItemPooler.transform);
            }

            _thisParent = setParent;
            _entityParent = entityParent;
            _isCanDrag = isCanDrag;
        }

        /// <summary> Tạo item trong itemSlot </summary>
        public void CreateItemInSlot(List<ItemSO> items)
        {
            for (int i = 0; i < ItemSlot._itemsSlot.Count && i < items.Count; i++)
            {
                if (items[i])
                {
                    Item item = m_ItemPooler.GetOrCreateObjectPool(items[i]._typeID, Vector3.zero).GetComponent<Item>();

                    if (ItemSlot.TryAddItemToItemSlot(item, false) && IsSamePrice)
                    {
                        item.Price = Price;
                    }
                }
            }
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
                m_IsDestroy = itemData.IsDestroyed;
            }
        }

        public override void LoadVariables()
        {
            if(m_IsDestroy) 
            {
                RemoveThis();
                return;
            }

            Init();

            // Entity entityParent = m_PoolManager.FindEntityById(IdParent);

            // if (entityParent)
            // {
            //     entityParent.PickUpEntity(this);
            // }

            // tìm item cha và tự chui vào đó
            if (m_PlayerCtrl.ID == IdParent)
            {
                m_PlayerCtrl.PickUpEntity(this);
            }
            else if (m_ItemPooler.GetObjectByID(IdParent))
            {
                m_ItemPooler.GetObjectByID(IdParent).PickUpEntity(this);
            }
            else if (m_StaffPooler.GetObjectByID(IdParent))// trường hợp cha là nhân viên
            {
                m_StaffPooler.GetObjectByID(IdParent).PickUpEntity(this);
            }
        }

        public override T GetData<T, D>()
        {
            if (ID != "")
            {
                ItemData data = new ItemData(GetEntityData(), EntityParent ? EntityParent.ID : "", Price);
                return (T)(object)data;
            }
            else
            {
                return (T)(object)null;
            }
        }
        #endregion
    }
}