using TMPro;
using UnityEngine;

public class UIPlayerProfile : MonoBehaviour
{
    public TMP_InputField InfUserName;
    public TextMeshProUGUI TextUserID;
    public TextMeshProUGUI TextHighestMoney;
    public TextMeshProUGUI TextTimePlay;

    User m_User;

    void Start()
    {
        m_User = FindFirstObjectByType<User>();

        m_User.OnDataChange += SetVariables;

        SetVariables(m_User);
    }

    private void FixedUpdate()
    {
        m_User.UserName = InfUserName.text;
    }

    private void SetVariables(User user)
    {
        InfUserName.text = user.UserName;
        TextUserID.text = user.UserID;
        TextHighestMoney.text = user.HighestMoney.ToString("F0");
        TextTimePlay.text = user.PlayTime.ToString("F0");
    }
}
