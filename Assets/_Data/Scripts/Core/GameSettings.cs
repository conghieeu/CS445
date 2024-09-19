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

    public bool _IsFullScreen { get => _isFullScreen; set => _isFullScreen = value; }
    public int _QualityIndex { get => _qualityIndex; set => _qualityIndex = value; }
    public float _MasterVolume { get => _masterVolume; set => _masterVolume = value; }
    public int _CurrentResolutionIndex { get => _currentResolutionIndex; set => _currentResolutionIndex = value; }
    public Quaternion _CamRotation { get => _camRotation; set => _camRotation = value; }

    public static event Action<GameSettingsData> ActionDataChange;

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
    }
}
