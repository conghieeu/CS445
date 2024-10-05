using UnityEngine.UI;
using TMPro;
using UnityEngine;
using CuaHang.AI;

namespace CuaHang.UI
{
    /// <summary> Khi có khách hàng muốn thanh toán, Đây là thanh hiển thị khách hàng ở trên máy tinh </summary>
    public class BtnBarCustomer : GameBehavior
    {
        [SerializeField] Image _imgFace;
        [SerializeField] TMP_Text _txtName;
        [SerializeField] TMP_Text _txtTotal;
        [SerializeField] TMP_Text _txtGive;
        [SerializeField] Button _btnBarCus;
        [SerializeField] UIComputerScreen _uIComputerScreen;
        [SerializeField] Customer _customerSelected;
        [SerializeField] float _customerChange;

        public Customer CustomerSelected { get => _customerSelected; set => _customerSelected = value; }
        public float CustomerChange { get => _customerChange; set => _customerChange = value; }

        private void Start()
        {
            _uIComputerScreen = GetComponentInParent<UIComputerScreen>();
            _btnBarCus = GetComponent<Button>();
            _btnBarCus.onClick.AddListener(() => _uIComputerScreen.ClickBarCustomer(this));
        }

        /// <summary> Hiện những thống số của khách hàng lênh cái thanh này </summary>
        public void SetVariables(Customer customer)
        { 
            CustomerSelected = customer;
            _txtName.text = $"Tên: {customer.Name}";
            _txtTotal.text = "Tổng tiền mua: " + CustomerSelected.TotalPay.ToString("F2");

            // khach hang dua tien nay cho player de thoi
            CustomerChange = (int)Random.Range(customer.TotalPay, PlayerCtrl.Instance.Money);
            _txtGive.text = $"Tiền đưa bạn: {CustomerChange.ToString("F2")}";
        }
 

    }
}
