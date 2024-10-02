using UnityEngine;
using UnityEngine.UI;

public class UILobby : GameBehavior
{
    [SerializeField] Button _btnNewGame;
    [SerializeField] Button _btnTiepTuc;
    [SerializeField] Button _btnThoatGame; // Thêm biến _btnThoatGame

    DataManager dataManager => DataManager.Instance;
    ScenesManager scenesManager;

    private void Awake()
    {
        scenesManager = FindFirstObjectByType<ScenesManager>();
    }

    private void Start()
    {
        _btnTiepTuc.gameObject.SetActive(dataManager.IsNewGame() == false);
        _btnNewGame.onClick.AddListener(OnClickNewGame);
        _btnThoatGame.onClick.AddListener(OnClickThoatGame); // Thêm AddListener cho _btnThoatGame

    }

    public void OnClickNewGame()
    {
        dataManager.OnStartNewGame();
        scenesManager.LoadSceneDemo();
    }

    public void OnClickThoatGame() // Thêm hàm OnClickThoatGame
    {
        Application.Quit();
    }
}

