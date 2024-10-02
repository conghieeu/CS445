using System;
using System.Collections;
using UnityEngine;

public enum Platform
{
    Standalone,
    Android,
    iOS
}

public class GameSystem : GameBehavior
{
    [Header("GAME SYSTEM")]
    [SerializeField] Platform _platform; // nền tảng game này là gì ?
    public static bool _IsConnectedInternet; // kết nối với internet chưa
    public static Platform CurrentPlatform { get; private set; }

    public Platform _Platform { get => _platform; private set => _platform = value; }

    public static event Action<bool> ActionInternetConnect;

    private void Awake()
    {
        StartCoroutine(CheckInternetConnection());
        CurrentPlatform = _platform;
    } 

    IEnumerator CheckInternetConnection()
    {
        _IsConnectedInternet = Application.internetReachability != NetworkReachability.NotReachable;

        if (!_IsConnectedInternet)
        {
            Debug.LogWarning("Mất kết nối Internet!");
        }

        ActionInternetConnect?.Invoke(_IsConnectedInternet);

        // Kiểm tra lại sau mỗi 5 giây
        yield return new WaitForSecondsRealtime(5);
        StartCoroutine(CheckInternetConnection());
    }

}

