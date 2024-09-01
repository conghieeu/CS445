using System;
using CuaHang;
using UnityEngine;

public class GameSettingStats : ObjectStats
{
    [Header("GAME SETTING STATS")]
    [SerializeField] GameSettingsData _gameSettingData;

    public static event Action<GameSettingsData> _OnDataChange;

    public override void LoadData<T>(T data)
    {
        _gameSettingData = (data as GameData)._gameSettingsData;

        // set properties
        GameSettings gameSettings = GetComponent<GameSettings>();
        gameSettings.SetProperties(_gameSettingData);

        // Thong bao
        _OnDataChange?.Invoke(_gameSettingData);
    }

    protected override void SaveData()
    {
        GameSettings gameSetting = GetComponent<GameSettings>();
        _gameSettingData._camRotation = gameSetting._CamRotation;
        _gameSettingData._currentResolutionIndex = gameSetting._CurrentResolutionIndex;
        _gameSettingData._isFullScreen = gameSetting._IsFullScreen;
        _gameSettingData._qualityIndex = gameSetting._QualityIndex;
        _gameSettingData._masterVolume = gameSetting._MasterVolume;

        GetGameData()._gameSettingsData = _gameSettingData;
    }
}
