using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CuaHang
{
    public class GameManager : Singleton<GameManager>
    {
        User _playerManager;
        PlayerCtrl _playerCtrl; 
    }
}