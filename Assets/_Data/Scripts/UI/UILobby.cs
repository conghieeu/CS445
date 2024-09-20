using Core;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : MonoBehaviour
{
    [SerializeField] Button _btnTiepTuc; 

    DataManager _SAE => DataManager.Instance;

    private void Start()
    {  
        _btnTiepTuc.gameObject.SetActive(_SAE.GameData._gamePlayData.IsInitialized);
    }

    public void OnBtnChoiMoi()
    {
        _SAE.OnStartNewGame();
    }
}

