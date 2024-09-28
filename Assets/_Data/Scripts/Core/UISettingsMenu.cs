
using System.Collections.Generic;
using CuaHang;
using CuaHang.UI;
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
        [SerializeField] RectTransform _panelContents;
        [SerializeField] AudioMixer _audioMixer;
        [SerializeField] bool _enableMenuSettings;
        [SerializeField] TMP_Dropdown _resolutionDropdown;
        [SerializeField] TMP_Dropdown _dropDownGraphics;
        [SerializeField] Toggle _toggleFullScreen;
        [SerializeField] Slider _sliderVolume;
        [SerializeField] Resolution[] _resolutions; // Array to store available screen resolutions

        bool isFullScreen;
        GameSettings gameSettings => ObjectsManager.Instance.GameSettings;

        private void Awake()
        {
            _enableMenuSettings = false;  
            SetDropDownResolution();
        }

        private void Start()
        { 
            _panelContents.gameObject.SetActive(false);
        }

        private void OnEnable()
        { 
            GameSettings.ActionDataChange += OnGameSettingChange;
        }

        private void OnDisable()
        {
            GameSettings.ActionDataChange -= OnGameSettingChange;
        }

        private void OnGameSettingChange(GameSettings gameSettings)
        {
            isFullScreen = gameSettings.IsFullScreen;

            // Load UI 
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
            // Screen.SetResolution(_resolutions[current].width, _resolutions[current].height, _isFullScreen);
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
