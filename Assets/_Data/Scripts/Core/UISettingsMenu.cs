
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
        [SerializeField] AudioMixer audioMixer;
        [SerializeField] bool _enableMenuSettings;
        [SerializeField] TMP_Dropdown resolutionDropdown;
        [SerializeField] TMP_Dropdown _dropDownGraphics;
        [SerializeField] Toggle _toggleFullScreen;
        [SerializeField] Slider _sliderVolume;
        [SerializeField] Resolution[] resolutions; // Array to store available screen resolutions

        private GameSettings _gameSettings;
        private GamePCInput _gamePCInput;

        private void Awake()
        {
            _gamePCInput = new();

            // tắt menu setting khi mới bắt đầu 
            _enableMenuSettings = false;
            _panelContent.gameObject.SetActive(_enableMenuSettings);

            _gameSettings = GameSettings.Instance;
            SetResolution();
        }

        private void OnEnable()
        {
            GameSettingStats._OnDataChange += OnGameSettingLoad;
            _gamePCInput.MenuSettings += SetActiveMenuSettings;
        }

        private void OnDisable()
        {
            GameSettingStats._OnDataChange -= OnGameSettingLoad;
        }

        private void OnGameSettingLoad(GameSettingsData data)
        {
            // Load UI
            _toggleFullScreen.isOn = data._isFullScreen;
            _sliderVolume.value = data._masterVolume;
            _dropDownGraphics.value = data._qualityIndex;

            SetVolume(data._masterVolume);
            SetQuality(data._qualityIndex);
            SetFullscreen(data._isFullScreen);
            SetResolutionCurrent(data._currentResolutionIndex);
        }

        private void SetResolution()
        {
            resolutions = Screen.resolutions; // Get all available screen resolutions from the system

            resolutionDropdown.ClearOptions(); // Clear any existing options in the dropdown

            List<string> options = new List<string>(); // Create a list to store resolution strings

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height; // Format resolution as a string

                options.Add(option); // Add the resolution string to the options list

                // if (resolutions[i].width == Screen.currentResolution.width &&
                //     resolutions[i].height == Screen.currentResolution.height)
                // {
                //     currentResolutionIndex = i; // Set the index of the current resolution
                // }
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

            Debug.Log(current);
            _gameSettings._CurrentResolutionIndex = current;
        }

        public void SetVolume(float volume)
        {
            audioMixer.SetFloat("volume", volume);
            _gameSettings._MasterVolume = volume;
        }

        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
            _gameSettings._QualityIndex = qualityIndex;
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            _gameSettings._IsFullScreen = isFullscreen;
        }


    }
}
