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

        [SerializeField] float _currentMoney;
        [SerializeField] int _reputation;
        [SerializeField] string _name;
        [SerializeField] float _money;
        [SerializeField] Transform _posHoldParcel; // vị trí đặt cái parcel này trên tay

        PlayerManager _playerManager => PlayerManager.Instance;

        public float CurrentMoney { get => _currentMoney; set => _currentMoney = value; }
        public int Reputation { get => _reputation; set => _reputation = value; }
        public string Name { get => Name; private set => Name = value; }
        public float Money
        {
            get => _money;
            set
            {
                if (value > 0 && value < 999999) _money = value;
                OnChangeMoney?.Invoke(_money);
            }
        }

        public Transform PosHoldParcel { get => _posHoldParcel; }
        public static event Action<float> OnChangeMoney;

        protected override void Awake()
        {
            base.Awake();
            _anim = GetComponentInChildren<Animator>();
        } 

        public void SetProperties(PlayerData data)
        { 
            _currentMoney = data.CurrentMoney;
            _reputation = data.Reputation;
            transform.position = data.Position;
            transform.rotation = data.Rotation; 
        }

        public void AddMoney(float amount)
        {
            _currentMoney += amount;

            // Kiểm tra và cập nhật highestMoney nếu cần
            if (_playerManager.HighestMoney < _currentMoney)
            {
                _playerManager.HighestMoney = _currentMoney;
            }
        }

        public void UpdateReputation(int reputation)
        {
            _reputation += reputation;
        }
    }
}