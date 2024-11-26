using System;
using System.Collections;
using System.Data.Common;
using Core;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : GameBehavior
{
    [Header("SCENES MANAGER")]
    [SerializeField] float transitionTime = 1f;
    DataManager dataManager;

    public UnityAction OnStartLoadScene;

    private void Start()
    {
        dataManager = DataManager.Instance;
    }

    public void LoadSceneMenuOnGameOver()
    {
        dataManager.OnStartNewGame();
        LoadSceneMenu();
    }

    public void LoadGameScene(GameScene gameScene)
    {
        Debug.Log($"Load scene {gameScene}");
        StartCoroutine(LoadSceneByGameScene(gameScene));
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