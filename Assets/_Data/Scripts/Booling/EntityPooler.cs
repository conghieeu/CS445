using System.Collections.Generic;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class EntityPooler<S> : Singleton<S>, ISaveData where S : GameBehavior
    {
        [Header("Entity Pooler")]
        [SerializeField] protected List<Transform> _prefabs;
        [SerializeField] protected List<Entity> _objectPools;

        public List<Entity> ListEntity { get => _objectPools; private set => _objectPools = value; }

        private void OnValidate()
        {
            Init();
        }

        private void Init()
        {
            _objectPools.Clear();
            foreach (Transform child in transform)
            {
                _objectPools.Add(child.GetComponent<Entity>());
            }
        }

        /// <summary> Xoá object khỏi pool và đánh dấu là có thể tái sử dụng  </summary>
        public virtual void RemoveEntityFromPool(Entity entity)
        {
            entity.RemoveThis();
        }

        /// <summary> Kiểm tra xem pool có chứa object với ID cụ thể hay không  </summary>
        public virtual bool ContainsID(string id)
        {
            foreach (var obj in ListEntity)
            {
                if (obj.ID == id) return true;
            }
            return false;
        }

        /// <summary> Lấy object từ pool theo ID </summary>
        public virtual Entity GetObjectByID(string id)
        {
            if (id == "") return null;

            foreach (var obj in ListEntity)
            {
                if (obj.ID == id) return obj;
            }
            return null;
        }

        /// <summary> Tái sử dụng object nhàn rỗi hoặc tạo mới object từ pool </summary>
        public Entity GetOrCreateObjectPool(TypeID typeID)
        {
            Entity objectPool = GetDisabledObject(typeID);

            if (objectPool) // tái sử dụng
            {
                objectPool.IsRecyclable = false;
                objectPool.gameObject.SetActive(true);
            }
            else // Create New 
            {
                foreach (var prefab in _prefabs)
                {
                    Entity entity = prefab.GetComponent<Entity>();

                    if (entity && entity.TypeID == typeID)
                    {
                        objectPool = Instantiate(entity, transform);
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
        private Entity GetDisabledObject(TypeID typeID)
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

        #region SaveData
        /// <summary> Entity sẽ truyền loại data vào đây set dữ liệu từ data, T: Kiểu dữ liệu trả về, V: kiểu dữ liệu muốn lấy </summary>
        public virtual void SetVariables<T, V>(T data)
        {
            // T: là list<V>
            if (data is List<V> dataList == false) return;

            foreach (var iData in dataList)
            {
                if (iData is EntityData entityData)
                {
                    Entity entity = GetObjectByID(entityData.Id);
                    if (entity)
                    {
                        entity.GetComponent<ISaveData>().SetVariables<V, object>(iData);
                    }
                    else
                    {
                        if (!entityData.IsDestroyed) // không tạo những đối tuọng bị phá huỷ
                        {
                            entity = GetOrCreateObjectPool(entityData.TypeID);
                            entity.GetComponent<ISaveData>().SetVariables<V, object>(iData); 
                        }
                    }
                }
            }
        }

        public virtual void LoadVariables()
        {
            foreach (var entity in ListEntity)
            {
                entity.GetComponent<ISaveData>().LoadVariables();
            }
        }

        public T GetData<T, D>()
        {
            List<D> listStaffData = new List<D>();

            foreach (var entity in ListEntity)
            {
                if (entity && entity.ID != "")
                {
                    listStaffData.Add(entity.GetComponent<ISaveData>().GetData<D, object>());
                }
            }
            return (T)(object)listStaffData;
        }

        public virtual void SaveData()
        {

        }
        #endregion
    }
}