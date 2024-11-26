using UnityEngine;

public class UIBangMoTa : MonoBehaviour
{
    private RectTransform rectTransform;
    private InputImprove inputImprove; // Tham chiếu đến lớp quản lý input

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        inputImprove = FindAnyObjectByType<InputImprove>(); // Tìm đối tượng quản lý input trong scene

        if (inputImprove == null)
        {
            Debug.LogError("Không tìm thấy InputImprove trong scene!");
        }
    }

    private void Update()
    {
        if (inputImprove != null)
        {
            FollowMouse();
        }
    }

    private void FollowMouse()
    {
        // Lấy vị trí chuột thông qua InputImprove
        Vector2 mousePosition = inputImprove.GameActionInput.UI.Point.ReadValue<Vector2>();

        // Đặt vị trí cho RectTransform
        rectTransform.position = mousePosition;
    }
}
