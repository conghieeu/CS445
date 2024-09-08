using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using CuaHang.AI;

namespace CuaHang.UI
{
    /// <summary> Khi có khách hàng muốn thanh toán, Đây là thanh hiển thị khách hàng ở trên máy tinh </summary>
    public class BtnBarCustomer : MonoBehaviour
    {
        public Customer _customerObserve; // là khách hàng mà cái này đang chứa
        public Image _image;
        public TextMeshProUGUI _txtTotal;
        public TextMeshProUGUI _txtGive;
        public Button _btnBarCus; 
        public UIComputerScreen _uIComputerScreen;

        private void Start()
        {
            _uIComputerScreen = GetComponentInParent<UIComputerScreen>();
            
            _btnBarCus = GetComponent<Button>();
            _btnBarCus.onClick.AddListener(OnClickThisBtnBar);
        }

        private void OnClickThisBtnBar()
        {
            Customer c = _customerObserve;
            _uIComputerScreen.CustomerSelectMark = _customerObserve;
            _uIComputerScreen.TxtCustomerValue.text = $"{this.name}\nMua: {c._totalPay}\nTiền đưa bạn: 300";
        }

        /// <summary> Hiện những thống số của khách hàng lênh cái thanh này </summary>
        public void SetCustomer(Customer customer)
        {
            _customerObserve = customer;
            _txtTotal.text = "Total: " + _customerObserve._totalPay.ToString("F1");
        }
    }
}
