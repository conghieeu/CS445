using UnityEngine;

public interface ISaveData
{
    public void SetVariables<T, V>(T data); 
    public void LoadVariables();
    public void SaveData();
    public T GetData<T, V>(); /// <summary> T: Kiểu dữ liệu trả về, D: Kiểu dữ liệu muốn lấy </summary>
}

