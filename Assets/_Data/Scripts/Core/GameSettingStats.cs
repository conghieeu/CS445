using System;
using CuaHang;
using UnityEngine;

public class GameSettingStats : ObjectStats
{
    [Header("GAME SETTING STATS")]
    [SerializeField] GameSettingsData _gameSettingData;

    CameraControl _cameraControl;

    public static event Action<GameSettingsData> _OnDataChange;

    private void Start()
    {
        _cameraControl = CameraControl.Instance;
    }

    public override void OnSetData<T>(T data)
    {
        Debug.Log("Set Data");
        if (data is GameSettingsData)
        {
            _gameSettingData = data as GameSettingsData;
        }
    }

    protected override void SaveData()
    {
        _gameSettingData = GetData();
        GetGameData()._gameSettingsData = _gameSettingData;
    }

    GameSettingsData GetData()
    {
        GameSettings gSet = GetComponent<GameSettings>();
        Quaternion camAngle = _gameSettingData.CamRotation;

        if (_cameraControl) camAngle = _cameraControl.CamshaftRotation;

        GameSettingsData data = new(
            gSet._IsFullScreen,
            gSet._QualityIndex,
            gSet._MasterVolume,
            gSet._CurrentResolutionIndex,
            camAngle);

        return data;
    }

    public override void OnLoadData()
    { 
        GameSettings gameSettings = GetComponent<GameSettings>();
        if (gameSettings)
        {
            gameSettings.SetVariables(_gameSettingData); 
            _OnDataChange?.Invoke(_gameSettingData);
        }
    }
}
