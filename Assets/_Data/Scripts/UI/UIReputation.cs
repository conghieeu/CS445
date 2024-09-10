using UnityEngine;
using UnityEngine.UI;

public class UIReputation : MonoBehaviour
{
    public Text reputationText;              // Text UI để hiển thị danh tiếng
    public ReputationSystem reputationSystem; // Tham chiếu tới hệ thống danh tiếng

    void Start()
    {
        if (reputationSystem != null)
        {
            // Đăng ký lắng nghe sự kiện thay đổi danh tiếng
            reputationSystem.OnReputationChanged += UpdateReputationUI;
            UpdateReputationUI(reputationSystem.Reputation); // Cập nhật UI ban đầu
        }
    }

    // Hàm cập nhật giao diện khi danh tiếng thay đổi
    private void UpdateReputationUI(int newReputation)
    {
        reputationText.text = "Reputation: " + newReputation.ToString();
    }
}
