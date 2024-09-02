
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace CuaHang.UI
{
    public class UIObjectInteraction : MonoBehaviour
    {
        [SerializeField] Item _itemSelected;

        [Header("Menu Context")]
        [SerializeField] GameObject _menuContext;
        [SerializeField] Button _buttonDrag;
        [SerializeField] Button _buttonShowInfo;

        [Header("Object Info Panel")]
        [SerializeField] bool _isShowInfoPanel;
        [SerializeField] GameObject _infoPanel;
        [SerializeField] TextMeshProUGUI _tmp;
        [SerializeField] string _defaultTmp;
        [SerializeField] BtnPressHandler _btnIncreasePrice;
        [SerializeField] BtnPressHandler _btnDiscountPrice;

        RaycastCursor _raycastCursor;

        void Start()
        {
            _defaultTmp = _tmp.text;
            _raycastCursor = SingleModuleManager.Instance._raycastCursor;

        }

        void OnEnable()
        {
            _buttonShowInfo.onClick.AddListener(OnClickShowInfo);
            _buttonDrag.onClick.AddListener(OnClickDrag);
            _btnIncreasePrice.OnButtonDown += IncreasePrice;
            _btnDiscountPrice.OnButtonDown += DiscountPrice;
            _btnIncreasePrice.OnButtonHolding += IncreasePrice;
            _btnDiscountPrice.OnButtonHolding += DiscountPrice;
        }

        void OnDisable()
        {

            _btnIncreasePrice.OnButtonDown -= IncreasePrice;
            _btnDiscountPrice.OnButtonDown -= DiscountPrice;
            _btnIncreasePrice.OnButtonHolding -= IncreasePrice;
            _btnDiscountPrice.OnButtonHolding -= DiscountPrice;
        }



        void FixedUpdate()
        {
            // Get Item selected
            if (_raycastCursor._itemFocus && !SingleModuleManager.Instance._objectDrag._isDragging)
            {
                _itemSelected = _raycastCursor._itemFocus.GetComponentInChildren<Item>();
            }
            else
            {
                _itemSelected = null;
            }

            ShowObjectDetails();
            ShowMenuContext();
        }

        private void ShowMenuContext()
        {
            if (_itemSelected && _itemSelected._isCanDrag)
            {
                _menuContext.SetActive(true);

                Vector3 worldPosition = _itemSelected.transform.position;
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
                _menuContext.transform.position = screenPosition;
            }
            else
            {
                _menuContext.SetActive(false);
            }
        }

        void ShowObjectDetails()
        {
            if (_itemSelected && _itemSelected._SO)
            {
                string x = $"Name: {_itemSelected._name} \nPrice: {_itemSelected._price.ToString("F1")} \n";
                _tmp.text = _itemSelected._SO._isCanSell ? x + "Item có thể bán" : x + "Item không thể bán";
            }
            else
            {
                _tmp.text = _defaultTmp;
            }

            // bật tắt tuỳ theo có item hay không
            if(!_itemSelected) _isShowInfoPanel = false;
            _infoPanel.SetActive(_itemSelected && _isShowInfoPanel);
        }

        // --------------BUTTON--------------

        private void OnClickShowInfo()
        {
            InputImprove.CallShowInfo(); 
            _isShowInfoPanel = !_isShowInfoPanel;
        }

        public void OnClickDrag()
        {
            InputImprove.CallDragPerformed();
        }

        public void IncreasePrice()
        {
            if (_itemSelected) _itemSelected.SetPrice(0.1f);
        }

        public void DiscountPrice()
        {
            if (_itemSelected) _itemSelected.SetPrice(-0.1f);
        }

    }
}
