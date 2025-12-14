using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Объект для хранения и передачи данных о настройках приложения
/// </summary>
[CreateAssetMenu(fileName = "SettingsData", menuName = "Data/SettingsData")]
public class SettingsData : ScriptableObject
{
    public const string SFX_VOLUME = "SFXVolume";
    public const string CAMERA_HEIGHT = "CameraHeight";

    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1f; // От 0 до 1

    [Range(-5f, 5f)]
    [SerializeField] private float cameraHeight = 0f; // От 0 до 5

    /// <summary>
    /// Громкость звуковых эффектов (значение от 0 до 1)
    /// </summary>
    public float SFXVolume { get => sfxVolume; set => sfxVolume = Mathf.Clamp(value, 0f, 1f); }

    /// <summary>
    /// Высота камеры (смещение камеры XR, значение от -5 до 5)
    /// </summary>
    public float CameraHeight { get => cameraHeight; set => cameraHeight = Mathf.Clamp(value, -5f, 5f); }

    /// <summary>
    /// Сохранить настройки в PlayerPrefs
    /// </summary>
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(SFX_VOLUME, SFXVolume);
        PlayerPrefs.SetFloat(CAMERA_HEIGHT, CameraHeight);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Выгрузить настройки из PlayerPrefs
    /// </summary>
    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey(SFX_VOLUME)) SFXVolume = PlayerPrefs.GetFloat(SFX_VOLUME);
        if (PlayerPrefs.HasKey(CAMERA_HEIGHT)) CameraHeight = PlayerPrefs.GetFloat(CAMERA_HEIGHT);
    }
}
