using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CuaHang;
using Core;
using UnityEngine;

public abstract class ObjectStats : HieuBehavior
{
    protected virtual void Start() // root thì mới cần bắt sự kiện
    {
        // On start scene
        if (SerializationAndEncryption.IsDataLoaded)
        {
            LoadData(GetGameData());
        }
        else  // Trường hợp file save chưa từng được tạo ra
        {
            LoadNoData();
        }

        // load event
        SerializationAndEncryption._OnDataLoaded += gameData =>
        {
            if (this != null && transform != null)
            {
                LoadData(gameData);
            }
        };

        // save event
        SerializationAndEncryption._OnDataSaved += () =>
        {
            if (this != null && transform != null)
            {
                SaveData();
            }
        };
    }

    protected GameData GetGameData() 
    {
        if(SerializationAndEncryption.Instance == null)
        {
            Debug.LogWarning("Không có GameData");
            return null;
        }
        return SerializationAndEncryption.Instance._gameData;
    }

    //-----------ABSTRACT------------
    /// <summary> Truyền giá trị save vào _gameData </summary>
    protected abstract void SaveData();
    protected abstract void LoadNoData(); // sẽ load với các setting được chuẩn bị sẵn
    public abstract void LoadData<T>(T data);

}
