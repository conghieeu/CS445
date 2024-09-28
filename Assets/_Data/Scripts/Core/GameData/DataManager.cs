using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using UnityEngine.SceneManagement;

/// <summary> Là GAMEDATA, chuỗi hoá và mã hoá lưu được nhiều loại dữ liệu của đối tượng </summary>
public class DataManager : Singleton<DataManager>
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
    public static event Action ActionInitalizedData;

    protected override void Awake()
    {
        base.Awake();

        SetDontDestroyOnLoad(true);

        _filePath = Application.persistentDataPath + _saveName;

        IsSaveFileExists = File.Exists(_filePath);

        if (!IsSaveFileExists)
        {
            GameData._gamePlayData = new GamePlayData();
        }
        else
        {
            LoadFileData();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            InitializeData();
        };
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= (scene, mode) =>
        {
            InitializeData();
        };
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void OnStartNewGame()
    {
        GameData._gamePlayData = new();
        File.WriteAllText(_filePath, SerializeAndEncrypt(GameData));
    }

    public void SaveData()
    {
        ActionSaveData?.Invoke(); // thong bao cac doi tuong day du lieu vao _gameData
        File.WriteAllText(_filePath, SerializeAndEncrypt(GameData)); // luu _gameData
        Debug.Log("Game data saved to: " + _filePath);
        _gameData._gamePlayData.IsInitialized = true;
    }

    public void InitializeData()
    {
        ActionSetData?.Invoke(GameData);
        ActionDataLoad?.Invoke();
        ActionInitalizedData?.Invoke();
    }

    public void LoadFileData()
    {
        if (File.Exists(_filePath))
        {
            string stringData = File.ReadAllText(_filePath);

            GameData = Deserialized(stringData);
            InitializeData();
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
