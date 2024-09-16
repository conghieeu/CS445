using System.Collections.Generic;
using UnityEngine;

namespace CuaHang.Pooler
{
    public enum TypePool
    {
        Item,
        Customer,
        Staff,
    }

    public class ObjectPooler : MonoBehaviour
    {
        [Header("BoolingObjects")]
        public TypePool _poolType;
        [SerializeField] protected List<Transform> _prefabs;
        [SerializeField] private List<ObjectPool> _objectPools;

        public List<ObjectPool> _ObjectPools { get => _objectPools; private set => _objectPools = value; }

        protected virtual void Awake()
        {
            // load childen item
            foreach (Transform child in transform)
            {
                _objectPools.Add(child.GetComponent<ObjectPool>());
            }
        }

        /// <summary> Xoá object khỏi pool và đánh dấu là có thể tái sử dụng  </summary>
        public virtual void RemoveObjectFromPool(ObjectPool objectPool)
        {
            objectPool.gameObject.SetActive(false);
            objectPool.IsRecyclable = true;
        }

        /// <summary> Kiểm tra xem pool có chứa object với ID cụ thể hay không  </summary>
        public virtual bool ContainsID(string id)
        {
            foreach (var obj in _ObjectPools)
            {
                if (obj.ID == id) return true;
            }
            return false;
        }

        /// <summary> 
        /// Lấy object từ pool theo ID 
        /// </summary>
        public virtual ObjectPool GetObjectByID(string id)
        {
            if (id == "") return null;

            foreach (var obj in _ObjectPools)
            {
                if (obj.ID == id) return obj;
            }
            return null;
        }

        /// <summary> Tái sử dụng object nhàn rỗi hoặc tạo mới object từ pool   </summary>
        public ObjectPool GetOrCreateObjectPool(TypeID typeID)
        {
            ObjectPool objectPool = GetDisabledObject(typeID);

            if (objectPool) // tái sử dụng
            {
                objectPool.IsRecyclable = false;
                objectPool.gameObject.SetActive(true);
            }
            else // Create New 
            {
                foreach (var prefab in _prefabs)
                {
                    ObjectPool pO = prefab.GetComponent<ObjectPool>();

                    if (pO && pO.TypeID == typeID)
                    {
                        objectPool = Instantiate(pO, transform);
                        _objectPools.Add(objectPool);
                        break;
                    }
                }
            }

            if (objectPool)
            {
                objectPool.GenerateIdentifier();
            }
            else
            {
                Debug.LogWarning($"Item {typeID} Này Tạo từ pool không thành công");
            }

            return objectPool;
        }

        /// <summary> Tìm object nhàn rỗi trong pool theo typeID  </summary>
        private ObjectPool GetDisabledObject(TypeID typeID)
        {
            foreach (var objectPool in _objectPools)
            {
                if (objectPool.TypeID == typeID && objectPool.IsRecyclable)
                {
                    return objectPool;
                }
            }
            return null;
        }
    }

}