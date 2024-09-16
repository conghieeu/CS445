using System;
using UnityEngine;

namespace CuaHang
{
    public abstract class ObjectPool : HieuBehavior
    {
        [Header("OBJECT POOL")]
        [SerializeField] protected string _id; // định danh đốit tượng
        [SerializeField] protected TypeID _typeID; // mẫu mã của từng loại
        [SerializeField] protected Type _type; // dùng để sếp loại
        [SerializeField] protected string _name;
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

    }
}
