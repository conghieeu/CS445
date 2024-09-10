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
            List<CustomerData> customersData = new List<CustomerData>();

            foreach (var pool in _cusPooler._ObjectPools)
            {
                if (pool && pool._ID != "" && pool.gameObject.activeInHierarchy)
                {
                    customersData.Add(pool.GetComponent<CustomerStats>().GetData());
                }
            }

            GetGameData()._customersData = customersData;
        }

        public override void LoadData<T>(T data)
        { 
            List<CustomerData> customersData = (data as GameData)._customersData;

            // tái tạo items data
            foreach (var cusData in customersData)
            {
                ObjectPool customer = GetComponent<CustomerPooler>().GetObjectByID(cusData._id);

                // load data những đối tượng đã tồn tại
                if (customer)
                {
                    customer.GetComponent<CustomerStats>().LoadData(cusData);
                }
                else
                {
                    // tạo mới
                    ObjectPool newCustomer = GetComponent<CustomerPooler>().GetOrCreateObjectPool(cusData._typeID);
                    newCustomer.GetComponent<CustomerStats>().LoadData(cusData);
                }
            }
        }

        protected override void LoadNoData()
        {
            // throw new System.NotImplementedException();
        }
    }
}