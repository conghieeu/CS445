using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Threading.Tasks;
using Firebase.Auth;

/// <summary> Là GAMEDATA, chuỗi hoá và mã hoá lưu được nhiều loại dữ liệu của đối tượng </summary>
public class DataManager : Singleton<DataManager>
{
    [Header("DATA MANAGER")]
    [SerializeField] private GameData gameData;

    [SerializeField] string saveName = "/gameData.save";
    [SerializeField] string filePath;
    [SerializeField] bool serialize;
    [SerializeField] bool usingXML;
    [SerializeField] bool encrypt;

    FirebaseDataSaver firebaseDataSaver;

    public bool IsSaveFileExists { get; private set; }
    public GameData GameData { get => gameData; set => gameData = value; }
    public string PlayerID
    {
        get
        {
            return PlayerPrefs.GetString("PlayerID");
        }
        set
        {
            PlayerPrefs.SetString("PlayerID", value);
            GameData._playerProfileData.UserID = value;
        }
    }

    public static UnityAction<GameData> ActionSetData;
    public static UnityAction ActionSaveData;
    public static UnityAction ActionGameSaved;
    public static UnityAction ActionDataLoad;
    public static UnityAction ActionDataLoaded;

    protected override void Awake()
    {
        base.Awake();
        firebaseDataSaver = FindFirstObjectByType<FirebaseDataSaver>();

        SetDontDestroyOnLoad(true);

        filePath = Application.persistentDataPath + saveName;
        IsSaveFileExists = File.Exists(filePath);

        if (!IsSaveFileExists)
        {
            GameData._gamePlayData = new GamePlayData();
        }
        else
        {
            LoadFileData();
        }
        SceneManager.sceneLoaded += (scene, mode) => InitializeData();
    }

    /// <summary> Kiểm tra xem trò chơi có phải là trò chơi mới hay không </summary>
    public bool IsNewGame()
    {
        if (GameData._gamePlayData != null && GameData._gamePlayData.IsInitialized) return false;
        return true;
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void OnStartNewGame()
    {
        GameData._gamePlayData = new();
        File.WriteAllText(filePath, SerializeAndEncrypt(GameData));
    }

    public void SaveData()
    {
        ActionSaveData?.Invoke();
        SaveGameData();
        
        // save to firebase
        firebaseDataSaver.SaveDataFn(PlayerID);
    }

    public void SaveGameData()
    {
        File.WriteAllText(filePath, SerializeAndEncrypt(GameData)); // luu _gameData
        Debug.Log("Game data saved to: " + filePath);

        GameData._gamePlayData.IsInitialized = true;
    }

    public void InitializeData()
    {
        ActionSetData?.Invoke(GameData);
        ActionDataLoad?.Invoke();
        ActionDataLoaded?.Invoke();
    }

    public void LoadFileData()
    {
        if (File.Exists(filePath))
        {
            string stringData = File.ReadAllText(filePath);
            GameData = Deserialized(stringData);
            InitializeData();
            Debug.Log("Game data loaded from: " + filePath);
        }
        else
        {
            Debug.LogWarning("Save file not found in: " + filePath);
        }
    }

    /// <summary> Let's first serialize and encrypt.... </summary>
    private string SerializeAndEncrypt(GameData gameData)
    {
        string stringData = "";

        if (serialize)
        {
            if (usingXML)
                stringData = Utils.SerializeXML<GameData>(gameData);
            else
                stringData = JsonUtility.ToJson(gameData);
        }

        if (encrypt)
        {
            stringData = Utils.EncryptAES(stringData);
        }

        return stringData;
    }

    /// <summary> Now let's de-serialize and de-encrypt.... </summary>
    private GameData Deserialized(string stringData)
    {
        // giải mã hoá
        if (encrypt)
        {
            stringData = Utils.DecryptAES(stringData);
        }

        GameData gameData = new GameData();

        // đọc tuần tự hoá json hoặc xml
        if (serialize)
        {
            if (usingXML)
                gameData = Utils.DeserializeXML<GameData>(stringData);
            else
                gameData = JsonUtility.FromJson<GameData>(stringData);
        }
        return gameData;
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

        public static string DecryptAES(string text)
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
