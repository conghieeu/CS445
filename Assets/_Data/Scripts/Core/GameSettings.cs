using System;
using System.Collections;
using System.Collections.Generic;
using CuaHang;
using Core;
using UnityEngine;

public class GameSettings : Singleton<GameSettings>
{
    [Header("GameSettings")]
    public GameSettingStats _gameSettingStats;

    [SerializeField] bool _isFullScreen;
    [SerializeField] int _qualityIndex;
    [SerializeField] float _masterVolume;
    [SerializeField] int _currentResolutionIndex;
    [SerializeField] Quaternion _camRotation;

    public bool _IsFullScreen { get => _isFullScreen; set => _isFullScreen = value; }
    public int _QualityIndex { get => _qualityIndex; set => _qualityIndex = value; }
    public float _MasterVolume { get => _masterVolume; set => _masterVolume = value; }
    public int _CurrentResolutionIndex { get => _currentResolutionIndex; set => _currentResolutionIndex = value; }
    public Quaternion _CamRotation { get => _camRotation; set => _camRotation = value; }

    protected override void Awake()
    {
        base.Awake();

        _gameSettingStats = GetComponentInChildren<GameSettingStats>();
    }

    public void SetProperties(GameSettingsData data)
    {
        _isFullScreen = data._isFullScreen;
        _qualityIndex = data._qualityIndex;
        _masterVolume = data._masterVolume;
        _currentResolutionIndex = data._currentResolutionIndex;
        _camRotation = data._camRotation;
    }
}
