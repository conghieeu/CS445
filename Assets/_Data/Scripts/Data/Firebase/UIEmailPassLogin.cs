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
    public GameObject PanelNotifyLogOutSuccess;

    EmailPassLogin m_EmailPassLogin;

    private void Start()
    {
        m_EmailPassLogin = FindFirstObjectByType<EmailPassLogin>();

        // button listener
        BtnLogin.onClick.AddListener(OnLogin);
        BtnSignUp.onClick.AddListener(OnSignUp);

        // băt sự kiện lấy kết quả của firebase
        m_EmailPassLogin.OnAuthResult += OnAuthResult;
        m_EmailPassLogin.OnAccountExists += OnAccountExist;
    }

    public void LogOutAccount()
    {
        m_EmailPassLogin.LogOutAccount();
        PanelNotifyLogOutSuccess.SetActive(true);
    }

    private void OnAccountExist(bool arg0)
    {
        if (arg0)
        {
            PanelNotifyAccountExist.SetActive(true);
        }
    }

    private void OnLogin()
    {
        if (TryFirebaseConnection())
        {
            m_EmailPassLogin.Login(LoginEmail.text, loginPassword.text);
        }
    }

    private void OnSignUp()
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

    private void OnAuthResult(Task<AuthResult> task)
    {
        PanelLoading.SetActive(true);

        if (task.IsCanceled || task.IsFaulted) return;

        if (task.Result.User.IsEmailVerified)
        {
            PanelLoading.SetActive(false);
            PanelSignUpSuccess.SetActive(true);
            return;
        }

        // login success
        Debug.Log($"Login success");
        PanelLoading.SetActive(false);
        PanelLoginSuccess.SetActive(true);

    }

}