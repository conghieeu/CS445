using System.Collections;
using System.Collections.Generic;
using CuaHang.AI;
using UnityEngine;
namespace CuaHang.Pooler
{
    public class CustomerPooler : EntityPooler<CustomerPooler>
    {
        [SerializeField] Transform _goOutShopPoint; 
        public Transform GoOutShopPoint { get => _goOutShopPoint; } 
 
        public override void SetVariables<T, V>(T data)
        {
            if (data is GamePlayData gamePlayData)
            {
                base.SetVariables<List<CustomerData>, CustomerData>(gamePlayData.CustomersData);
            }
        }

        public override void SaveData()
        {
            DataManager.Instance.GameData._gamePlayData.CustomersData = GetData<List<CustomerData>, CustomerData>();
        }
    }
}