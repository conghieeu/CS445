using System;
using CuaHang;
using UnityEngine;
using UnityEngine.Events;

public class GameSettings : GameBehavior, ISaveData
{
    [Header("GAME SETTINGS")]
    [SerializeField] bool _isFullScreen;
    [SerializeField] int _qualityIndex;
    [SerializeField] float _masterVolume;
    [SerializeField] int _currentResolutionIndex;
    [SerializeField] Quaternion _camRotation;

    GameSettingsData _gameSettingsData;
    CameraControl _cameraControl;
    
    public static UnityAction<GameSettings> ActionDataChange;

    public bool IsFullScreen { get => _isFullScreen; set => _isFullScreen = value; }
    public int QualityIndex { get => _qualityIndex; set => _qualityIndex = value; }
    public float MasterVolume { get => _masterVolume; set => _masterVolume = value; }
    public int CurrentResolutionIndex { get => _currentResolutionIndex; set => _currentResolutionIndex = value; }
    public Quaternion CamRotation { get => _camRotation; set => _camRotation = value; }


    private void Awake()
    {
        _cameraControl = FindAnyObjectByType<CameraControl>();
    }

    #region SaveData
    public void SetVariables<T, V>(T data)
    {
        if (data is GameSettingsData gsData)
        {
            IsFullScreen = gsData.IsFullScreen;
            QualityIndex = gsData.QualityIndex;
            MasterVolume = gsData.MasterVolume;
            CurrentResolutionIndex = gsData.CurrentResolutionIndex;
            CamRotation = gsData.CamRotation;
            _gameSettingsData = gsData;
        }
    }

    public void LoadVariables()
    {
        ActionDataChange?.Invoke(this);
    }

    public void SaveData()
    {
        DataManager.Instance.GameData._gameSettingsData = GetData<GameSettingsData, object>();
    }

    public T GetData<T, D>()
    {
        GameSettingsData data = new GameSettingsData(
            IsFullScreen,
            QualityIndex,
            MasterVolume,
            CurrentResolutionIndex,
            _cameraControl ? _cameraControl.transform.rotation : _gameSettingsData.CamRotation);
        return (T)(object)(data);
    }
    #endregion
}
