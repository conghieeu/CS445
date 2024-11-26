using UnityEngine;

namespace CuaHang
{
    public class GameManager : GameBehavior
    {
        public GameObject PanelGameOver;
        public GameObject PanelWin;
        DataManager dataManager;
        PlayerCtrl playerCtrl;

        private void Start()
        {
            playerCtrl = FindFirstObjectByType<PlayerCtrl>();
            dataManager = FindFirstObjectByType<DataManager>();
        }

        private void FixedUpdate()
        {
            OnGameOver();
        }

        private void OnWin()
        {
            if (playerCtrl.Money >= 1000000)
            {
                Time.timeScale = 0;
                PanelWin.SetActive(true);
                SaveGame();
            }
        }

        private void OnGameOver()
        {
            if (playerCtrl.Reputation <= 0)
            {
                Time.timeScale = 0;
                PanelGameOver.SetActive(true);
                SaveGame();
            }
        }

        private void SaveGame()
        {
            dataManager.SaveData();
        }
    }
}