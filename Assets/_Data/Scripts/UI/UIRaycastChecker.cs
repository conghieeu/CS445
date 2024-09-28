using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace CuaHang.UI
{
    public class UIRaycastChecker : GameBehavior
    {
        GraphicRaycaster _graphicRaycaster;

        private void OnValidate()
        {
            _graphicRaycaster = GetComponent<GraphicRaycaster>();
        }

        /// <summary> Kiểm tra xem người dùng có chạm vào UI hay không </summary>
        public bool IsPointerOverUI()
        { 
            if (_graphicRaycaster == null) return false;

            PointerEventData eventData = new PointerEventData(EventSystem.current);
            
            if (Input.touchCount > 0)
            {
                eventData.position = Input.GetTouch(0).position;
            }
            else if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
            {
                eventData.position = Input.mousePosition;
            }
            else
            {
                return false;
            }

            List<RaycastResult> results = new List<RaycastResult>();
            _graphicRaycaster.Raycast(eventData, results);

            return results.Count > 0;
        }
    }
}
