using Core; 
using UnityEngine;

public class BtnSave : MonoBehaviour
{
	public void SaveGame()
    {
        DataManager.Instance.SaveData();
    }
}

