using CuaHang;
using Core;
using UnityEngine;

public abstract class ObjectStats : HieuBehavior
{
    public DataManager _SAE => DataManager.Instance;

    protected virtual void OnEnable()
    {
        DataManager.ActionSaveData += OnActionSaveData;
        DataManager.ActionSetData += OnActionSetData;
        DataManager.ActionDataLoad += OnActionLoadData;
    }

    protected virtual void OnDisable()
    {
        DataManager.ActionSetData -= OnActionSetData;
        DataManager.ActionSaveData -= OnActionSaveData;
        DataManager.ActionDataLoad -= OnActionLoadData;
    }

    protected GameData GetGameData() => _SAE.GameData;

    private void OnActionSaveData()
    {
        if (this) SaveData();
    }

    private void OnActionLoadData()
    {
        if (this) OnLoadData();
    }

    private void OnActionSetData(GameData data)
    {
        if (this == null) return;

        if (_SAE.IsSaveFileExists)
        {
            OnSetData<GameSettingsData>(data._gameSettingsData);
            OnSetData<PlayerProfileData>(data._playerProfileData);
        }

        if (_SAE.GameData._gamePlayData.IsInitialized)
        {
            OnSetData<GamePlayData>(data._gamePlayData);
        }
    }

    public abstract void OnSetData<T>(T data);

    public abstract void OnLoadData();

    /// <summary> Truyền giá trị save vào _gameData </summary>
    protected abstract void SaveData();
}
