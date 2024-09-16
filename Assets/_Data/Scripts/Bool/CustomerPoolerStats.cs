using System.Collections.Generic;
using CuaHang.AI;
using CuaHang.Pooler;

namespace CuaHang
{
    public class CustomerPoolerStats : ObjectStats
    {
        List<CustomerData> _customersData = new List<CustomerData>();

        public List<CustomerData> CustomersData { get => _customersData; set => _customersData = value; }

        public override void OnSetData<T>(T data)
        {
            CustomersData = (data as GameData)._gamePlayData.CustomersData;
        }

        public override void OnLoadData()
        {
            // tái tạo items data
            foreach (var cusData in CustomersData)
            {
                ObjectPool customer = GetComponent<CustomerPooler>().GetObjectByID(cusData.Id);

                // load data những đối tượng đã tồn tại
                if (customer)
                {
                    customer.GetComponent<CustomerStats>().OnSetData(cusData);
                }
                else
                {
                    // tạo mới
                    ObjectPool newCustomer = GetComponent<CustomerPooler>().GetOrCreateObjectPool(cusData.TypeID);
                    newCustomer.GetComponent<CustomerStats>().OnSetData(cusData);
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
                if (pool && pool.ID != "" && pool.gameObject.activeInHierarchy)
                {
                    customersData.Add(pool.GetComponent<CustomerStats>().GetData());
                }
            }

            return customersData;
        }


    }
}