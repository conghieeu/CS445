
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
        [SerializeField] Button _buttonEdit;
        [SerializeField] Button _buttonCancelEdit;

        [Header("Object Info Panel")]
        [SerializeField] bool _isShowInfoPanel;
        [SerializeField] GameObject _infoPanel;
        [SerializeField] TextMeshProUGUI _tmp;
        [SerializeField] string _defaultTmp;
        [SerializeField] BtnPressHandler _btnIncreasePrice;
        [SerializeField] BtnPressHandler _btnDiscountPrice;

        CameraControl _cameraControl;
        RaycastCursor _raycastCursor;
        InputImprove _input;

        private void Awake()
        {
            _cameraControl = CameraControl.Instance;
            _input = new InputImprove();
        }

        void Start()
        {
            _defaultTmp = _tmp.text;
            _raycastCursor = SingleModuleManager.Instance._raycastCursor;
        }

        void OnEnable()
        {
            _input.ShowInfo += ctx => { _isShowInfoPanel = !_isShowInfoPanel; };

            _btnIncreasePrice.OnButtonDown += IncreasePrice;
            _btnDiscountPrice.OnButtonDown += DiscountPrice;
            _btnIncreasePrice.OnButtonHolding += IncreasePrice;
            _btnDiscountPrice.OnButtonHolding += DiscountPrice;
        }

        void OnDisable()
        {
            _input.ShowInfo -= ctx => { _isShowInfoPanel = false; };

            _btnIncreasePrice.OnButtonDown -= IncreasePrice;
            _btnDiscountPrice.OnButtonDown -= DiscountPrice;
            _btnIncreasePrice.OnButtonHolding -= IncreasePrice;
            _btnDiscountPrice.OnButtonHolding -= DiscountPrice;
        }

        void FixedUpdate()
        {
            // Get Item selected
            if (_raycastCursor._itemFocus && !SingleModuleManager.Instance._itemDrag._isDragging)
            {
                _itemSelected = _raycastCursor._itemFocus.GetComponentInChildren<Item>();
            }
            else
            {
                _itemSelected = null;
            }


            if (_cameraControl._itemEditing)
            {
                _buttonCancelEdit.gameObject.SetActive(true);
            }
            else
            {
                _buttonCancelEdit.gameObject.SetActive(false);
            }

            ShowMenuContext();
            ShowObjectDetails();
        }

        private void ShowMenuContext()
        {
            if (_itemSelected && _itemSelected._isCanDrag && _cameraControl._itemEditing != _itemSelected)
            {
                _menuContext.SetActive(true);

                // dat panel lai vi tri item select
                Vector3 worldPosition = _itemSelected.transform.position;
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
                _menuContext.transform.position = screenPosition;

                // Hiện button Edit
                if (_itemSelected._camHere)
                {
                    _buttonEdit.gameObject.SetActive(true);
                }
                else
                {
                    _buttonEdit.gameObject.SetActive(false);
                }
            }
            else
            {
                _menuContext.SetActive(false);
            }

        }

        void ShowObjectDetails()
        {
            // hiện thị stats
            if (_itemSelected && _itemSelected._SO)
            {
                string x = $"Name: {_itemSelected._name} \nPrice: {_itemSelected._price.ToString("F1")} \n";
                _tmp.text = _itemSelected._SO._isCanSell ? x + "Item có thể bán" : x + "Item không thể bán";
            }
            else
            {
                _tmp.text = _defaultTmp;
            }

            // show info panel
            if (!_itemSelected) _isShowInfoPanel = false;
            _infoPanel.SetActive(_itemSelected && _isShowInfoPanel);
        }

        // --------------BUTTON--------------

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
