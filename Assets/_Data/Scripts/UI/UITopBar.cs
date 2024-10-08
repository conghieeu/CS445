using TMPro;
using UnityEngine;

namespace CuaHang.UI
{
    public class UITopBar : GameBehavior
    {
        [SerializeField] TextMeshProUGUI _txtMoney;
        [SerializeField] TextMeshProUGUI _txtReputation;

        PlayerCtrl m_playerCtrl;

        private void Start()
        {
            m_playerCtrl = FindFirstObjectByType<PlayerCtrl>();

            OnChangeMoney(m_playerCtrl.Money);
            OnChangeReputation(m_playerCtrl.Reputation);
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