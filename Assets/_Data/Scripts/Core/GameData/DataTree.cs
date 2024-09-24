using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntityData
{
    [SerializeField] string id;
    [SerializeField] string name;
    [SerializeField] TypeID typeID;
    [SerializeField] Vector3 position;
    [SerializeField] Quaternion rotation;

    public EntityData(string id, string name, TypeID typeID, Vector3 position, Quaternion rotation)
    {
        Id = id;
        Name = name;
        TypeID = typeID;
        Position = position;
        Rotation = rotation;
    }

    public string Id { get => id; set => id = value; }
    public string Name { get => name; set => name = value; }
    public TypeID TypeID { get => typeID; set => typeID = value; }
    public Vector3 Position { get => position; set => position = value; }
    public Quaternion Rotation { get => rotation; set => rotation = value; }
}

[Serializable]
public class ItemData : EntityData
{
    [SerializeField] string _idItemParent; // id item cha
    [SerializeField] float _price;

    public ItemData(EntityData entityData, string idItemParent, float price)
    : base(entityData.Id, entityData.Name, entityData.TypeID, entityData.Position, entityData.Rotation)
    {
        IdItemParent = idItemParent;
        Price = price;
    }

    public ItemData(string id, string name, TypeID typeID, Vector3 position, Quaternion rotation, string idItemParent, float price)
    : base(id, name, typeID, position, rotation)
    {
        IdItemParent = idItemParent;
        Price = price;
    }

    public string IdItemParent { get => _idItemParent; set => _idItemParent = value; }
    public float Price { get => _price; set => _price = value; }
}

[Serializable]
public class StaffData : EntityData
{
    public StaffData(EntityData entityData)
    : base(entityData.Id, entityData.Name, entityData.TypeID, entityData.Position, entityData.Rotation)
    { 
    }
}

[Serializable]
public class CustomerData : EntityData
{
    [SerializeField] float _totalPay;
    [SerializeField] bool _isNotNeedBuy; // Không cần mua gì nữa 
    [SerializeField] bool _playerConfirmPay; // Player xác nhận thanh toán

    public CustomerData(EntityData entityData, bool isNotNeedBuy, bool playerConfirmPay)
    : base(entityData.Id, entityData.Name, entityData.TypeID, entityData.Position, entityData.Rotation)
    {
        IsNotNeedBuy = isNotNeedBuy;
        PlayerConfirmPay = playerConfirmPay;
    }

    public CustomerData(string id, string name, TypeID typeID, Vector3 position, Quaternion rotation, bool isNotNeedBuy, bool playerConfirmPay) : base(id, name, typeID, position, rotation)
    {
        IsNotNeedBuy = isNotNeedBuy;
        PlayerConfirmPay = playerConfirmPay;
    }

    public float TotalPay { get => _totalPay; set => _totalPay = value; }
    public bool IsNotNeedBuy { get => _isNotNeedBuy; set => _isNotNeedBuy = value; }
    public bool PlayerConfirmPay { get => _playerConfirmPay; set => _playerConfirmPay = value; }
}

[Serializable]
public class PlayerData : EntityData
{
    [SerializeField] float _currentMoney;
    [SerializeField] int _reputation;

    public PlayerData(EntityData entityData, float currentMoney, int reputation)
    : base(entityData.Id, entityData.Name, entityData.TypeID, entityData.Position, entityData.Rotation)
    {
        CurrentMoney = currentMoney;
        Reputation = reputation;
    }

    public PlayerData(string id, string name, TypeID typeID, Vector3 position, Quaternion rotation, float currentMoney, int reputation) 
    : base(id, name, typeID, position, rotation)
    {
        CurrentMoney = currentMoney;
        Reputation = reputation;
    }

    public float CurrentMoney { get => _currentMoney; set => _currentMoney = value; }
    public int Reputation { get => _reputation; set => _reputation = value; }
}

[Serializable]
public class GameSettingsData
{
    [SerializeField] bool _isFullScreen;
    [SerializeField] int _qualityIndex;
    [SerializeField] float _masterVolume;
    [SerializeField] int _currentResolutionIndex;
    [SerializeField] Quaternion _camRotation;

    public GameSettingsData(bool isFullScreen, int qualityIndex, float masterVolume, int currentResolutionIndex, Quaternion camRotation)
    {
        IsFullScreen = isFullScreen;
        QualityIndex = qualityIndex;
        MasterVolume = masterVolume;
        CurrentResolutionIndex = currentResolutionIndex;
        CamRotation = camRotation;
    }

    public Quaternion CamRotation { get => CamRotation1; set => CamRotation1 = value; }
    public bool IsFullScreen { get => _isFullScreen; set => _isFullScreen = value; }
    public int QualityIndex { get => _qualityIndex; set => _qualityIndex = value; }
    public float MasterVolume { get => _masterVolume; set => _masterVolume = value; }
    public int CurrentResolutionIndex { get => _currentResolutionIndex; set => _currentResolutionIndex = value; }
    public Quaternion CamRotation1 { get => _camRotation; set => _camRotation = value; }
}

[Serializable]
public class GamePlayData
{
    [SerializeField] bool _isInitialized;
    [SerializeField] PlayerData _playerData; // dữ liệu nhân vật người chơi
    [SerializeField] List<CustomerData> _customersData;
    [SerializeField] List<StaffData> _staffsData;
    [SerializeField] List<ItemData> _itemsData;

    public GamePlayData()
    { 
        _isInitialized = false;
        _customersData = new();
        _staffsData = new();
        _itemsData = new();
    }

    public bool IsInitialized { get => _isInitialized; set => _isInitialized = value; }
    public PlayerData PlayerData { get => _playerData; set => _playerData = value; }
    public List<CustomerData> CustomersData { get => _customersData; set => _customersData = value; }
    public List<StaffData> StaffsData { get => _staffsData; set => _staffsData = value; }
    public List<ItemData> ItemsData { get => _itemsData; set => _itemsData = value; }
}

[Serializable]
public class PlayerProfileData
{
    [SerializeField] string _userName;
    [SerializeField] float _highestMoney;
    [SerializeField] float _playTime; // Tổng thời gian chơi tính bằng phút 

    public PlayerProfileData(string userName, float highestMoney, float playTime)
    {
        UserName = userName;
        HighestMoney = highestMoney;
        PlayTime = playTime;
    }

    public string UserName { get => _userName; set => _userName = value; }
    public float HighestMoney { get => _highestMoney; set => _highestMoney = value; }
    public float PlayTime { get => _playTime; set => _playTime = value; }
}

[Serializable]
public class GameData
{
    public PlayerProfileData _playerProfileData;
    public GameSettingsData _gameSettingsData;
    public GamePlayData _gamePlayData;
}