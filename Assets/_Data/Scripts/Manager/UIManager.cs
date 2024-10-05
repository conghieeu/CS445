using System.Collections.Generic;
using CuaHang.UI;
using UnityEngine;

namespace CuaHang
{
    public class UIManager : GameBehavior
    {
        [Header("Main Popup")] [Tooltip("Những panel chính")]
        public List<GameObject> _mainPanel;

        // private void OnValidate()
        // {
        //     _mainPanel = new List<UIPanel>(FindObjectsByType<UIPanel>(FindObjectsSortMode.None));
        // }
    }
}
