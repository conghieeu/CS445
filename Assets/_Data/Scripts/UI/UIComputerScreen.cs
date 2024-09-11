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
        [SerializeField] Button _btnGoPayPanel;
        [SerializeField] RectTransform _panelBuyItem;

        [Header("Payment Panel")]
        [SerializeField] bool _isCanPay;
        [SerializeField] Button _btnGoBuyPanel;
        [SerializeField] Button _btnPay;
        [SerializeField] RectTransform _panelPayment;
        [SerializeField] RectTransform _panelSlotHolder;
        [SerializeField] RectTransform _prefabBtnSlot;
        [SerializeField] TextMeshProUGUI _txtCustomerValue;
        [SerializeField] TextMeshProUGUI _txtReport;
        [SerializeField] Customer _customerSelectMark;
        [SerializeField] TMP_InputField _infRefund;
        [SerializeField] float _tienThoi;
        [SerializeField] List<WaitingLine.WaitingSlot> _comSlot;
        [SerializeField] List<SlotBar> _barSlots;

        PlayerCtrl _playerCtrl;

        public Customer CustomerSelectMark { get => _customerSelectMark; set => _customerSelectMark = value; }
        public TextMeshProUGUI TxtCustomerValue { get => _txtCustomerValue; set => _txtCustomerValue = value; }

        void Start()
        {
            CameraControl._EventOnEditItem += SetActiveContent;
            SetActiveContent(null);

            _btnGoPayPanel.onClick.AddListener(OnClickGoPayPanel);
            _btnGoBuyPanel.onClick.AddListener(OnClickGoBuyPanel);
            _btnPay.onClick.AddListener(OnClickPayBtn);
            _infRefund.onValueChanged.AddListener(ValidateInput);
            _playerCtrl = PlayerCtrl.Instance;

            for (int i = 0; i < 10; i++) _barSlots.Add(new SlotBar());
        }

        void FixedUpdate()
        {
            CreateBtnSlot(); 
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

        /// <summary> được gọi mỗi khi giá trị trong inputField thay đổi </summary>
        private void ValidateInput(string input)
        {
            if (float.TryParse(input, out float tienThoi))
            {
                if (CustomerSelectMark && 300 - tienThoi <= CustomerSelectMark.TotalPay)
                {
                    if (_playerCtrl.Money < tienThoi)
                    {
                        _txtReport.text = "Cảnh báo: Không đủ tiền để thối";
                    }
                    else
                    {
                        _txtReport.text = "Bạn có thể thanh toán, tính đúng nếu không mún bị mất tiền";
                        _tienThoi = tienThoi;
                        _isCanPay = true;
                        return;
                    }
                }
                else
                {
                    _txtReport.text = "Bạn đang lấy tiền của khách hoặc bạn chưa lựa chọn khách hàng để giao dịch";
                }
            }
            else
            {
                _txtReport.text = "Chuỗi ko hợp lệ: Chỉ chứa số.";
            }

            _isCanPay = false;
        }

        // tạo slot trong panel slot holder cho cho hop voi khach hang o hang cho
        private void CreateBtnSlot()
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
                        if (_comSlot[i]._customer) btnBar.SetCustomer(_comSlot[i]._customer);
                    }
                }
            }
        }

        private void OnClickPayBtn()
        {
            if (!CustomerSelectMark) return;

            if (_playerCtrl.Money < _tienThoi)
            {
                _txtReport.text = "Cảnh báo: Không đủ tiền để thối";
                return;
            }

            if (_isCanPay)
            {
                CustomerSelectMark.IsPlayerConfirmPay = true;

                float coinAdd = 300 - _tienThoi;

                if (_playerCtrl.Money >= _tienThoi)
                {
                    _playerCtrl.Money += coinAdd;
                }

                CustomerSelectMark = null;
            }
        }

        private void OnClickGoPayPanel()
        {
            SetAtivePanel(_panelPayment);
        }

        private void OnClickGoBuyPanel()
        {
            SetAtivePanel(_panelBuyItem);
        }

        private void SetAtivePanel(Transform panel)
        {
            if (panel == _panelPayment)
            {
                _panelPayment.gameObject.SetActive(true);
                _panelBuyItem.gameObject.SetActive(false);
                return;
            }

            if (panel == _panelBuyItem)
            {
                _panelBuyItem.gameObject.SetActive(true);
                _panelPayment.gameObject.SetActive(false);
                return;
            }
        }
    }
}
