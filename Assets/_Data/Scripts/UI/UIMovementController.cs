using UnityEngine;

namespace CuaHang.UI
{
    public class UIMovementController : UIPanel
    {

        private void Start()
        {
            UIComputerScreen.e_IsOnEditComputer += active =>
            {
                SetActiveCanvasGroup(!active);
            };
        }
    }

}
