using UnityEngine;

namespace CuaHang.UI
{
    public class UIMovementController : GameBehavior
    {
        private void Start()
        {
            RaycastCursor.ActionEditItem += item => SetActive(item != null);

            if (GameSystem.CurrentPlatform == Platform.Android)
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
