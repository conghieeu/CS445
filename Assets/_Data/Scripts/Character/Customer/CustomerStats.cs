
using System.Collections.Generic;
using CuaHang.Pooler;
using UnityEngine;

namespace CuaHang.AI
{
    public class CustomerStats : ObjectStats
    {
        [Header("ItemStats")]
        [SerializeField] CustomerData _customerData;

        // Lay du lieu cua chinh cai nay de save
        public CustomerData GetData()
        {
            Customer customer = GetComponent<Customer>();
            _customerData = new CustomerData(
                customer.ID,
                customer.TypeID,
                customer.Name,
                customer.TotalPay,
                customer.IsDoneShopping,
                customer.IsPlayerConfirmPay,
                customer.transform.position,
                customer.transform.rotation);

            return _customerData;
        }

        /// <summary> CusomterPooler se yeu cau load </summary>
        public override void OnSetData<T>(T data)
        {
            if (data is CustomerData)
            {
                _customerData = data as CustomerData;
            }
        }

        protected override void SaveData() { }

        public override void OnLoadData()
        {
            if (GetGameData()._gamePlayData.IsInitialized)
            {
                GetComponent<Customer>().SetProperties(_customerData);
            }
        }
    }

}