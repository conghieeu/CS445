using UnityEngine;
using UnityEngine.EventSystems;

public class UIMoTa : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject descriptionTable; // Bảng mô tả (sẵn trong scene)
    private InputImprove inputImprove;

    private void Awake()
    {
        if (descriptionTable == null)
        {
            Debug.LogError("Description Table chưa được gán trong Inspector!");
        }

        inputImprove = FindAnyObjectByType<InputImprove>();
        if (inputImprove == null)
        {
            Debug.LogError("Không tìm thấy InputImprove trong scene!");
        }
    }

    private void Update()
    {
        if (descriptionTable.activeSelf && inputImprove != null)
        {
            FollowMouse();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (descriptionTable != null)
        {
            descriptionTable.SetActive(true); // Hiển thị bảng mô tả
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (descriptionTable != null)
        {
            descriptionTable.SetActive(false); // Ẩn bảng mô tả
        }
    }

    private void FollowMouse()
    {
        // Lấy vị trí chuột từ InputImprove
        Vector2 mousePosition = inputImprove.GameActionInput.UI.Point.ReadValue<Vector2>();

        // Đặt vị trí bảng mô tả theo chuột (nếu thuộc UI, dùng RectTransform)
        RectTransform rectTransform = descriptionTable.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.position = mousePosition;
        }
        else
        {
            Debug.LogError("Description Table không có RectTransform!");
        }
    }
}
