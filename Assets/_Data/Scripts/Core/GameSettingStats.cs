using System;
using CuaHang;
using UnityEngine;

public class GameSettingStats : ObjectStats
{
    [Header("GAME SETTING STATS")]
    [SerializeField] GameSettingsData _gameSettingData;

    CameraControl _cameraControl;

    public static event Action<GameSettingsData> _OnDataChange;

    protected override void Start()
    {
        base.Start();
        _cameraControl = CameraControl.Instance;
    }

    public override void LoadData<T>(T data)
    {
        _gameSettingData = (data as GameData)._gameSettingsData;

        // save data chưa từng được khởi tạo
        if (!_gameSettingData.IsInitialized) return;

        // set properties
        GameSettings gameSettings = GetComponent<GameSettings>();
        gameSettings.SetProperties(_gameSettingData);

        // Thong bao
        _OnDataChange?.Invoke(_gameSettingData);
    }

    protected override void SaveData()
    {
        _gameSettingData = GetData();
        GetGameData()._gameSettingsData = _gameSettingData;
    }

    protected override void LoadNoData()
    {
        SaveData();
    }

    public GameSettingsData GetData()
    {
        GameSettings gSet = GetComponent<GameSettings>();

        Quaternion camAngle = _gameSettingData.CamRotation;
        if (_cameraControl) camAngle = _cameraControl.CamshaftRotation;

        GameSettingsData data = new(
            true,
            gSet._IsFullScreen,
            gSet._QualityIndex,
            gSet._MasterVolume,
            gSet._CurrentResolutionIndex,
            camAngle);

        return data;
    }

}
