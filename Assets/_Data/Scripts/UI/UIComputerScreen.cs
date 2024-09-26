using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using CuaHang.AI;
using TMPro;

namespace CuaHang.UI
{
    public class UIComputerScreen : MonoBehaviour
    {
        [Serializable]
        public class SlotBar
        {
            public Customer _customer;
            public BtnBarCustomer _bar;
        }

        [SerializeField] MayTinh _mayTinh;

        [Header("Buy Panel")]
        [SerializeField] RectTransform _panelBuyItem;

        [Header("Payment Panel")]
        [SerializeField] bool _isCanPay;
        [SerializeField] Button _btnPay;
        [SerializeField] RectTransform _panelPayment;
        [SerializeField] RectTransform _panelSlotHolder;
        [SerializeField] RectTransform _prefabBtnSlot;
        [SerializeField] TextMeshProUGUI _txtCustomerValue;
        [SerializeField] TextMeshProUGUI _txtReport;
        [SerializeField] TMP_InputField _infRefund;
        [SerializeField] float _profit;
        [SerializeField] List<WaitingLine.WaitingSlot> _comSlot;
        [SerializeField] List<SlotBar> _barSlots;

        BtnBarCustomer _btnCustomerSelected;
        PlayerCtrl _playerCtrl => PlayerCtrl.Instance;

        void Start()
        { 
            SetActiveContent(null);

            _btnPay.onClick.AddListener(OnClickPay);
            _infRefund.onValueChanged.AddListener(OnInfRefund);

            for (int i = 0; i < 10; i++) _barSlots.Add(new SlotBar());
        }

        private void OnEnable()
        {
            RaycastCursor.ActionEditItem += SetActiveContent;
        }

        private void OnDisable()
        {
            RaycastCursor.ActionEditItem -= SetActiveContent;
        }

        void FixedUpdate()
        {
            UpdateBtnSlot();
        }

        public void SetActiveContent(Item item)
        {
            if (item && item.GetComponent<MayTinh>())
            {
                if (_panelPayment) _panelPayment.gameObject.SetActive(true);
                _mayTinh = item.GetComponent<MayTinh>();
            }
            else
            {
                if (_panelPayment) _panelPayment.gameObject.SetActive(false);
                if (_panelBuyItem) _panelBuyItem.gameObject.SetActive(false);
                _mayTinh = null;
            }
        }

        public void ClickBarCustomer(BtnBarCustomer btnBarCustomer)
        {
            _btnCustomerSelected = btnBarCustomer;
            _txtCustomerValue.text = $"Name: {this.name}\nTổng mua: {btnBarCustomer.CustomerSelected.TotalPay}\nTiền đưa bạn: {btnBarCustomer.CustomerChange}";
        }

        public void OnInfRefund(string input)
        {
            _isCanPay = false; 

            if (float.TryParse(input, out float changeAmount) == false)
            {
                _txtReport.text = "Chuỗi ko hợp lệ: Chỉ chứa số.";
                return;
            }

            if (_btnCustomerSelected == null)
            {
                _txtReport.text = "Cảnh báo: Chưa chọn khách để thanh toán";
                return;
            }

            if (changeAmount < _btnCustomerSelected.CustomerChange - _btnCustomerSelected.CustomerSelected.TotalPay)
            {
                _txtReport.text = "Bạn đang lấy tiền của khách hoặc bạn chưa lựa chọn khách hàng để giao dịch";
                return;
            }

            if (changeAmount > _playerCtrl.Money)
            {
                _txtReport.text = "Cảnh báo: Không đủ tiền để thối";
            }
            else
            {
                _txtReport.text = "Bạn có thể thanh toán, tính đúng nếu không mún bị mất tiền";
                _profit = _btnCustomerSelected.CustomerChange - changeAmount;
                _isCanPay = true;
            }
        }

        // tạo slot trong panel slot holder cho cho hop voi khach hang o hang cho
        private void UpdateBtnSlot()
        {
            if (_mayTinh == null) return;

            _comSlot = _mayTinh._waitingLine._waitingSlots;

            // doi chieu su khach biet
            for (int c = 0; c < _comSlot.Count; c++)
            {
                if (_barSlots[c]._customer == _comSlot[c]._customer) continue;

                // Recreate list bar
                for (int i = 0; i < _comSlot.Count; i++)
                {
                    if (_barSlots[i]._bar) Destroy(_barSlots[i]._bar.gameObject);
                    // tao cai moi
                    _barSlots[i]._customer = _comSlot[i]._customer;

                    if (_barSlots[i]._customer)
                    {
                        BtnBarCustomer btnBar = Instantiate(_prefabBtnSlot, _panelSlotHolder).GetComponentInChildren<BtnBarCustomer>();
                        _barSlots[i]._bar = btnBar;
                        if (_comSlot[i]._customer) btnBar.SetVariables(_comSlot[i]._customer);
                    }
                }
            }
        }

        private void OnClickPay()
        {
            if (_isCanPay)
            {
                _btnCustomerSelected.CustomerSelected.IsPlayerConfirmPay = true;
                _playerCtrl.Money += _profit;
                _btnCustomerSelected = null;
                _playerCtrl.UpdateReputation(CustomerAction.Buy);
            }
        }

    }
}
