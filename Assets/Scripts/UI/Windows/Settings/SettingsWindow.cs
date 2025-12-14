using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Общий класс для всех окон настроек
/// </summary>
public class SettingsWindow : UIWindow
{
    [Header("SettingsWindow: UI")]
    [SerializeField] protected Slider sfxVolumeSlider;
    [Space]
    [SerializeField] protected ModdedButton heightUp;
    [SerializeField] protected ModdedButton heightDown;

    protected virtual void Start()
    {
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChange);

        heightUp.OnClick.AddListener(HeightUp);
        heightDown.OnClick.AddListener(HeightDown);

        SettingsController.Instance.OnWindowOpenStatusChange += Settings_OnWindowStatusChange;
    }

    private void Settings_OnWindowStatusChange(SettingsWindow lastChangedWindow)
    {
        SettingsController.Instance.SaveSettingsData();

        CheckForLastOpenedWindow();
    }

    protected void SetupSettingsView()
    {
        SettingsController.Instance.LoadSettingsData();
        sfxVolumeSlider.value = SettingsController.Instance.SettingsData.SFXVolume;
    }

    protected void OnSFXVolumeChange(float value)
    {
        SettingsController.Instance.SetSFXVolume(value);
    }

    protected void HeightUp()
    {
        SettingsController.Instance.SetCameraYOffsetUp();
    }

    protected void HeightDown()
    {
        SettingsController.Instance.SetCameraYOffsetDown();
    }

    public override void Enable(bool enable, float fadeTime = 0)
    {
        if (enable)
        {
            // Меняем значение открытых окон настроек на 1
            SettingsController.Instance.ChangeOpenedWindowsCount(this, 1);

            SetupSettingsView();
        }
        else
        {
            // Меняем значение открытых окон настроек на -1
            SettingsController.Instance.ChangeOpenedWindowsCount(this, -1);

            SettingsController.Instance.SaveSettingsData();
        }

        base.Enable(enable, fadeTime);
    }

    /// <summary>
    /// Проверка на то, является ли это окно последним их открытых
    /// </summary>
    /// <remarks>Если это не последнее открытое окно, то нельзя менять настройки у этого окна</remarks>
    private void CheckForLastOpenedWindow()
    {
        if (SettingsController.Instance.LastOpenedWindow == this)
        {
            EnableSettingsElements(true);
            SetupSettingsView();
        }
        else
        {
            EnableSettingsElements(false);
        }
    }

    /// <summary>
    /// Включить или выключить элементы UI настроек
    /// </summary>
    /// <param name="enable"></param>
    protected void EnableSettingsElements(bool enable)
    {
        sfxVolumeSlider.interactable = enable;
        heightUp.Interactable = enable;
        heightDown.Interactable = enable;
    }
}
