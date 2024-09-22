using System;
using CuaHang;
using UnityEngine;

public class GameSettingStats : ObjectStats
{
    [Header("GAME SETTING STATS")]
    [SerializeField] GameSettingsData _gameSettingData;

    CameraControl _cameraControl => CameraControl.Instance;


    private void FixedUpdate()
    {
        SaveData();
    }

    public override void OnSetData<T>(T data)
    {
        if (data is GameSettingsData)
        {
            _gameSettingData = data as GameSettingsData;
        }
    }

    public override void OnLoadData()
    {
        GameSettings gameSettings = GetComponent<GameSettings>();
        if (gameSettings)
        {
            gameSettings.SetVariables(_gameSettingData);
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
            gSet.IsFullScreen,
            gSet.QualityIndex,
            gSet.MasterVolume,
            gSet.CurrentResolutionIndex,
            camAngle);

        return data;
    }

}
