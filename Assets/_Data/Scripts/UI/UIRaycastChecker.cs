using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

namespace CuaHang.UI
{
    public class UIRaycastChecker : GameBehavior
    {
       
        GraphicRaycaster graphicRaycaster;
        InputImprove m_InputImprove;

        private void Awake()
        {
            m_InputImprove = FindAnyObjectByType<InputImprove>();
            graphicRaycaster = GetComponent<GraphicRaycaster>();
        }

        /// <summary> Kiểm tra xem người dùng có chạm vào UI hay không </summary>
        public bool IsPointerOverCanvas()
        {
            if (graphicRaycaster == null) return false;

            PointerEventData eventData = new PointerEventData(EventSystem.current);

            eventData.position = m_InputImprove.MousePosition.action.ReadValue<Vector2>();

            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, results);

            return results.Count == 0;
        }

        public bool IsClickOnUI()
        {

            // Check if the mouse was clicked over a UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Clicked on the UI");
                return true;
            }

            return false;
        }

        public bool IsPointerOverUIObject()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}
