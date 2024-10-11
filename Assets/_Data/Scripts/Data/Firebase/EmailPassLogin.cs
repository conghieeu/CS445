using System.Collections;
using UnityEngine;
using TMPro;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.SceneManagement;

public class EmailPassLogin : GameBehavior
{
    [Header("Email Pass Login")]
    DatabaseReference connectedRef;
    DataManager dataManager;
    FirebaseDataSaver firebaseDataSaver;

    public bool IsFirebaseConnection { get; private set; }
    public UnityAction<Task<AuthResult>> OnAuthResult;
    public UnityAction<Task<AuthResult>> OnLogIn;
    public UnityAction<Task<AuthResult>> OnSignUp;
    public UnityAction<bool> OnEmailExist;

    private void Start()
    {
        dataManager = GetComponent<DataManager>();
        firebaseDataSaver = GetComponent<FirebaseDataSaver>();

        // Trỏ đến nút đặc biệt `.info/connected`
        connectedRef = FirebaseDatabase.DefaultInstance.GetReference(".info/connected");

        // Lắng nghe thay đổi trạng thái kết nối với Firebase
        connectedRef.ValueChanged += HandleFirebaseConnectionChanged;
    }

    private void HandleFirebaseConnectionChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError("Lỗi khi kiểm tra kết nối Firebase: " + args.DatabaseError.Message);
            return;
        }

        IsFirebaseConnection = (bool)args.Snapshot.Value;

        if (IsFirebaseConnection)
        {
            Debug.Log("Đã kết nối đến Firebase.");
        }
        else
        {
            Debug.LogWarning("Mất kết nối đến Firebase!");
        }
    }

    // bool CheckInternetConnection()
    // {
    //     bool isConnectedInternet = Application.internetReachability != NetworkReachability.NotReachable;
    //     OnInternetConnection?.Invoke(isConnectedInternet);

    //     return isConnectedInternet;
    // }

    public void SignUp(string email, string password)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        OnEmailExist?.Invoke(true);

        // tạo tài khoản
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                In($"Error: CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                In($"Error: CreateUserWithEmailAndPasswordAsync encountered an error: {task.Exception}");
                return;
            }

            OnEmailExist?.Invoke(false);
            OnSignUp?.Invoke(task);

            AuthResult result = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);

            // save new account to firebase
            firebaseDataSaver.SaveDataFn(result.User.UserId);

            if (result.User.IsEmailVerified)
            {
                In($"Sign up Successful");
                return;
            }
            else
            {
                In($"Please verify your email!!");
                StartCoroutine(SendEmailForVerificationAsync());
            }
        });
    }

    public void SignOut()
    {
        dataManager = FindFirstObjectByType<DataManager>();

        dataManager.ResetGame();
        FindFirstObjectByType<SceneLoader>().LoadGameScene(GameScene.Loading);

        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        auth.SignOut();
    }

    public void Login(string email, string password)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        Credential credential = EmailAuthProvider.GetCredential(email, password);

        auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
            OnAuthResult?.Invoke(task);

            if (task.IsCanceled)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);

            Debug.Log($"LOADING");
            firebaseDataSaver.LoadDataFn(result.User.UserId); // tải data trên firebase về đè lênh data local 
            FindFirstObjectByType<SceneLoader>().LoadGameScene(GameScene.Loading);

            OnLogIn?.Invoke(task);
        });
    }

    public void ResetPassword(string email)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        auth.SendPasswordResetEmailAsync(email).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                return;
            }
            Debug.Log("Password reset email sent successfully.");
        });
    }

    IEnumerator SendEmailForVerificationAsync()
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user != null)
        {
            var sendEmailTask = user.SendEmailVerificationAsync();
            yield return new WaitUntil(() => sendEmailTask.IsCompleted);

            if (sendEmailTask.Exception != null)
            {
                print("Email send error");
                FirebaseException firebaseException = sendEmailTask.Exception.GetBaseException() as FirebaseException;
                AuthError error = (AuthError)firebaseException.ErrorCode;

                switch (error)
                {
                    case AuthError.None:
                        break;
                    case AuthError.Unimplemented:
                        break;
                    case AuthError.Failure:
                        break;
                    case AuthError.InvalidCustomToken:
                        break;
                    case AuthError.CustomTokenMismatch:
                        break;
                    case AuthError.InvalidCredential:
                        break;
                    case AuthError.UserDisabled:
                        break;
                    case AuthError.AccountExistsWithDifferentCredentials:
                        break;
                    case AuthError.OperationNotAllowed:
                        break;
                    case AuthError.EmailAlreadyInUse:
                        break;
                    case AuthError.RequiresRecentLogin:
                        break;
                    case AuthError.CredentialAlreadyInUse:
                        break;
                    case AuthError.InvalidEmail:
                        break;
                    case AuthError.WrongPassword:
                        break;
                    case AuthError.TooManyRequests:
                        break;
                    case AuthError.UserNotFound:
                        break;
                    case AuthError.ProviderAlreadyLinked:
                        break;
                    case AuthError.NoSuchProvider:
                        break;
                    case AuthError.InvalidUserToken:
                        break;
                    case AuthError.UserTokenExpired:
                        break;
                    case AuthError.NetworkRequestFailed:
                        break;
                    case AuthError.InvalidApiKey:
                        break;
                    case AuthError.AppNotAuthorized:
                        break;
                    case AuthError.UserMismatch:
                        break;
                    case AuthError.WeakPassword:
                        break;
                    case AuthError.NoSignedInUser:
                        break;
                    case AuthError.ApiNotAvailable:
                        break;
                    case AuthError.ExpiredActionCode:
                        break;
                    case AuthError.InvalidActionCode:
                        break;
                    case AuthError.InvalidMessagePayload:
                        break;
                    case AuthError.InvalidPhoneNumber:
                        break;
                    case AuthError.MissingPhoneNumber:
                        break;
                    case AuthError.InvalidRecipientEmail:
                        break;
                    case AuthError.InvalidSender:
                        break;
                    case AuthError.InvalidVerificationCode:
                        break;
                    case AuthError.InvalidVerificationId:
                        break;
                    case AuthError.MissingVerificationCode:
                        break;
                    case AuthError.MissingVerificationId:
                        break;
                    case AuthError.MissingEmail:
                        break;
                    case AuthError.MissingPassword:
                        break;
                    case AuthError.QuotaExceeded:
                        break;
                    case AuthError.RetryPhoneAuth:
                        break;
                    case AuthError.SessionExpired:
                        break;
                    case AuthError.AppNotVerified:
                        break;
                    case AuthError.AppVerificationFailed:
                        break;
                    case AuthError.CaptchaCheckFailed:
                        break;
                    case AuthError.InvalidAppCredential:
                        break;
                    case AuthError.MissingAppCredential:
                        break;
                    case AuthError.InvalidClientId:
                        break;
                    case AuthError.InvalidContinueUri:
                        break;
                    case AuthError.MissingContinueUri:
                        break;
                    case AuthError.KeychainError:
                        break;
                    case AuthError.MissingAppToken:
                        break;
                    case AuthError.MissingIosBundleId:
                        break;
                    case AuthError.NotificationNotForwarded:
                        break;
                    case AuthError.UnauthorizedDomain:
                        break;
                    case AuthError.WebContextAlreadyPresented:
                        break;
                    case AuthError.WebContextCancelled:
                        break;
                    case AuthError.DynamicLinkNotActivated:
                        break;
                    case AuthError.Cancelled:
                        break;
                    case AuthError.InvalidProviderId:
                        break;
                    case AuthError.WebInternalError:
                        break;
                    case AuthError.WebStorateUnsupported:
                        break;
                    case AuthError.TenantIdMismatch:
                        break;
                    case AuthError.UnsupportedTenantOperation:
                        break;
                    case AuthError.InvalidLinkDomain:
                        break;
                    case AuthError.RejectedCredential:
                        break;
                    case AuthError.PhoneNumberNotFound:
                        break;
                    case AuthError.InvalidTenantId:
                        break;
                    case AuthError.MissingClientIdentifier:
                        break;
                    case AuthError.MissingMultiFactorSession:
                        break;
                    case AuthError.MissingMultiFactorInfo:
                        break;
                    case AuthError.InvalidMultiFactorSession:
                        break;
                    case AuthError.MultiFactorInfoNotFound:
                        break;
                    case AuthError.AdminRestrictedOperation:
                        break;
                    case AuthError.UnverifiedEmail:
                        break;
                    case AuthError.SecondFactorAlreadyEnrolled:
                        break;
                    case AuthError.MaximumSecondFactorCountExceeded:
                        break;
                    case AuthError.UnsupportedFirstFactor:
                        break;
                    case AuthError.EmailChangeNeedsVerification:
                        break;
                    default:
                        break;
                }
            }
            else
            {
                print("Email successfully send");
            }
        }
    }

    // In ra tất cả các nhà cung cấp có sẵn cho một email nhất định.
    List<string> DisplayIdentityProviders(FirebaseAuth auth, string email)
    {
        List<string> listProvider = new List<string>();

        auth.FetchProvidersForEmailAsync(email).ContinueWith((authTask) =>
        {
            if (authTask.IsCanceled)
            {
                Debug.Log("Lấy nhà cung cấp bị hủy.");
            }
            else if (authTask.IsFaulted)
            {
                Debug.Log("Lấy nhà cung cấp gặp lỗi.");
                Debug.Log(authTask.Exception.ToString());
            }
            else if (authTask.IsCompleted)
            {
                Debug.Log("Nhà cung cấp Email:");
                foreach (string provider in authTask.Result)
                {
                    Debug.Log(provider);
                    listProvider.Add(provider);
                }
            }
        });

        return listProvider;
    }
}

