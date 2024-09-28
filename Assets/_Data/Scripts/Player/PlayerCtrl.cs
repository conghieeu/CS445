using System;
using CuaHang.AI;
using QFSW.QC;
using UnityEngine;

namespace CuaHang
{
    public class PlayerCtrl : Entity
    {
        public static PlayerCtrl Instance;
        public SensorCast _sensorForward;
        public Animator _anim;

        [SerializeField] Transform _posHoldParcel;
        [SerializeField] float _money;
        [SerializeField] int _currentReputation; // danh tieng
        [SerializeField] int maxReputation = 100;
        [SerializeField] int minReputation = 0;

        [Command]
        public int Reputation
        {
            get => _currentReputation;
            set
            {
                if (value >= maxReputation) _currentReputation = maxReputation;
                else if (value <= minReputation) _currentReputation = minReputation;
                else _currentReputation = value;

                ActionReputationChange?.Invoke(_currentReputation);
            }
        }
        [Command]
        public float Money
        {
            get => _money;
            set
            {
                if (value >= 999999) _money = 999999;
                else if (value <= 0) _money = 0;
                else _money = value;

                ActionMoneyChange?.Invoke(_money);
            }
        }

        public Transform PosHoldParcel { get => _posHoldParcel; }
        public static event Action<float> ActionMoneyChange;
        public static event Action<float> ActionReputationChange;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            if (Instance) Destroy(this); else { Instance = this; }
        }

        public void UpdateReputation(CustomerAction action)
        {
            switch (action)
            {
                case CustomerAction.Buy:
                    Reputation += 10;
                    break;
                case CustomerAction.Return:
                    Reputation -= 5;
                    break;
                case CustomerAction.Complain:
                    Reputation -= 15;
                    break;
                case CustomerAction.Praise:
                    Reputation += 10;
                    break;
                default:
                    Debug.Log("Hành động không xác định");
                    break;
            }
        }

        #region Save Data
        public override void SetVariables<T, V>(T data)
        { 
            if (data is GamePlayData gamePlayData)
            { 
                PlayerData playerData = gamePlayData.PlayerData;
                base.SetVariables<PlayerData, object>(playerData);
                Money = playerData.CurrentMoney;
                Reputation = playerData.Reputation;
            }
        }

        public override void SaveData()
        {
            DataManager.Instance.GameData._gamePlayData.PlayerData = GetData<PlayerData, object>();
        }

        public override T GetData<T, D>()
        {
            PlayerData data = new PlayerData(GetEntityData(), Money, Reputation);
            return (T)(object)(data);
        }
        #endregion
    }
}