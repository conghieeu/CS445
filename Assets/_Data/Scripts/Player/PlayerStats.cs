using System;
using UnityEngine;

namespace CuaHang
{
    public class PlayerStats : ObjectStats
    {
        [Header("PlayerStats")]
        [SerializeField] PlayerData _playerData;

        public override void OnSetData<T>(T data)
        {
            if (data is GamePlayData) _playerData = (data as GamePlayData).PlayerData;  
        }

        public override void OnLoadData()
        {
            if (GetGameData()._gamePlayData.IsInitialized)
            {
                GetComponent<PlayerCtrl>().SetProperties(_playerData);
            }
        }

        protected override void SaveData()
        {
            _playerData = GetData();
            GetGameData()._gamePlayData.PlayerData = _playerData;
        }

        /// <summary> Lấy dữ liệu trạng thái hiện tại của đối tương này </summary>
        private PlayerData GetData()
        {
            PlayerCtrl playerCtrl = GetComponent<PlayerCtrl>();
            PlayerData data = new PlayerData(playerCtrl.Money, playerCtrl.Reputation, transform.position, transform.rotation);
            return data;
        }
    }
}