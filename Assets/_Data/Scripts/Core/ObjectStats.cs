using CuaHang;
using Core;

public abstract class ObjectStats : HieuBehavior
{
    public SerializationAndEncryption _SAE => SerializationAndEncryption.Instance;

    protected virtual void OnEnable()
    {
        SerializationAndEncryption.ActionSaveData += OnSaveData;
        SerializationAndEncryption.ActionSetData += OnSetData;
        SerializationAndEncryption.ActionDataLoad += OnLoadData;
    }

    protected virtual void OnDisable()
    {
        SerializationAndEncryption.ActionSetData -= OnSetData;
        SerializationAndEncryption.ActionSaveData -= OnSaveData;
        SerializationAndEncryption.ActionDataLoad -= OnLoadData;
    }

    protected GameData GetGameData() => _SAE.GameData;

    private void OnSaveData()
    {
        if (this) SaveData();
    }

    private void OnSetData(GameData data)
    {
        if (this == null) return;

        if (_SAE.IsSaveFileExists == false)
        {
            LoadNewData();
            LoadNewGame();
        }
        else if (_SAE.GameData._gamePlayData.IsInitialized == false)
        {
            LoadNewGame();
        }
        else
        {
            OnSetData<GameData>(data);
        }
    }

    public abstract void OnSetData<T>(T data);

    public abstract void OnLoadData();

    /// <summary> Truyền giá trị save vào _gameData </summary>
    protected abstract void SaveData();

    /// <summary> Truyền giá trị save vào _gameData </summary>
    protected abstract void LoadNewData();

    /// <summary> sẽ load với các setting được chuẩn bị sẵn </summary>
    protected abstract void LoadNewGame();
}
