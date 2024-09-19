using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CuaHang
{
    public class PlayerCtrl : Singleton<PlayerCtrl>
    {
        public SensorCast _sensorForward; // cảm biến đằng trước
        public Animator _anim;

        [SerializeField] int _reputation;
        [SerializeField] string _name;
        [SerializeField] float _money;
        [SerializeField] Transform _posHoldParcel; // vị trí đặt cái parcel này trên tay

        PlayerManager _playerManager => PlayerManager.Instance;

        public string Name { get => Name; private set => Name = value; }
        public int Reputation
        {
            get => _reputation;
            set
            {
                if (value >= 999999) _reputation = 999999;
                else if (value <= 0) _reputation = 0;
                else _reputation = value;

                ActionReputationChange?.Invoke(_reputation);
            }
        }
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

        protected override void Awake()
        {
            base.Awake();
            _anim = GetComponentInChildren<Animator>();
        }

        public void SetProperties(PlayerData data)
        {
            Money = data.CurrentMoney;
            Reputation = data.Reputation;
            transform.position = data.Position;
            transform.rotation = data.Rotation;
        }

        public void AddMoney(float amount)
        {
            Money += amount;
        }

        public void UpdateReputation(int reputation)
        {
            _reputation += reputation;
        }
    }
}