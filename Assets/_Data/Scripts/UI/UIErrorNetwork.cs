using System;
using UnityEngine;

namespace CuaHang.UI
{
    public class UIErrorNetwork : GameBehavior
    {
        private void OnEnable()
        {
            GameSystem.ActionInternetConnect += CheckInternet;
        }

        private void OnDisable()
        {
            GameSystem.ActionInternetConnect -= CheckInternet;
        }

        private void CheckInternet(bool value)
        {
            //SetActive(value);
        }
    }
}