using TMPro;
using UnityEngine;

namespace CuaHang.UI
{
    public class UITopBar : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _txtMoney;
        [SerializeField] TextMeshProUGUI _txtReputation;

        private void Start()
        {
            UpdateTextCoin(PlayerCtrl.Instance.Money);
            UpdateReputation(PlayerCtrl.Instance.Reputation);
        }

        private void OnEnable()
        {
            PlayerCtrl.ActionMoneyChange += UpdateTextCoin;
        }

        private void OnDisable()
        {
            PlayerCtrl.ActionMoneyChange -= UpdateTextCoin;
        }

        private void UpdateTextCoin(float money)
        {
            _txtMoney.text = $"Coin: {money.ToString("F1")}";
        }

        private void UpdateReputation(int reputation)
        {
            _txtMoney.text = $"Uy tín: {reputation.ToString()}";
        }
    }

}