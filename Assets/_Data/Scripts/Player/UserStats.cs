using System;
using UnityEngine;

public class UserStats : ObjectStats
{
    [SerializeField] PlayerProfileData _playerProfileData;

    public override void OnSetData<T>(T data)
    {
        if (data is PlayerProfileData)
        {
            _playerProfileData = data as PlayerProfileData;
        }
    }

    public override void OnLoadData()
    {
        if (_playerProfileData == null) return;

        // set properties
        GetComponent<User>().SetProperties(_playerProfileData);
        
    }

    protected override void SaveData()
    {
        _playerProfileData = GetData();
        GetGameData()._playerProfileData = _playerProfileData;
    }

    private PlayerProfileData GetData()
    {
        User playerManager = GetComponent<User>();
        PlayerProfileData data = new(playerManager.UserName, playerManager.HighestMoney, playerManager.PlayTime);
        return data;
    }

}

