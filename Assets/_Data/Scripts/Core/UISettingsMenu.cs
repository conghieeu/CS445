
using System.Collections.Generic;
using CuaHang.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Core
{
    public class UISettingsMenu : UIPanel
    {
        [SerializeField] AudioMixer _audioMixer;
        [SerializeField] bool _enableMenuSettings;
        [SerializeField] TMP_Dropdown resolutionDropdown;
        [SerializeField] TMP_Dropdown _dropDownGraphics;
        [SerializeField] Toggle _toggleFullScreen;
        [SerializeField] Slider _sliderVolume;
        [SerializeField] Resolution[] resolutions; // Array to store available screen resolutions

        GameSettings _gameSettings => GameSettings.Instance;

        protected override void Awake()
        {
            base.Awake();
            _enableMenuSettings = false;
            _panelContent.gameObject.SetActive(_enableMenuSettings);
        }

        private void Start()
        {
            SetDropDownResolution();
        }

        private void OnEnable()
        {
            GameSettings.ActionDataChange += OnGameSettingChange;
        }

        private void OnDisable()
        {
            GameSettings.ActionDataChange -= OnGameSettingChange;
        }

        private void OnGameSettingChange(GameSettings data)
        {
            // Load UI
            _toggleFullScreen.isOn = data.IsFullScreen;
            _sliderVolume.value = data.MasterVolume;
            _dropDownGraphics.value = data.QualityIndex;

            SetVolume(data.MasterVolume);
            SetQuality(data.QualityIndex);
            SetFullscreen(data.IsFullScreen);
            SetResolutionCurrent(data.CurrentResolutionIndex);
        }

        private void SetDropDownResolution()
        {
            resolutions = Screen.resolutions; // Get all available screen resolutions from the system

            resolutionDropdown.ClearOptions(); // Clear any existing options in the dropdown

            List<string> options = new List<string>(); // Create a list to store resolution strings

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height; // Format resolution as a string

                options.Add(option);
            }

            resolutionDropdown.AddOptions(options); // Add the resolution options to the dropdown
        }

        /// <summary> Được sử dụng để bật tắt khi nhấn phím </summary>
        public void SetActiveMenuSettings(InputAction.CallbackContext context)
        {
            _enableMenuSettings = !_enableMenuSettings;
            _panelContent.gameObject.SetActive(_enableMenuSettings);
        }

        public void SetResolutionCurrent(int current)
        {
            resolutionDropdown.value = current;
            resolutionDropdown.RefreshShownValue();

            _gameSettings.CurrentResolutionIndex = current;
        }

        public void SetVolume(float volume)
        {
            _audioMixer.SetFloat("volume", volume);
            _gameSettings.MasterVolume = volume;
        }

        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
            _gameSettings.QualityIndex = qualityIndex;
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            _gameSettings.IsFullScreen = isFullscreen;
        }


    }
}
