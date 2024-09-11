using TMPro;
using UnityEngine;

namespace CuaHang.UI
{
    public class UITopBar : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _txtMoney;

        private void Start()
        {
            UpdateTextCoin(PlayerCtrl.Instance.Money);
        }

        private void OnEnable()
        {
            PlayerCtrl.OnChangeMoney += UpdateTextCoin;
        }

        private void OnDisable()
        {
            PlayerCtrl.OnChangeMoney -= UpdateTextCoin;
        }

        private void UpdateTextCoin(float money)
        {
            _txtMoney.text = $"Tổng tiền: {money.ToString("F1")}";
        }
    }

}