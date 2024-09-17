using CuaHang;
using Core;

public abstract class ObjectStats : HieuBehavior
{
    public SerializationAndEncryption _SAE => SerializationAndEncryption.Instance;

    protected virtual void OnEnable()
    {
        SerializationAndEncryption.ActionSaveData += OnActionSaveData;
        SerializationAndEncryption.ActionSetData += OnActionSetData;
        SerializationAndEncryption.ActionDataLoad += OnActionLoadData;
    }

    protected virtual void OnDisable()
    {
        SerializationAndEncryption.ActionSetData -= OnActionSetData;
        SerializationAndEncryption.ActionSaveData -= OnActionSaveData;
        SerializationAndEncryption.ActionDataLoad -= OnActionLoadData;
    }

    protected GameData GetGameData() => _SAE.GameData;

    private void OnActionSaveData()
    {
        if (this) SaveData();
    }

    private void OnActionLoadData()
    {
        if(this) OnLoadData();
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
