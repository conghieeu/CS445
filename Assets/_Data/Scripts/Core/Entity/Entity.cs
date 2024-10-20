using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CuaHang
{
    public class Entity : GameBehavior, ISaveData, IPointerClickHandler
    {
        [Header("ENTITY")]
        [SerializeField] protected string _id; // định danh đốit tượng
        [SerializeField] protected string _name;
        [SerializeField] protected TypeID _typeID; // mẫu mã của từng loại
        [SerializeField] protected Type _type; // dùng để sếp loại
        [SerializeField] protected bool _isRecyclable;

        public TypeID TypeID { get => _typeID; set => _typeID = value; }
        public Type Type { get => _type; set => _type = value; }
        public string Name { get => _name; set => _name = value; }
        public bool IsRecyclable { get => _isRecyclable; set => _isRecyclable = value; }
        public string ID
        {
            get => _id;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _id = value;
                }
            }
        }

        [ContextMenu("Generate Identifier")]
        public void GenerateIdentifier()
        {
            _id = System.Guid.NewGuid().ToString();
        }

        public virtual EntityData GetEntityData()
        {
            return new EntityData(ID, Name, _isRecyclable, TypeID, transform.position, transform.rotation);
        }

        public virtual void RemoveThis()
        {
            gameObject.SetActive(false);
            IsRecyclable = true;
        }

        public virtual void PickUpEntity(Entity entity)
        {
            // for override
        }

        public virtual void SetVariables<T, V>(T data)
        {
            if (data is EntityData entityData)
            {
                ID = entityData.Id;
                Name = entityData.Name;
                TypeID = entityData.TypeID;
                transform.position = entityData.Position;
                transform.rotation = entityData.Rotation;
            }
        }

        public virtual void LoadVariables()
        {
            // for override
        }

        public virtual void SaveData()
        {
            // for override
        }

        public virtual T GetData<T, D>()
        {
            throw new NotImplementedException();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"CLICK THIS ITEM");
        }
    }
}
