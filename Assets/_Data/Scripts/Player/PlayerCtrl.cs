using System; 
using CuaHang.AI;
using QFSW.QC;
using UnityEngine;

namespace CuaHang
{
    public class PlayerCtrl : Singleton<PlayerCtrl>
    {
        public SensorCast _sensorForward;
        public Animator _anim;

        [SerializeField] string _name;
        [SerializeField] float _money;
        [SerializeField] Transform _posHoldParcel;
        
        [Header("Reputation")]
        [SerializeField] int _currentReputation; // danh tieng
        [SerializeField] int maxReputation = 100;
        [SerializeField] int minReputation = 0;

        public string Name { get => Name; private set => Name = value; }
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

        protected override void Awake()
        {
            base.Awake();
            _anim = GetComponent<Animator>();
        }

        public void SetProperties(PlayerData data)
        { 
            Money = data.CurrentMoney;
            Debug.Log(data.Reputation);
            Reputation = data.Reputation;
            transform.position = data.Position;
            transform.rotation = data.Rotation;
        } 

        public void UpdateReputation(CustomerAction action)
        {
            Debug.Log(action);
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
    }
}