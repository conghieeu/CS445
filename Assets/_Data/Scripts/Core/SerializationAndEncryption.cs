using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

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

namespace Core
{
    /// <summary> Là GAMEDATA, chuỗi hoá và mã hoá lưu được nhiều loại dữ liệu của đối tượng </summary>
    public class SerializationAndEncryption : Singleton<SerializationAndEncryption>
    {
        [Header("Serialization And Encryption")]
        [SerializeField] GameData _gameData = new();
        [SerializeField] bool _isSaveFileExists;
        [SerializeField] string _saveName = "/gameData.save";
        [SerializeField] string _filePath;

        [Header("SerializationAndEncryption")]
        [SerializeField] bool _serialize;
        [SerializeField] bool _usingXML;
        [SerializeField] bool _encrypt;

        public bool IsSaveFileExists { get => _isSaveFileExists; private set => _isSaveFileExists = value; }
        public GameData GameData { get => _gameData; set => _gameData = value; }

        public static event Action ActionSaveData;
        public static event Action<GameData> ActionSetData;
        public static event Action ActionDataLoad;

        protected override void Awake()
        {
            base.Awake();
            SetDontDestroyOnLoad(true);
            _filePath = Application.persistentDataPath + _saveName;
            LoadData();

            IsSaveFileExists = File.Exists(_filePath);

            if (!IsSaveFileExists)
            {
                GameData._gamePlayData = new();
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                LoadData();
            };
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= (scene, mode) =>
            {
                LoadData();
            };
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }

        public void StartNewGame()
        {
            GameData._gamePlayData = new();
            SaveData();
        }

        public void SaveData()
        {
            ActionSaveData?.Invoke();
            File.WriteAllText(_filePath, SerializeAndEncrypt(GameData));
            Debug.Log("Game data saved to: " + _filePath);
        }

        public void LoadData()
        {
            if (File.Exists(_filePath))
            {
                string stringData = File.ReadAllText(_filePath);

                GameData = Deserialized(stringData);
                ActionSetData?.Invoke(GameData);
                ActionDataLoad?.Invoke();

                Debug.Log("Game data loaded from: " + _filePath);
            }
            else
            {
                Debug.LogWarning("Save file not found in: " + _filePath);
            }
        }

        /// <summary> Let's first serialize and encrypt.... </summary>
        private string SerializeAndEncrypt(GameData gameData)
        {
            string stringData = "";

            if (_serialize)
            {
                if (_usingXML)
                    stringData = Utils.SerializeXML<GameData>(gameData);
                else
                    stringData = JsonUtility.ToJson(gameData);
            }

            if (_encrypt)
            {
                stringData = Utils.EncryptAES(stringData);
            }

            return stringData;
        }

        /// <summary> Now let's de-serialize and de-encrypt.... </summary>
        private GameData Deserialized(string stringData)
        {
            // giải mã hoá
            if (_encrypt)
            {
                stringData = Utils.DecryptAES(stringData);
            }

            GameData gameData = new GameData();

            // đọc tuần tự hoá json hoặc xml
            if (_serialize)
            {
                if (_usingXML)
                    gameData = Utils.DeserializeXML<GameData>(stringData);
                else
                    gameData = JsonUtility.FromJson<GameData>(stringData);
            }
            return gameData;
        }
    }

    public static class Utils
    {
        public static string SerializeXML<T>(System.Object inputData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, inputData);
                    return sww.ToString();
                }
            }
        }

        public static T DeserializeXML<T>(string data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var sww = new StringReader(data))
            {
                using (XmlReader reader = XmlReader.Create(sww))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
        }

        static byte[] ivBytes = new byte[16]; // Generate the iv randomly and send it along with the data, to later parse out
        static byte[] keyBytes = new byte[16]; // Generate the key using a deterministic algorithm rather than storing here as a variable

        static void GenerateIVBytes()
        {
            System.Random rnd = new System.Random();
            rnd.NextBytes(ivBytes);
        }

        const string nameOfGame = "HieuDev";
        static void GenerateKeyBytes()
        {
            int sum = 0;
            foreach (char curChar in nameOfGame)
                sum += curChar;

            System.Random rnd = new System.Random(sum);
            rnd.NextBytes(keyBytes);
        }

        public static string EncryptAES(string data)
        {
            GenerateIVBytes();
            GenerateKeyBytes();

            SymmetricAlgorithm algorithm = Aes.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(keyBytes, ivBytes);
            byte[] inputBuffer = Encoding.Unicode.GetBytes(data);
            byte[] outputBuffer = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

            string ivString = Encoding.Unicode.GetString(ivBytes);
            string encryptedString = Convert.ToBase64String(outputBuffer);

            return ivString + encryptedString;
        }

        public static string DecryptAES(this string text)
        {
            GenerateIVBytes();
            GenerateKeyBytes();

            int endOfIVBytes = ivBytes.Length / 2;  // Half length because unicode characters are 64-bit width

            string ivString = text.Substring(0, endOfIVBytes);
            byte[] extractedivBytes = Encoding.Unicode.GetBytes(ivString);

            string encryptedString = text.Substring(endOfIVBytes);

            SymmetricAlgorithm algorithm = Aes.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(keyBytes, extractedivBytes);
            byte[] inputBuffer = Convert.FromBase64String(encryptedString);
            byte[] outputBuffer = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

            string decryptedString = Encoding.Unicode.GetString(outputBuffer);

            return decryptedString;
        }
    }

}