using System;
using CuaHang;
using UnityEngine;
using UnityEngine.Events;

public class User : GameBehavior, ISaveData
{
    [Header("USER")]
    [SerializeField] string userID;
    public string UserName;
    public float HighestMoney;
    public float PlayTime; // Tổng thời gian chơi tính bằng phút

    PlayerCtrl m_PlayerCtrl;

    public UnityAction<User> OnDataChange;

    public string UserID
    {
        get => userID; set
        {
            userID = value;
            OnDataChange?.Invoke(this);
        }
    }

    private void Start()
    {
        m_PlayerCtrl = FindFirstObjectByType<PlayerCtrl>();
    }

    private void FixedUpdate()
    {
        PlayTime += Time.fixedDeltaTime / 60; // Chuyển đổi giây thành phút
        PlayTime = Mathf.Round(PlayTime); // Làm tròn kết quả

        if (m_PlayerCtrl)
        {
            SetHighestMoney(m_PlayerCtrl.Money);
        }
    }

    public void SetHighestMoney(float money)
    {
        if (money > HighestMoney)
        {
            HighestMoney = money;
            OnDataChange?.Invoke(this);
        }
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
            UserID = playerProfileData.UserID;
            UserName = playerProfileData.UserName;
            HighestMoney = playerProfileData.HighestMoney;
            PlayTime = playerProfileData.PlayTime;

            OnDataChange?.Invoke(this);
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