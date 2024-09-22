using System.Collections.Generic;
using CuaHang.AI;
using CuaHang.Pooler;
using QFSW.QC;
using UnityEngine;

namespace CuaHang
{
    public class CustomerPoolerStats : ObjectStats
    {
        List<CustomerData> _customersData = new List<CustomerData>();
 
        public override void OnSetData<T>(T data)
        {
            if(!(data is GamePlayData)) return;

            _customersData = (data as GamePlayData).CustomersData;
        }

        public override void OnLoadData()
        {
            Debug.Log("Customer load");

            // tái tạo items data
            foreach (var cusData in _customersData)
            {
                ObjectPool customer = GetComponent<CustomerPooler>().GetObjectByID(cusData.Id);
 
                if (customer) // load data những đối tượng đã tồn tại
                {
                    customer.GetComponent<CustomerStats>().OnSetData(cusData);
                }
                else // tạo mới
                { 
                    ObjectPool newCustomer = GetComponent<CustomerPooler>().GetOrCreateObjectPool(cusData.TypeID);
                    Debug.Log("Cus 1", newCustomer);
                    newCustomer.GetComponent<CustomerStats>().OnSetData(cusData);
                    Debug.Log("Cus 2", newCustomer);
                }
            }
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
                if (pool && pool.ID != "" && pool.gameObject.activeInHierarchy)
                {
                    customersData.Add(pool.GetComponent<CustomerStats>().GetData());
                }
            }

            return customersData;
        }


    }
}