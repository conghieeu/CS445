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

        DataManager.ActionGameSaved += SaveDataFn;
    }

    public void SaveDataFn()
    {
        if(this == null)  return;
        
        string json = JsonUtility.ToJson(GameData);
        dbRef.Child("users").Child(GetUserID()).SetRawJsonValueAsync(json);
    }

    public void LoadDataFn()
    {
        StartCoroutine(LoadDataEnum());
    }

    IEnumerator LoadDataEnum()
    {
        var serverData = dbRef.Child("users").Child(GetUserID()).GetValueAsync();
        yield return new WaitUntil(predicate: () => serverData.IsCompleted);

        print("process is complete");

        DataSnapshot snapshot = serverData.Result;
        string jsonData = snapshot.GetRawJsonValue();

        if (jsonData != null)
        {
            print("server data found");

            m_DataManager.GameData = JsonUtility.FromJson<GameData>(jsonData);
        }
        else
        {
            print("no data found");
        }
    }

    public string GetUserID()
    {
        return m_User.UserID;
    }
}
