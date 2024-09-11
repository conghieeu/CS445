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

        [SerializeField] string _name;
        [SerializeField] float _money;
        [SerializeField] Transform _posHoldParcel; // vị trí đặt cái parcel này trên tay

        public string Name { get => _name; private set => _name = value; }
        public float Money
        {
            get => _money;
            set
            {
                if (value > 0 && value < 999999) _money = value;
                OnChangeMoney?.Invoke(Money);
            }
        }

        public Transform PosHoldParcel { get => _posHoldParcel;}

        public static event Action<float> OnChangeMoney;

        protected override void Awake()
        {
            base.Awake();
            _anim = GetComponentInChildren<Animator>(); 
        }

        public void SetProperties(PlayerData data)
        {
            Name = data.Name;
            Money = data.Money;
            transform.position = data.Position;
            transform.rotation = data.Rotation; 
        }
    }
}