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
                CameraControl.OnEditItem += item => EnableCanvasGroup(!item);
            }

        }
    }

}
