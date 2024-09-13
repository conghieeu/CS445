using System;
using UnityEngine;

namespace CuaHang.UI
{
    public class UIErrorNetwork : UIPanel
    {
        private void Start()
        {
            GameSystem._OnCheckConnect += CheckInternet;
        }

        private void CheckInternet(bool value)
        {
            if (!value)
            {
                SetActiveContents(true);
                Time.timeScale = 0;
            }
            else
            {
                SetActiveContents(false);
                Time.timeScale = 1;
            }
        }
    }
}