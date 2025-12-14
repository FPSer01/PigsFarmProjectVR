using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// Класс системы управления настройками приложения
/// </summary>
public class SettingsController : MonoBehaviour
{
    public static SettingsController Instance { get; private set; }

    // Значения для микшеров, в децебелах
    public const float MIN_AUDIO_VALUE = -80f;
    public const float MAX_AUDIO_VALUE = 0f;

    [Header("Data Container")]
    [SerializeField] private SettingsData settingsData;

    [Header("References")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Other")]
    [SerializeField] private float CameraYOffsetChange = 0.05f;

    public SettingsData SettingsData { get => settingsData; }

    private XROrigin xrOrigin;

    /// <summary>
    /// Первое открытое окно настроек
    /// </summary>
    public SettingsWindow LastOpenedWindow { private set; get; }
    private SettingsWindow previousOpenedWindow;

    /// <summary>
    /// Сколько открыто окон настроек в данный момент
    /// </summary>
    public int OpenedSettingsWindows { private set; get; }

    /// <summary>
    /// Событие, возникающее при открытии или закрытии какого-либо окна настроек
    /// </summary>
    public event Action<SettingsWindow> OnWindowOpenStatusChange;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        xrOrigin = FindAnyObjectByType<XROrigin>();
        LoadSettingsData();
    }

    /// <summary>
    /// Загрузить данные настроек
    /// </summary>
    public void LoadSettingsData()
    {
        settingsData.LoadSettings();

        SetSFXVolume(settingsData.SFXVolume);
        SetCameraYOffset(settingsData.CameraHeight);
    }

    /// <summary>
    /// Сохранить данные настроек
    /// </summary>
    public void SaveSettingsData()
    {
        settingsData.SaveSettings();
    }

    /// <summary>
    /// Изменить громкость звуковых эффектов
    /// </summary>
    /// <param name="normalizedValue">Значение громкости от 0 до 1</param>
    public void SetSFXVolume(float normalizedValue)
    {
        float volume = GetLogarithmicVolume(normalizedValue);
        audioMixer.SetFloat("SFXVolume", volume);
        settingsData.SFXVolume = normalizedValue;
    }

    /// <summary>
    /// Увеличить смещение камеры XR Origin
    /// </summary>
    public void SetCameraYOffsetUp()
    {
        SetCameraYOffset(xrOrigin.CameraYOffset + CameraYOffsetChange);
    }

    /// <summary>
    /// Уменьшить смещение камеры XR Origin
    /// </summary>
    public void SetCameraYOffsetDown()
    {
        SetCameraYOffset(xrOrigin.CameraYOffset - CameraYOffsetChange);
    }

    /// <summary>
    /// Изменить смещение камеры XR Origin
    /// </summary>
    /// <param name="newOffset">Новое значение смещения</param>
    public void SetCameraYOffset(float newOffset)
    {
        settingsData.CameraHeight = newOffset;
        xrOrigin.CameraYOffset = settingsData.CameraHeight;
    }

    /// <summary>
    /// Получить значение громкости звуков от 0 до 1
    /// </summary>
    /// <returns></returns>
    public float GetSFXVolume()
    {
        return settingsData.SFXVolume;
    }

    /// <summary>
    /// Получить смещение камеры XR Origin
    /// </summary>
    /// <returns></returns>
    public float GetCameraYOffset()
    {
        return settingsData.CameraHeight;
    }

    /// <summary>
    /// Выдает громкость звука по логарифмической функции
    /// </summary>
    /// <param name="value">Значение настройки звука от 0 до 1</param>
    /// <returns></returns>
    private float GetLogarithmicVolume(float value)
    {
        if (value <= 0f)
            return MIN_AUDIO_VALUE;

        return Mathf.Log10(value) * 20f;
    }

    /// <summary>
    /// Изменить количество открытых окон настроек
    /// </summary>
    /// <param name="window"></param>
    /// <param name="amount"></param>
    /// <remarks></remarks>
    public void ChangeOpenedWindowsCount(SettingsWindow window, int amount)
    {
        if (amount == 0)
            return;

        OpenedSettingsWindows += amount;
        OpenedSettingsWindows = Mathf.Clamp(OpenedSettingsWindows, 0, int.MaxValue);

        // Если открываем какое-либо окно
        if (amount > 0)
        {
            previousOpenedWindow = LastOpenedWindow;
            LastOpenedWindow = window;
        }
        // Если закрываем какое-либо окно
        else if (amount < 0)
        {
            if (window == LastOpenedWindow)
                LastOpenedWindow = previousOpenedWindow;
            else
                previousOpenedWindow = null;
        }

        if (OpenedSettingsWindows == 0)
        {
            LastOpenedWindow = null;
        }

        // ДЕБАГ
        if (previousOpenedWindow != null && LastOpenedWindow != null)
        {
            Debug.Log($"[SETTINGS SYSTEM] Windows opened: {OpenedSettingsWindows}, previous: {previousOpenedWindow.name}, last: {LastOpenedWindow.name}");
        }
        else if (LastOpenedWindow != null && previousOpenedWindow == null)
        {
            Debug.Log($"[SETTINGS SYSTEM] Windows opened: {OpenedSettingsWindows}, last: {LastOpenedWindow.name}");
        }
        else
        {
            Debug.Log($"[SETTINGS SYSTEM] Windows opened: {OpenedSettingsWindows}");
        }

        OnWindowOpenStatusChange?.Invoke(window);
    }
}
