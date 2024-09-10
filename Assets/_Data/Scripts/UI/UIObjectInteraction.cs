
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace CuaHang.UI
{
    public class UIObjectInteraction : MonoBehaviour
    {
        [SerializeField] Item _itemSelected; 

        [Header("On Item Drag")]
        [SerializeField] PointDragItem _buttonOnDrag;

        [Header("On Item Editing")]
        [SerializeField] Button _buttonCancelEdit;

        [Header("Menu Context")]
        [SerializeField] GameObject _menuContext;
        [SerializeField] Button _buttonDrag;
        [SerializeField] Button _buttonShowInfo;
        [SerializeField] Button _buttonEdit;

        [Header("Object Info Panel")]
        [SerializeField] bool _isShowInfo;
        [SerializeField] GameObject _infoPanel;
        [SerializeField] TextMeshProUGUI _tmp;
        [SerializeField] string _defaultTmp;
        [SerializeField] BtnPressHandler _btnIncreasePrice;
        [SerializeField] BtnPressHandler _btnDiscountPrice;

        CameraControl _cameraControl;
        RaycastCursor _raycastCursor;
        InputImprove _input;
        ItemDrag _itemDrag;

        private void Awake()
        {
            _cameraControl = CameraControl.Instance;
            _input = InputImprove.Instance;
            _itemDrag = RaycastCursor.Instance._ItemDrag;
        }

        private void Start()
        {
            _defaultTmp = _tmp.text;
            _raycastCursor = RaycastCursor.Instance;
        }

        private void OnEnable()
        {
            _input.ShowInfo += ctx => ShowObjectDetails();
            _input.DragItem += ctx => OnClickButtonDrag();

            _btnIncreasePrice.OnButtonDown += IncreasePrice;
            _btnDiscountPrice.OnButtonDown += DiscountPrice;
            _btnIncreasePrice.OnButtonHolding += IncreasePrice;
            _btnDiscountPrice.OnButtonHolding += DiscountPrice;
        }

        private void OnDisable()
        {
            _input.ShowInfo -= ctx => ShowObjectDetails();
            _input.DragItem -= ctx => OnClickButtonDrag();

            _btnIncreasePrice.OnButtonDown -= IncreasePrice;
            _btnDiscountPrice.OnButtonDown -= DiscountPrice;
            _btnIncreasePrice.OnButtonHolding -= IncreasePrice;
            _btnDiscountPrice.OnButtonHolding -= DiscountPrice;
        }

        private void FixedUpdate()
        {
            // Get Item selected
            if (_raycastCursor._ItemSelect != null && _raycastCursor._ItemSelect != _itemSelected && !_itemDrag._isDragging)
            {
                _itemSelected = _raycastCursor._ItemSelect.GetComponentInChildren<Item>();
            }
            else
            {
                _itemSelected = null;
            }

            if (_buttonShowInfo.GetComponent<Image>().enabled == false)
            {
                _infoPanel.SetActive(false);
                _isShowInfo = false;
            }

            OnEditItem();
            OnDragItem();
            OnSelectedItem();
        }

        /// <summary> bật button cancel edit </summary>
        private void OnEditItem()
        {
            if (_cameraControl.ItemEditing)
            {
                _buttonCancelEdit.gameObject.SetActive(true);
            }
            else
            {
                _buttonCancelEdit.gameObject.SetActive(false);
            }
        }

        /// <summary> Hiện button di chuyển item </summary>
        private void OnClickButtonDrag()
        {
            if(GameSystem.CurrentPlatform != Platform.Android) return;

            _buttonOnDrag.SetActiveCanvasGroup(true);
            _buttonOnDrag.transform.position = _input.MousePosition();
            _buttonOnDrag.transform.position = _buttonDrag.transform.position;
        }

        /// <summary> bật button rotation item drag </summary>
        private void OnDragItem()
        {
            if(GameSystem.CurrentPlatform != Platform.Android) return;

            if (_itemDrag.gameObject.activeInHierarchy)
            { 
                _itemDrag.MoveItemDragOnAndroid(); // di chuyen item theo point drag
            }
            else
            {
                _buttonOnDrag.SetActiveCanvasGroup(false);
            }
        }

        private void FollowTarget(Transform target, Transform follower)
        {
            Vector3 worldPosition = target.position;
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            follower.position = screenPosition;
        }

        /// <summary> Hiện option có thể chọn khi click đối tượng item </summary>
        private void OnSelectedItem()
        {
            if (_itemSelected && _itemSelected._isCanDrag && _cameraControl.ItemEditing != _itemSelected)
            {
                _buttonShowInfo.GetComponent<Image>().enabled = true;

                // Trường hợp gặp select item khong the edit
                if (_itemSelected._camHere) _buttonEdit.gameObject.SetActive(true);
                else _buttonEdit.gameObject.SetActive(false);

                _buttonDrag.gameObject.SetActive(true);

                // dat panel lai vi tri item select
                FollowTarget(_itemSelected.transform, _menuContext.transform);
            }
            else
            {
                _buttonDrag.gameObject.SetActive(false);
                _buttonEdit.gameObject.SetActive(false);
                _buttonShowInfo.GetComponent<Image>().enabled = false;
            }
        }

        private void ShowObjectDetails()
        {
            _isShowInfo = !_isShowInfo;

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

            _infoPanel.SetActive(_isShowInfo);
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
