using System.Collections.Generic;
using CuaHang.AI;
using CuaHang.Pooler;
using Core;
using UnityEngine;

namespace CuaHang
{
    public class CustomerPoolerStats : ObjectStats
    {
        [Header("CUSTOMER POOLER STATS")]
        [SerializeField] CustomerPooler _cusPooler;

        protected override void Start()
        {
            base.Start();
            _cusPooler = GetComponent<CustomerPooler>();
        }

        protected override void SaveData()
        {
            List<CustomerData> cussData = new List<CustomerData>();

            foreach (var pool in _cusPooler._ObjectPools)
            {
                if (pool && pool._ID != "" && pool.gameObject.activeInHierarchy)
                {
                    cussData.Add(pool.GetComponent<CustomerStats>().GetData());
                }
            }

            GetGameData()._customersData = cussData;
        }

        public override void LoadData<T>(T data)
        { 
            List<CustomerData> customersData = (data as GameData)._customersData;

            // tái tạo items data
            foreach (var cusData in customersData)
            {
                // load data những đối tượng đã tồn tại
                if (_cusPooler.GetObjectID(cusData._id))
                {
                    _cusPooler.GetObjectID(cusData._id).GetComponent<CustomerStats>().LoadData(cusData);
                }
                else
                {
                    // tạo
                    ObjectPool customer = _cusPooler.GetObjectPool(cusData._typeID);
                    customer.GetComponent<CustomerStats>().LoadData(cusData);
                }
            }
        }
    }
}