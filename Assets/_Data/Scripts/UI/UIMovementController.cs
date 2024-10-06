using UnityEngine;

namespace CuaHang.UI
{
    public class UIMovementController : GameBehavior
    {
        RaycastCursor m_RaycastCursor;
        GameSystem m_GameSystem;

        private void Start()
        {
            m_GameSystem = FindFirstObjectByType<GameSystem>();
            m_RaycastCursor = FindFirstObjectByType<RaycastCursor>();

            m_RaycastCursor.ActionEditItem += item => SetActive(item != null);

            if (m_GameSystem.CurrentPlatform == Platform.Android)
            {
                SetActive(true);
            }
            else
            {
                SetActive(false);
            }
        } 

    }
}
