using UnityEngine;
using System.Collections.Generic;


public class GameBehavior : MonoBehaviour
{
    [Header("GAME BEHAVIOR")]
    [SerializeField] private bool enableDebugLog;

    protected void In(object value)
    {
        if (enableDebugLog) Debug.Log(value, transform);
    }

    /// <summary> Đặt địa điểm Item này </summary> 
    public void SetLocation(Transform location)
    {
        transform.position = location.position;
        transform.rotation = location.rotation;
    }

    /// <summary> Hàm để tắt tất cả các đối tượng con của một Transform cha </summary>
    public void CloseAllChildren(Transform parent)
    {
        // Duyệt qua tất cả các đối tượng con của Transform cha
        foreach (Transform child in parent)
        {
            if (child != null) // Kiểm tra xem đối tượng con có khác null không
            {
                child.gameObject.SetActive(false); // Tắt đối tượng con
            }
        }
    }

    public T GetChild<T>(string name) where T : class
    {
        foreach (Transform child in transform)
        {
            if (child.name == name && child.GetComponent<T>() != null)
            {
                return child.GetComponent<T>();
            }
        }
        return default;
    }

    /// <summary> Xáo trộn các phần tử trong danh sách. </summary>
    public void Shuffle<T>(IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
