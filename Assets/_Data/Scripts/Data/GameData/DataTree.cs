using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntityData
{
    public string Id;
    public string Name;
    public bool IsDestroyed;
    public TypeID TypeID;
    public Vector3 Position;
    public Quaternion Rotation;

    public EntityData(string id, string name, bool isDestroyed, TypeID typeID, Vector3 position, Quaternion rotation)
    {
        Id = id;
        Name = name;
        IsDestroyed = isDestroyed;
        TypeID = typeID;
        Position = position;
        Rotation = rotation;
    }
}

[Serializable]
public class ItemData : EntityData
{
    [SerializeField] string _idItemParent; // id item cha
    [SerializeField] float _price;

    public ItemData(EntityData entityData, string idItemParent, float price)
    : base(entityData.Id, entityData.Name, entityData.IsDestroyed, entityData.TypeID, entityData.Position, entityData.Rotation)
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
    : base(entityData.Id, entityData.Name, entityData.IsDestroyed, entityData.TypeID, entityData.Position, entityData.Rotation)
    {
    }
}

[Serializable]
public class CustomerData : EntityData
{
    [SerializeField] float _totalPay;
    [SerializeField] List<TypeID> _listItemBuy;
    [SerializeField] bool _playerConfirmPay; // Player xác nhận thanh toán

    public CustomerData(EntityData entityData, float totalPay, List<TypeID> listItemBuy, bool playerConfirmPay)
    : base(entityData.Id, entityData.Name, entityData.IsDestroyed, entityData.TypeID, entityData.Position, entityData.Rotation)
    {
        TotalPay = totalPay;
        ListItemBuy = listItemBuy;
        PlayerConfirmPay = playerConfirmPay;
    }

    public float TotalPay { get => _totalPay; set => _totalPay = value; }
    public List<TypeID> ListItemBuy { get => _listItemBuy; set => _listItemBuy = value; }
    public bool PlayerConfirmPay { get => _playerConfirmPay; set => _playerConfirmPay = value; }
}

[Serializable]
public class PlayerData : EntityData
{
    [SerializeField] float _currentMoney;
    [SerializeField] int _reputation;


    public PlayerData(EntityData entityData, float currentMoney, int reputation)
    : base(entityData.Id, entityData.Name, entityData.IsDestroyed, entityData.TypeID, entityData.Position, entityData.Rotation)
    {
        CurrentMoney = currentMoney;
        Reputation = reputation;
    }

    public PlayerData(string id, string name, bool isDestroyed, TypeID typeID, Vector3 position, Quaternion rotation, float currentMoney, int reputation)
    : base(id, name, isDestroyed, typeID, position, rotation)
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

    public GameSettingsData()
    {
        IsFullScreen = new();
        QualityIndex = new();
        MasterVolume = new();
        CurrentResolutionIndex = new();
        CamRotation = new();
    }

    public GameSettingsData(bool isFullScreen, int qualityIndex, float masterVolume, int currentResolutionIndex, Quaternion camRotation)
    {
        IsFullScreen = isFullScreen;
        QualityIndex = qualityIndex;
        MasterVolume = masterVolume;
        CurrentResolutionIndex = currentResolutionIndex;
        CamRotation = camRotation;
    }

    public Quaternion CamRotation { get => _camRotation; set => _camRotation = value; }
    public bool IsFullScreen { get => _isFullScreen; set => _isFullScreen = value; }
    public int QualityIndex { get => _qualityIndex; set => _qualityIndex = value; }
    public float MasterVolume { get => _masterVolume; set => _masterVolume = value; }
    public int CurrentResolutionIndex { get => _currentResolutionIndex; set => _currentResolutionIndex = value; }

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
    public string UserID;
    public string UserName;
    public float HighestMoney;
    public float PlayTime;

        public PlayerProfileData()
    {
        UserID = "";
        UserName = "";
        HighestMoney = new();
        PlayTime = new();
    }

    public PlayerProfileData(string userId, string userName, float highestMoney, float playTime)
    {
        UserID = userId;
        UserName = userName;
        HighestMoney = highestMoney;
        PlayTime = playTime;
    }
}

[Serializable]
public class GameData
{
    public PlayerProfileData _playerProfileData;
    public GameSettingsData _gameSettingsData;
    public GamePlayData _gamePlayData;

    public GameData()
    {
        _playerProfileData = new PlayerProfileData();
        _gameSettingsData = new GameSettingsData();
        _gamePlayData = null;
    }
}