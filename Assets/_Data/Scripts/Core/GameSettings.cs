using System;
using UnityEngine;

public class GameSettings : Singleton<GameSettings>
{
    [Header("GameSettings")]
    public GameSettingStats _gameSettingStats;

    [SerializeField] bool _isFullScreen;
    [SerializeField] int _qualityIndex;
    [SerializeField] float _masterVolume;
    [SerializeField] int _currentResolutionIndex;
    [SerializeField] Quaternion _camRotation;

    public bool IsFullScreen { get => _isFullScreen; set => _isFullScreen = value; }
    public int QualityIndex { get => _qualityIndex; set => _qualityIndex = value; }
    public float MasterVolume { get => _masterVolume; set => _masterVolume = value; }
    public int CurrentResolutionIndex { get => _currentResolutionIndex; set => _currentResolutionIndex = value; }
    public Quaternion CamRotation { get => _camRotation; set => _camRotation = value; }

    public static event Action<GameSettings> ActionDataChange;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetVariables(GameSettingsData data)
    {
        _isFullScreen = data.IsFullScreen;
        _qualityIndex = data.QualityIndex;
        _masterVolume = data.MasterVolume;
        _currentResolutionIndex = data.CurrentResolutionIndex;
        _camRotation = data.CamRotation;

        ActionDataChange?.Invoke(this);
    }
}
