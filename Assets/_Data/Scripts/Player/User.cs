using System;
using UnityEngine;

public class User : Singleton<User>
{

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
}