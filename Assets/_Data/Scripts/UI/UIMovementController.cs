using UnityEngine;

namespace CuaHang.UI
{
    public class UIMovementController : UIPanel
    {
        protected override void Start()
        {
            base.Start();

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
            RaycastCursor.ActionEditItem += EnableCanvasGroup;
        }

        private void OnDisable()
        {
            RaycastCursor.ActionEditItem += EnableCanvasGroup;
        }

        private void EnableCanvasGroup(Item item)
        {
            base.EnableCanvasGroup(item != null);
        }
    }
}
