using Core;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : MonoBehaviour
{
    [SerializeField] Button _btnTiepTuc; 

    SerializationAndEncryption _SAE => SerializationAndEncryption.Instance;

    private void Start()
    {  
        _btnTiepTuc.gameObject.SetActive(_SAE.GameData._gamePlayData.IsInitialized);
    }

    public void OnBtnChoiMoi()
    {
        _SAE.OnStartNewGame();
    }
}

