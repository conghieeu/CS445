using System;
using UnityEngine;

namespace CuaHang
{
    public class PlayerStats : ObjectStats
    {
        [Header("PlayerStats")]
        public PlayerData _playerData;

        public static event Action<PlayerData> _OnDataChange;

        public override void LoadData<T>(T data)
        {
            _playerData = (data as GameData)._playerData;

            if (!_playerData._isInitialized) return;

            // set properties
            transform.position = _playerData._position;
            transform.rotation = _playerData._rotation;

            _OnDataChange?.Invoke(_playerData);
        }

        protected override void SaveData()
        {
            _playerData = new PlayerData(_playerData._name, _playerData._money, transform.rotation, transform.position);
            _playerData._isInitialized = true;
            GetGameData()._playerData = _playerData;
        }

        protected override void LoadNoData()
        {
            SaveData();
        }
    }
}