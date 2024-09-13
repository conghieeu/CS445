
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
                customer._ID,
                customer._typeID,
                customer._name,
                customer.TotalPay,
                customer.IsDoneShopping,
                customer.IsPlayerConfirmPay,
                customer.transform.position,
                customer.transform.rotation);

            return _customerData;
        }

        /// <summary> CusomterPooler se yeu cau load </summary>
        public override void LoadData<T>(T data)
        { 
            _customerData = data as CustomerData;

            if (_customerData == null) return;
            
            GetComponent<Customer>().SetProperties(_customerData);
        }

        protected override void SaveData() { }
        protected override void LoadNewGame() { }
        protected override void LoadNewData() { }
    }

}