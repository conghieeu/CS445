using UnityEngine;
using System;

public class ReputationSystem : Singleton<ReputationSystem>
{
    [Header("Reputation System")]
    [SerializeField] int _reputation = 50;          // Giá trị ban đầu của danh tiếng
    [SerializeField] int maxReputation = 100;      // Giới hạn trên của danh tiếng
    [SerializeField] int minReputation = 0;        // Giới hạn dưới của danh tiếng 

    public int Reputation { get => _reputation; }

    // Định nghĩa enum cho các hành động của khách hàng
    public enum CustomerAction
    {
        Buy,
        Return,
        Complain,
        Praise
    }

    public event Action<int> OnReputationChanged;

    // Tăng danh tiếng
    public void IncreaseReputation(int amount)
    {
        _reputation += amount;
        if (_reputation > maxReputation)
        {
            _reputation = maxReputation;
        }
        OnReputationChanged?.Invoke(_reputation);
    }

    // Giảm danh tiếng
    public void DecreaseReputation(int amount)
    {
        _reputation -= amount;
        if (_reputation < minReputation)
        {
            _reputation = minReputation;
        }
        OnReputationChanged?.Invoke(_reputation);
    }

    public void UpdateReputation(CustomerAction action)
    {
        switch (action)
        {
            case CustomerAction.Buy:
                IncreaseReputation(10);
                break;
            case CustomerAction.Return:
                DecreaseReputation(5);
                break;
            case CustomerAction.Complain:
                DecreaseReputation(15);
                break;
            case CustomerAction.Praise:
                IncreaseReputation(20);
                break;
            default:
                Debug.Log("Hành động không xác định");
                break;
        }
    }
}
