using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Core
{
    public class UISettingsMenu : GameBehavior
    {
        [Header("UI SETTING MENU")]
        public Button ButtonLogin;
        public Button ButtonLogOut;

        [SerializeField] GameObject PanelNotifyLogOut;
        [SerializeField] RectTransform _panelContents;
        [SerializeField] AudioMixer _audioMixer;
        [SerializeField] bool _enableMenuSettings;
        [SerializeField] TMP_Dropdown _resolutionDropdown;
        [SerializeField] TMP_Dropdown _dropDownGraphics;
        [SerializeField] Toggle _toggleFullScreen;
        [SerializeField] Slider _sliderVolume;
        [SerializeField] Resolution[] _resolutions; // Array to store available screen resolutions

        bool isFullScreen;
        GameSettings gameSettings;
        User user;
        UIEmailPassLogin uIEmailPassLogin;
        DataManager dataManager;
        EmailPassLogin emailPassLogin;
        SceneLoader sceneManager;
        GameSystem gameSystem;

        private void Start()
        {
            sceneManager = FindFirstObjectByType<SceneLoader>();
            emailPassLogin = FindFirstObjectByType<EmailPassLogin>();
            dataManager = FindFirstObjectByType<DataManager>();
            gameSettings = FindFirstObjectByType<GameSettings>();
            emailPassLogin = FindFirstObjectByType<EmailPassLogin>();
            user = FindFirstObjectByType<User>();
            uIEmailPassLogin = FindFirstObjectByType<UIEmailPassLogin>();
            gameSettings = FindFirstObjectByType<GameSettings>();

            _enableMenuSettings = false;
            SetDropDownResolution();

            _panelContents.gameObject.SetActive(false);

            gameSettings.ActionDataChange += OnGameSettingChange;
            user.OnDataChange += OnUserDataChange;

            // Set value 
            OnUserDataChange(user);
            OnGameSettingChange(gameSettings);

            // listen button
            if (ButtonLogOut)
            {
                ButtonLogOut.onClick.AddListener(OnClickLogOut);
            }
        }

        private void OnClickLogOut()
        {
            PanelNotifyLogOut.SetActive(true);
            emailPassLogin.SignOut();
        }

        private void OnUserDataChange(User user)
        {
            if (this == null || ButtonLogin == null || ButtonLogOut == null) return;

            if (user.UserID == "")
            {
                ButtonLogin.gameObject.SetActive(true);
                ButtonLogOut.gameObject.SetActive(false);
            }
            else
            {
                ButtonLogin.gameObject.SetActive(false);
                ButtonLogOut.gameObject.SetActive(true);
            }
        }

        private void OnGameSettingChange(GameSettings gameSettings)
        {
            isFullScreen = gameSettings.IsFullScreen;

            _toggleFullScreen.isOn = gameSettings.IsFullScreen;
            _sliderVolume.value = gameSettings.MasterVolume;
            _dropDownGraphics.value = gameSettings.QualityIndex;
            _resolutionDropdown.value = gameSettings.CurrentResolutionIndex;

            SetVolume(gameSettings.MasterVolume);
            SetQuality(gameSettings.QualityIndex);
            SetFullscreen(gameSettings.IsFullScreen);
            SetResolutionCurrent(gameSettings.CurrentResolutionIndex);
        }

        private void SetDropDownResolution()
        {
            _resolutions = Screen.resolutions;
            _resolutionDropdown.ClearOptions();
            List<string> options = new List<string>(); // Create a list to store resolution strings
            for (int i = 0; i < _resolutions.Length; i++)
            {
                string option = _resolutions[i].width + "x" + _resolutions[i].height; // Format resolution as a string
                options.Add(option);
            }

            _resolutionDropdown.AddOptions(options);
        }

        /// <summary> Được sử dụng để bật tắt khi nhấn phím </summary>
        public void SetActiveMenuSettings(InputAction.CallbackContext context)
        {
            _enableMenuSettings = !_enableMenuSettings;
            _panelContents.gameObject.SetActive(_enableMenuSettings);
        }

        public void SetResolutionCurrent(int current)
        {
            if (gameSystem.CurrentPlatform == Platform.Standalone)
            {
                Screen.SetResolution(_resolutions[current].width, _resolutions[current].height, _toggleFullScreen.isOn);
            }
            gameSettings.CurrentResolutionIndex = current;
        }

        public void SetVolume(float volume)
        {
            _audioMixer.SetFloat("volume", volume);
            gameSettings.MasterVolume = volume;
        }

        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
            gameSettings.QualityIndex = qualityIndex;
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            gameSettings.IsFullScreen = isFullscreen;
        }


    }
}
