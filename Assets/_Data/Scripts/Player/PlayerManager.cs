using System;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] string _userName;
    [SerializeField] float _highestMoney;
    [SerializeField] float _playTime; // Tổng thời gian chơi tính bằng phút 

    PlayerProfile _playerProfile;

    public string UserName { get => _userName; set => _userName = value; }
    public float HighestMoney { get => _highestMoney; set => _highestMoney = value; }
    public float PlayTime { get => _playTime; set => _playTime = value; }

    public static event Action OnDataChange;

    protected override void Awake()
    {
        base.Awake();
        _playerProfile = GetComponent<PlayerProfile>();
    }

    public void SetProperties(PlayerProfileData data)
    {
        UserName = data.UserName;
        HighestMoney = data.HighestMoney;
        PlayTime = data.PlayTime;

        OnDataChange?.Invoke();
    }
}

