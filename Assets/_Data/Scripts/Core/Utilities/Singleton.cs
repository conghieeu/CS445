using UnityEngine;

public class Singleton<T> : GameBehavior where T : GameBehavior
{
    [Header("SINGLETON")]
    public bool _dontDestroyOnLoad;

    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    Debug.LogWarning($"Lỗi singleton: đối tượng {typeof(T).Name} không tồn tại");
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            if (_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            if(instance && instance.GetComponent<Singleton<T>>()._dontDestroyOnLoad)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetDontDestroyOnLoad(bool value)
    {
        _dontDestroyOnLoad = value;
        if (_dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
