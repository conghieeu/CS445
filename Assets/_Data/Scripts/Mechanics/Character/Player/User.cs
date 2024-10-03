using System;
using UnityEngine;

public class User : GameBehavior, ISaveData
{
    [Header("USER")]
    public string UserID;
    public string UserName;
    public float HighestMoney;
    public float PlayTime; // Tổng thời gian chơi tính bằng phút

    public static event Action OnDataChange;

    public void SetProperties(PlayerProfileData data)
    {
        UserName = data.UserName;
        HighestMoney = data.HighestMoney;
        PlayTime = data.PlayTime;

        OnDataChange?.Invoke();
    }

    public PlayerProfileData GetData()
    {
        return new PlayerProfileData(UserID, UserName, HighestMoney, PlayTime);
    }

    #region Save Game
    public void SetVariables<T, V>(T data)
    {
        if (data is PlayerProfileData playerProfileData)
        {
            UserName = playerProfileData.UserName;
            HighestMoney = playerProfileData.HighestMoney;
            PlayTime = playerProfileData.PlayTime;
        }
    }

    public void LoadVariables()
    {
        // throw new NotImplementedException();
    }

    public void SaveData()
    {
        DataManager.Instance.GameData._playerProfileData = GetData<PlayerProfileData, object>();
    }

    public T GetData<T, V>()
    {
        PlayerProfileData playerProfileData = new PlayerProfileData(UserID, UserName, HighestMoney, PlayTime);
        return (T)(object)(playerProfileData);
    }

    #endregion
}