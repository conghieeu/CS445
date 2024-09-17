using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CuaHang
{
    public class GameManager : Singleton<GameManager>
    {
        PlayerManager _playerManager;
        PlayerCtrl _playerCtrl;
        // Các hệ thống khác như UIManager, AudioManager, v.v.

        private void OnEnable()
        {
            SerializationAndEncryption.ActionSaveData += SaveGame;
        }

        private void OnDisable()
        {
            SerializationAndEncryption.ActionSaveData -= SaveGame;
        }

        protected override void Awake()
        {
            base.Awake();
            // Khởi tạo các hệ thống khác nếu cần
        }

        void Start()
        {
            _playerManager = FindObjectOfType<PlayerManager>();
            // Khởi tạo các hệ thống khác
        }

        // Quản lý trạng thái trò chơi
        public void StartGame()
        {
            // Logic để bắt đầu trò chơi
            Debug.Log("Game Started");
        }

        public void PauseGame()
        {
            // Logic để tạm dừng trò chơi
            Time.timeScale = 0;
            Debug.Log("Game Paused");
        }

        public void ResumeGame()
        {
            // Logic để tiếp tục trò chơi
            Time.timeScale = 1;
            Debug.Log("Game Resumed");
        }

        public void EndGame()
        {
            // Logic để kết thúc trò chơi
            Debug.Log("Game Ended");
        }

        // Quản lý cấp độ
        public void LoadLevel(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }

        public void ReloadCurrentLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadNextLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }

        // Quản lý điểm số và thành tích
        public void AddPlayerMoney(float amount)
        {
            _playerCtrl.AddMoney(amount);
        }

        public void UpdatePlayerReputation(int reputation)
        {
            _playerCtrl.UpdateReputation(reputation);
        }
        public void LoadGame()
        {
            // Logic để tải trò chơi
            Debug.Log("Game Loaded");
        }

        public void SaveGame()
        {
            SerializationAndEncryption.Instance.GameData._gamePlayData.IsInitialized = true;
        }

    }
}