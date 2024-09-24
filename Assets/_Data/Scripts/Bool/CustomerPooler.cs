using System.Collections;
using System.Collections.Generic;
using CuaHang.AI;
using UnityEngine;
namespace CuaHang.Pooler
{
    public class CustomerPooler : EntityPooler, ISaveData
    {
        [SerializeField] Transform _goOutShopPoint;
        public static CustomerPooler Instance { get; private set; }
        public Transform GoOutShopPoint { get => _goOutShopPoint; }

        protected override void Awake()
        {
            base.Awake();

            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public override void SaveData()
        {
            DataManager.Instance.GameData._gamePlayData.CustomersData = GetData<List<CustomerData>, CustomerData>();
        }
    }
}