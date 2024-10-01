using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    /// <summary> Loading khi mới khởi động game </summary>
    public class LoadingScene : GameBehavior
    {
        private AsyncOperation m_async; // Bien luu doi tuong AsyncOperation
        public static Action<float> OnLoading; // Su kien hook

        private void Start()
        {
            m_async = SceneManager.LoadSceneAsync(GameScene.DataHolder.ToString(), LoadSceneMode.Additive);
            // m_async = SceneManager.LoadSceneAsync(GameScene.OptionsMenu.ToString(), LoadSceneMode.Additive);
        }

        private void Update()
        {
            OnLoading?.Invoke(m_async.progress);

            // Neu scene DataHolder thuc su duoc load het
            if (m_async.isDone)
            {
                SceneManager.LoadScene(GameScene.Menu.ToString());
            }
        }
    }
}
