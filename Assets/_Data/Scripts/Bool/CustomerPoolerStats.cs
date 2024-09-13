using System.Collections.Generic;
using CuaHang.AI;
using CuaHang.Pooler;
using Core;
using UnityEngine;

namespace CuaHang
{
    public class CustomerPoolerStats : ObjectStats
    {
        public override void LoadData<T>(T data)
        {
            List<CustomerData> customersData = (data as GameData)._gamePlayData.CustomersData;

            // tái tạo items data
            foreach (var cusData in customersData)
            {
                ObjectPool customer = GetComponent<CustomerPooler>().GetObjectByID(cusData.Id);

                // load data những đối tượng đã tồn tại
                if (customer)
                {
                    customer.GetComponent<CustomerStats>().LoadData(cusData);
                }
                else
                {
                    // tạo mới
                    ObjectPool newCustomer = GetComponent<CustomerPooler>().GetOrCreateObjectPool(cusData.TypeID);
                    newCustomer.GetComponent<CustomerStats>().LoadData(cusData);
                }
            }
        }

        protected override void LoadNewGame()
        {
            SaveData();
        }

        protected override void LoadNewData()
        {
            SaveData();
        }

        protected override void SaveData()
        {
            List<CustomerData> customersData = GetData();

            GetGameData()._gamePlayData.CustomersData = customersData;
        }

        private List<CustomerData> GetData()
        {
            List<CustomerData> customersData = new List<CustomerData>();
            CustomerPooler cusPooler = GetComponent<CustomerPooler>();

            foreach (var pool in cusPooler._ObjectPools)
            {
                if (pool && pool._ID != "" && pool.gameObject.activeInHierarchy)
                {
                    customersData.Add(pool.GetComponent<CustomerStats>().GetData());
                }
            }

            return customersData;
        }
    }
}