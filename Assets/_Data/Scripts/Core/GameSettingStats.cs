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

    public override void LoadData<T>(T data)
    {
        if (data is GameData) _gameSettingData = (data as GameData)._gameSettingsData;
        else if (data is GameSettingsData) _gameSettingData = data as GameSettingsData;

        // set properties
        GetComponent<GameSettings>().SetProperties(_gameSettingData);

        // Thong bao
        _OnDataChange?.Invoke(_gameSettingData);
    }

    protected override void LoadNewGame() { }

    protected override void LoadNewData()
    {
        SaveData();
        LoadData(GetData()); // mục đích cập nhập và thông báo
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

}
