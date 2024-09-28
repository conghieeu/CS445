using System;
using UnityEngine;

public class User : Singleton<User>, ISaveData
{
    [Header("USER")]
    [SerializeField] string _name;
    [SerializeField] float _highestMoney;
    [SerializeField] float _playTime; // Tổng thời gian chơi tính bằng phút  

    public string UserName { get => _name; set => _name = value; }
    public float HighestMoney { get => _highestMoney; set => _highestMoney = value; }
    public float PlayTime { get => _playTime; set => _playTime = value; }

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
        return new PlayerProfileData(UserName, HighestMoney, PlayTime);
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
        PlayerProfileData playerProfileData = new PlayerProfileData(UserName, HighestMoney, PlayTime);
        return (T)(object)(playerProfileData);
    }

    #endregion
}