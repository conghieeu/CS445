using CuaHang;
using CuaHang.Pooler;
using UnityEngine;

public class EntityStats : GameBehavior
{
    DataManager _dataManager => DataManager.Instance;
    protected virtual void OnEnable()
    {
        if (this == null) return;
        DataManager.ActionSetData += SetData;
        DataManager.ActionSaveData += SaveData;
        DataManager.ActionDataLoad += LoadData;
    }

    protected virtual void OnDisable()
    {
        if (this == null) return;
        DataManager.ActionSetData -= SetData;
        DataManager.ActionSaveData -= SaveData;
        DataManager.ActionDataLoad -= LoadData;
    }

    /// <summary> Input(GameData) </summary>
    public virtual void SetData<T>(T data)
    {
        if (GetComponent<ISaveData>() == null)
        {
            Debug.Log("Khong co ISaveData", transform);
            return;
        }

        if (data is GameData gameData)
        {
            if (_dataManager.IsSaveFileExists)
            {
                GetComponent<ISaveData>().SetVariables<PlayerProfileData, object>(gameData._playerProfileData);
                GetComponent<ISaveData>().SetVariables<GameSettingsData, object>(gameData._gameSettingsData);
            }
            if (_dataManager.GameData._gamePlayData.IsInitialized)
            {
                GetComponent<ISaveData>().SetVariables<GamePlayData, object>(gameData._gamePlayData);
            }
        }
    }

    public virtual void LoadData()
    {
        GetComponent<ISaveData>().LoadVariables();
    }

    /// <summary> Tin hieu save game tu DataManager </summary>
    public virtual void SaveData()
    {
        GetComponent<ISaveData>().SaveData();
    }

}
