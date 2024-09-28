using System;
using System.Collections;
using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : Singleton<ScenesManager>
{
    [Header("SCENES MANAGER")]
    [SerializeField] float _transitionTime = 1f;

    public static event Action _OnLoadNextScene;

    public void LoadSceneDemo()
    {
        StartCoroutine(LoadLevel(GameScene.Demo));
    }

    public void LoadSceneMenu()
    {
        StartCoroutine(LoadLevel(GameScene.Menu));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        _OnLoadNextScene?.Invoke();
        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene(sceneName);
    }

}

