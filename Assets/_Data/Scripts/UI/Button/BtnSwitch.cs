using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CuaHang.UI
{
    [RequireComponent(typeof(Button))]
    public class BtnSwitch : GameBehavior
    {
        [Header("UI Panel")]
        [SerializeField] UIPanel _panelOpen;
        [SerializeField] UIPanel _panelClose;

        [Header("Active Object")]
        [SerializeField] Transform _objectCloseHolder;
        [SerializeField] GameObject _objectOpen;
        [SerializeField] List<GameObject> _listObjectClose;

        Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            CloseAllPanels();
            OpenNewPanel();
        }

        private void CloseAllPanels()
        {
            foreach (var panel in _listObjectClose)
            {
                if (panel != null) // Kiểm tra xem đối tượng có khác null không
                {
                    panel.SetActive(false); // Tắt đối tượng
                }
            }

            if(_objectCloseHolder)
            {
                CloseAllChildren(_objectCloseHolder);
            }

            if (_objectOpen) _objectOpen.SetActive(true);
        }

        private void OpenNewPanel()
        {
            if (_panelClose)
            {
                _panelClose.SetActiveContents(false);
            }

            if (_panelOpen)
            {
                _panelOpen.SetActiveContents(true);
            }

        }
    }

}
