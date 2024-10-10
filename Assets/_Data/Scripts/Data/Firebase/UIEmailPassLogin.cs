using System.Collections;
using UnityEngine;
using TMPro;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;

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
    public GameObject PanelLoginSuccess;
    public GameObject PanelSignUpSuccess;
    public GameObject PanelNotifyAccountExist;
    public GameObject PanelNotifyAccountNotExistToLogin;

    EmailPassLogin m_EmailPassLogin;
    DataManager m_DataManager;

    private void Start()
    {
        m_EmailPassLogin = FindFirstObjectByType<EmailPassLogin>();
        m_DataManager = FindFirstObjectByType<DataManager>();

        // button listener
        BtnLogin.onClick.AddListener(OnClickLogin);
        BtnSignUp.onClick.AddListener(OnClickSignUp);

        // băt sự kiện lấy kết quả của auth firebase  
        m_EmailPassLogin.OnEmailExist += OnEmailExist;
        m_EmailPassLogin.OnLogIn += OnLogIn;
        m_EmailPassLogin.OnSignUp += OnSignUp;
        m_EmailPassLogin.OnAuthResult += OnAuthResult;
    }


    private void OnClickLogin()
    {
        if (TryFirebaseConnection())
        {
            m_EmailPassLogin.Login(LoginEmail.text, loginPassword.text);
        }
    }

    private void OnClickSignUp()
    {
        if (TryFirebaseConnection())
        {
            m_EmailPassLogin.SignUp(SignUpEmail.text, SignUpPassword.text);
        }
    }

    private bool TryFirebaseConnection()
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

    /// <summary> Khi có phản hồi từ EmailPassLogin </summary>
    private void OnEmailExist(bool isExist)
    {
        if (PanelNotifyAccountExist) PanelNotifyAccountExist.SetActive(isExist);
    }

    private void OnSignUp(Task<AuthResult> task)
    {
        PanelSignUpSuccess.SetActive(true);
    }

    private void OnLogIn(Task<AuthResult> task)
    {
        PanelLoginSuccess.SetActive(true);
    }
    
    private void OnAuthResult(Task<AuthResult> task)
    {
        // login fail
        if (task.IsFaulted)
        {
            PanelNotifyAccountNotExistToLogin.SetActive(true);
        }
    }
}