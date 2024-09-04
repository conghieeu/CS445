using Core; 
using UnityEngine;

public class BtnSave : MonoBehaviour
{
	public void SaveGame()
    {
        SerializationAndEncryption.Instance.SaveData();
    }
}

