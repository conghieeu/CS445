using System.Collections;
using UnityEngine;
using Firebase.Database;

public class FirebaseDataSaver : MonoBehaviour
{
    DatabaseReference dbRef;
    DataManager m_DataManager;
    EmailPassLogin m_EmailPassLogin;
    User m_User;

    public GameData GameData => m_DataManager.GameData;

    private void Start()
    {
        m_DataManager = FindFirstObjectByType<DataManager>();
        m_User = FindFirstObjectByType<User>();

        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SaveDataFn(string UserID)
    {
        if (UserID != "")
        {
            string json = JsonUtility.ToJson(GameData);
            dbRef.Child("users").Child(UserID).SetRawJsonValueAsync(json);
        }
    }

    public void SignUpNewData(string UserID)
    {
        if (UserID != "")
        {
            GameData newGameData = new GameData();
            string json = JsonUtility.ToJson(newGameData);
            dbRef.Child("users").Child(UserID).SetRawJsonValueAsync(json);
        }
    }

    public void LoadDataFn(string UserID)
    {
        StartCoroutine(LoadDataEnum(UserID));
    }

    IEnumerator LoadDataEnum(string UserID)
    {
        var serverData = dbRef.Child("users").Child(UserID).GetValueAsync();
        yield return new WaitUntil(predicate: () => serverData.IsCompleted);

        DataSnapshot snapshot = serverData.Result;
        string jsonData = snapshot.GetRawJsonValue();

        if (jsonData != null)
        {
            m_DataManager.GameData = JsonUtility.FromJson<GameData>(jsonData);
            m_DataManager.PlayerID = UserID;
            m_DataManager.InitializeData();
            m_DataManager.SaveData();
        }
        else
        {
            print("no data found");
        }
    }
}
