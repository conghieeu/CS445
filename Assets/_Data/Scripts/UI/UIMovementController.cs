using UnityEngine;

namespace CuaHang.UI
{
    public class UIMovementController : UIPanel
    {
        private void Start()
        {
            if (GameSystem.CurrentPlatform == Platform.Standalone)
            {
                SetActiveCanvasGroup(false);
            }
            else if (GameSystem.CurrentPlatform == Platform.Android)
            {
                CameraControl._EventOnEditItem += item => SetActiveCanvasGroup(!item);
            }

        }
    }

}
