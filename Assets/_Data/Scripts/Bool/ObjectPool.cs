using System;
using UnityEngine;

namespace CuaHang
{
    public abstract class ObjectPool : HieuBehavior
    {
        [Header("POOL OBJECT")]
        [SerializeField] private string _id;
        public TypeID _typeID;
        public string _name;
        public bool _isRecyclable;

        private void Reset()
        {
            CreateID();
        }

        public virtual void OnCreate()
        {
            CreateID();
        }

        public string GenerateIdentifier => System.Guid.NewGuid().ToString();

        public string _ID
        {
            get => _id;
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _id = value;
            }
        }

        /// <summary> tạo mã định danh </summary> 
        public void CreateID()
        {
            _id = GenerateIdentifier;
        }

    }
}
