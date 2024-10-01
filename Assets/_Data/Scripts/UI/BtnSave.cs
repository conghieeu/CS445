using Core; 
using UnityEngine;

public class BtnSave : GameBehavior
{
	public void SaveGame()
    {
        DataManager.Instance.SaveData();
    }
}

