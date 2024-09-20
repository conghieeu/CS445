using System;
using System.Collections.Generic;
using UnityEngine; 

[Serializable]
public class ItemData
{
    [SerializeField] string _id;
    [SerializeField] string _idItemParent; // id item cha
    [SerializeField] TypeID _typeID;
    [SerializeField] float _price;
    [SerializeField] Vector3 _position;
    [SerializeField] Quaternion _rotation;

    public ItemData(string id, string idItemParent, TypeID typeID, float price, Vector3 position, Quaternion rotation)
    {
        Id = id;
        IdItemParent = idItemParent;
        TypeID = typeID;
        Price = price;
        Position = position;
        Rotation = rotation;
    }

    public string Id { get => _id; set => _id = value; }
    public string IdItemParent { get => _idItemParent; set => _idItemParent = value; }
    public TypeID TypeID { get => _typeID; set => _typeID = value; }
    public float Price { get => _price; set => _price = value; }
    public Vector3 Position { get => _position; set => _position = value; }
    public Quaternion Rotation { get => _rotation; set => _rotation = value; }
}

[Serializable]
public class StaffData
{
    [SerializeField] string id;
    [SerializeField] TypeID typeID;
    [SerializeField] string name;
    [SerializeField] Vector3 position;

    public string Id { get => id; set => id = value; }
    public TypeID TypeID { get => typeID; set => typeID = value; }
    public string Name { get => name; set => name = value; }
    public Vector3 Position { get => position; set => position = value; }

    public StaffData(string id, TypeID typeID, string name, Vector3 position)
    {
        Id = id;
        TypeID = typeID;
        Name = name;
        Position = position;
    }
}

[Serializable]
public class CustomerData
{
    [SerializeField] string _id;
    [SerializeField] TypeID _typeID;
    [SerializeField] string _name;
    [SerializeField] float _totalPay;
    [SerializeField] bool _isNotNeedBuy; // Không cần mua gì nữa 
    [SerializeField] bool _playerConfirmPay; // Player xác nhận thanh toán
    [SerializeField] Vector3 _position;
    [SerializeField] Quaternion _rotation;

    public CustomerData(string id, TypeID typeID, string name, float totalPay, bool isNotNeedBuy, bool playerConfirmPay, Vector3 position, Quaternion rotation)
    {
        Id = id;
        TypeID = typeID;
        Name = name;
        TotalPay = totalPay;
        IsNotNeedBuy = isNotNeedBuy;
        PlayerConfirmPay = playerConfirmPay;
        Position = position;
        Rotation = rotation;
    }

    public string Id { get => _id; set => _id = value; }
    public TypeID TypeID { get => _typeID; set => _typeID = value; }
    public string Name { get => _name; set => _name = value; }
    public float TotalPay { get => _totalPay; set => _totalPay = value; }
    public bool IsNotNeedBuy { get => _isNotNeedBuy; set => _isNotNeedBuy = value; }
    public bool PlayerConfirmPay { get => _playerConfirmPay; set => _playerConfirmPay = value; }
    public Vector3 Position { get => _position; set => _position = value; }
    public Quaternion Rotation { get => _rotation; set => _rotation = value; }
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
public class PlayerData
{
    [SerializeField] float _currentMoney;
    [SerializeField] int _reputation;
    [SerializeField] Vector3 _position;
    [SerializeField] Quaternion _rotation;

    public PlayerData(float currentMoney, int reputation, Vector3 position, Quaternion rotation)
    {
        CurrentMoney = currentMoney;
        Reputation = reputation;
        Position = position;
        Rotation = rotation;
    }

    public float CurrentMoney { get => _currentMoney; set => _currentMoney = value; }
    public int Reputation { get => _reputation; set => _reputation = value; }
    public Vector3 Position { get => _position; set => _position = value; }
    public Quaternion Rotation { get => _rotation; set => _rotation = value; }
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