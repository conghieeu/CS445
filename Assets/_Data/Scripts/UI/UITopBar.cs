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
            OnChangeMoney(PlayerCtrl.Instance.Money);
            OnChangeReputation(PlayerCtrl.Instance.Reputation);
        }

        private void OnEnable()
        {
            PlayerCtrl.ActionMoneyChange += OnChangeMoney;
            PlayerCtrl.ActionReputationChange += OnChangeReputation;
        }

        private void OnDisable()
        {
            PlayerCtrl.ActionMoneyChange -= OnChangeMoney;
            PlayerCtrl.ActionReputationChange -= OnChangeReputation;
        }

        private void OnChangeReputation(float reputation)
        {
            _txtReputation.text = $"Uy tín: {reputation.ToString()}";
        } 

        private void OnChangeMoney(float money)
        {
            _txtMoney.text = $"Coin: {money.ToString("F1")}";
        }
 
    }
}