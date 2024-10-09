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
        if(this == null || UserID == "")  return;
        
        string json = JsonUtility.ToJson(GameData);
        dbRef.Child("users").Child(UserID).SetRawJsonValueAsync(json);
    }

    public void LoadDataFn(string UserID)
    {
        StartCoroutine(LoadDataEnum(UserID));
    }

    IEnumerator LoadDataEnum(string UserID)
    {
        var serverData = dbRef.Child("users").Child(UserID).GetValueAsync();
        yield return new WaitUntil(predicate: () => serverData.IsCompleted);

        print("process is complete");

        DataSnapshot snapshot = serverData.Result;
        string jsonData = snapshot.GetRawJsonValue();

        if (jsonData != null)
        {
            print("server data found");

            m_DataManager.GameData = JsonUtility.FromJson<GameData>(jsonData);
            m_DataManager.SaveGameData();
        }
        else
        {
            print("no data found");
        }
    }
}
