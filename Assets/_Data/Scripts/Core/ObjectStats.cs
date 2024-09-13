using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CuaHang;
using Core;
using UnityEngine;

public abstract class ObjectStats : HieuBehavior
{
    public SerializationAndEncryption _SAE => SerializationAndEncryption.Instance;

    protected virtual void OnEnable()
    {
        SerializationAndEncryption._OnDataLoaded += OnLoadData;
        SerializationAndEncryption._OnDataSaved += OnSaveData;
    }

    protected virtual void OnDisable()
    {
        SerializationAndEncryption._OnDataLoaded -= OnLoadData;
        SerializationAndEncryption._OnDataSaved -= OnSaveData;
    }

    protected GameData GetGameData() => _SAE.GameData;

    private void OnSaveData()
    {
        if (this != null && transform != null)
        {
            SaveData();
        }
    }

    private void OnLoadData(GameData data)
    {
        if (this != null && transform != null)
        { 
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
                LoadData(data);
            }
        }
    }

    public abstract void LoadData<T>(T data);
    protected abstract void SaveData();  /// <summary> Truyền giá trị save vào _gameData </summary>
    protected abstract void LoadNewData();  /// <summary> Truyền giá trị save vào _gameData </summary>
    protected abstract void LoadNewGame(); /// <summary> sẽ load với các setting được chuẩn bị sẵn </summary>

}
