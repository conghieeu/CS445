using System;
using UnityEngine;

namespace CuaHang
{
    public class PlayerStats : ObjectStats
    {
        [Header("PlayerStats")]
        public PlayerData _playerData;

        public static event Action<PlayerData> _OnDataChange;

        public override void OnSetData<T>(T data)
        { 
            if (data is GameData) _playerData = (data as GameData)._gamePlayData.PlayerData;
            else if (data is PlayerData) _playerData = data as PlayerData;

            if (_playerData == null) return; 
 
            GetComponent<PlayerCtrl>().SetProperties(_playerData);
            _OnDataChange?.Invoke(_playerData);
        }

        protected override void SaveData()
        {
            _playerData = GetData();
            GetGameData()._gamePlayData.PlayerData = _playerData;
        }

        protected override void LoadNewGame()
        {
            Debug.Log("Load new game");
            SaveData();
            OnSetData(GetData()); // mục đích cập nhập và thông báo
        }

        protected override void LoadNewData()
        {
            Debug.Log("Load new data");
            SaveData();
            OnSetData(GetData());
        }

        /// <summary> Lấy dữ liệu trạng thái hiện tại của đối tương này </summary>
        PlayerData GetData()
        {
            PlayerCtrl playerCtrl = GetComponent<PlayerCtrl>();
            PlayerData data = new PlayerData(playerCtrl.CurrentMoney, playerCtrl.Reputation, transform.position, transform.rotation);
            return data;
        }
    }
}