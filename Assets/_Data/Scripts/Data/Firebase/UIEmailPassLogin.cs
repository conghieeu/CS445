using System.Collections;
using UnityEngine;
using TMPro;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase;
using UnityEngine.UI;

public class UIEmailPassLogin : GameBehavior
{
    [Header("Login")]
    public TMP_Text LoginEmail;
    public TMP_Text loginPassword;
    public Button BtnLogin;

    [Header("Sign up")]
    public TMP_Text SignUpEmail;
    public TMP_Text SignUpPassword;
    public TMP_Text SignUpPasswordConfirm;
    public Button BtnSignUp;

    [Header("Extra")]
    public GameObject PanelFirebaseConnectionFail;
    public GameObject PanelLoading;
    public GameObject LoginSuccess;

    EmailPassLogin m_EmailPassLogin;

    private void Start()
    {
        m_EmailPassLogin = FindFirstObjectByType<EmailPassLogin>();

        // button listener
        BtnLogin.onClick.AddListener(OnLogin);
        BtnSignUp.onClick.AddListener(OnSignUp);

        // băt sự kiện lấy kết quả của firebase
        m_EmailPassLogin.OnAuthResult += OnConnectionLoading;
    }

    private void OnLogin()
    {
        if (IsFirebaseConnection())
        {
            m_EmailPassLogin.Login(LoginEmail.text, loginPassword.text);
        }
    }

    private void OnSignUp()
    {
        if (IsFirebaseConnection())
        {
            m_EmailPassLogin.SignUp(SignUpEmail.text, SignUpPassword.text);
        }
    }

    private bool IsFirebaseConnection()
    {
        if (m_EmailPassLogin.IsFirebaseConnection)
        {
            return true;
        }
        else
        {
            PanelFirebaseConnectionFail.SetActive(true);
            return false;
        }
    }

    private void OnConnectionLoading(System.Threading.Tasks.Task<AuthResult> task)
    {
        PanelLoading.SetActive(true);

        if (task.IsCanceled || task.IsFaulted) return;

        PanelLoading.SetActive(false);
        LoginSuccess.SetActive(true);
    }

}