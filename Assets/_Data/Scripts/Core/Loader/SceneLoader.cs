using System;
using System.Collections;
using Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : GameBehavior
{
    [Header("SCENES MANAGER")]
    [SerializeField] float transitionTime = 1f;

    public UnityAction OnStartLoadScene;

    public void LoadGameScene(GameScene gameScene)
    {
        StartCoroutine(LoadSceneByGameScene(gameScene));
    }
    
    /// <summary> dùng khi đăng xuất </summary>
    public void RestartGame()
    {
        // Đặt lại các biến toàn cục hoặc singleton nếu cần
        ResetGlobalState();

        // Tải lại cảnh khởi đầu, giả sử cảnh khởi đầu có tên là "MainScene"
        LoadGameScene(GameScene.Loading);
    }

    private void ResetGlobalState()
    {
        throw new NotImplementedException();
    }

    // delay để hiện ứng chuyển scene chạy
    IEnumerator LoadSceneByGameScene(GameScene gameScene)
    {
        OnStartLoadScene?.Invoke();
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(gameScene.ToString());
    }

    public void LoadSceneMenu() => LoadGameScene(GameScene.Menu);
    public void LoadSceneDemo() => LoadGameScene(GameScene.Demo);

}

public enum GameScene
{
    DataHolder,
    Menu,
    Demo,
    Level_1,
    OptionsMenu,
    Loading,
}