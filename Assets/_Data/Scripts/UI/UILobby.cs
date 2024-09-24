using UnityEngine;
using UnityEngine.UI;

public class UILobby : MonoBehaviour
{
    [SerializeField] Button _btnNewGame;
    [SerializeField] Button _btnTiepTuc; 
    [SerializeField] Button _btnThoatGame; // Thêm biến _btnThoatGame

    DataManager _SAE => DataManager.Instance;
    ScenesManager scenesManager;

    private void Start()
    {  
        _btnTiepTuc.gameObject.SetActive(_SAE.GameData._gamePlayData.IsInitialized);
        _btnNewGame.onClick.AddListener(OnClickNewGame);
        _btnThoatGame.onClick.AddListener(OnClickThoatGame); // Thêm AddListener cho _btnThoatGame
        scenesManager = ScenesManager.Instance;
    }

    public void OnClickNewGame()
    {
        _SAE.OnStartNewGame();
        scenesManager.LoadSceneDemo();
    }

    public void OnClickThoatGame() // Thêm hàm OnClickThoatGame
    {
        Application.Quit();
    }
}

