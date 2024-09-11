using System;
using UnityEngine;

namespace CuaHang
{
    public class PlayerStats : ObjectStats
    {
        [Header("PlayerStats")]
        public PlayerData _playerData;

        public static event Action<PlayerData> _OnDataChange;

        PlayerCtrl _playerCtrl;

        protected override void Start()
        {
            _playerCtrl = GetComponent<PlayerCtrl>();
            base.Start();
        }

        public override void LoadData<T>(T data)
        {
            if (data is GameData) _playerData = (data as GameData)._playerData;
            else if (data is PlayerData) _playerData = data as PlayerData;

            if (!_playerData.IsInitialized) return;

            // set properties
            _playerCtrl.SetProperties(_playerData);
            _OnDataChange?.Invoke(_playerData);
        }

        protected override void SaveData()
        {
            _playerData = GetData();
            GetGameData()._playerData = _playerData;
        }

        protected override void LoadNoData()
        {
            SaveData();
            LoadData(GetData());
        }

        /// <summary> Lấy dữ liệu trạng thái hiện tại của đối tương này </summary>
        public PlayerData GetData()
        {
            PlayerCtrl player = GetComponent<PlayerCtrl>();
            ReputationSystem reputationSystem = ReputationSystem.Instance;
            PlayerData data = new PlayerData(true, player.Name, player.Money, reputationSystem.Reputation, transform.position, transform.rotation);

            return data;
        }

    }
}