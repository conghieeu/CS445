using UnityEngine;

namespace CuaHang.UI
{
    public class UIMovementController : UIPanel
    {
        private void Start()
        {
            if (GameSystem.CurrentPlatform == Platform.Standalone)
            {
                EnableCanvasGroup(false);
            }
            else if (GameSystem.CurrentPlatform == Platform.Android)
            {
                EnableCanvasGroup(true);
            }
        }

        private void OnEnable()
        {
            RaycastCursor.ActionEditItem += item => EnableCanvasGroup(!item);
        }

        private void OnDisable()
        {
            RaycastCursor.ActionEditItem -= item => EnableCanvasGroup(!item);
        }

    }
}
