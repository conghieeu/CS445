using System;
using UnityEngine;

public class PlayerProfile : ObjectStats
{
    [SerializeField] PlayerProfileData _playerProfileData;

    public static event Action<PlayerProfileData> _OnDataChange;

    public override void LoadData<T>(T data)
    {
        _playerProfileData = (data as GameData)._playerProfileData;

        // set properties
        GetComponent<PlayerManager>().SetProperties(_playerProfileData);

        // Thong bao
        _OnDataChange?.Invoke(_playerProfileData);
    }

    protected override void SaveData()
    {
        _playerProfileData = GetData();
        GetGameData()._playerProfileData = _playerProfileData;
    }

    protected override void LoadNewData()
    {
        SaveData();
        LoadData(GetData()); // mục đích cập nhập và thông báo
    }

    protected override void LoadNewGame() { }

    PlayerProfileData GetData()
    {
        PlayerManager playerManager = GetComponent<PlayerManager>();
        PlayerProfileData data = new(playerManager.UserName, playerManager.HighestMoney, playerManager.PlayTime);
        return data;
    }
}

