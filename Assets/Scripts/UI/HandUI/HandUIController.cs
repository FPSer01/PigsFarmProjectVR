using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Контроллер UI на руке
/// </summary>
public class HandUIController : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField] private UIWindow mainPanelWindow;
    [SerializeField] private UIWindow settingsWindow;
    [SerializeField] private UIWindow buttonWindow;
    [Space]
    [SerializeField] private TMP_Text handUIText;
    [SerializeField] private float fadeTime = 0.1f;

    [Header("Buttons")]
    [SerializeField] private ModdedButton openMenuButton;
    [SerializeField] private ModdedButton closeMenuButton;
    [SerializeField] private ModdedButton settingsButton;
    [SerializeField] private ModdedButton retryButton;
    [SerializeField] private ModdedButton exitButton;

    private void Awake()
    {
        UserGuideController.Instance.OnGuideChange += OnGuideChange;
    }

    private void Start()
    {
        openMenuButton.OnClick.AddListener(OpenMenu);
        closeMenuButton.OnClick.AddListener(CloseMenu);
        settingsButton.OnClick.AddListener(ToggleSettings);
        retryButton.OnClick.AddListener(Retry);
        exitButton.OnClick.AddListener(Exit);

        mainPanelWindow.Enable(false);
        settingsWindow.Enable(false);
        buttonWindow.Enable(true);
    }

    private void OnGuideChange(GuideInfo guideData)
    {
        handUIText.text = guideData.GuideText;
        Debug.Log("Text Updated");
    }

    private void ToggleSettings()
    {
        settingsWindow.Enable(!settingsWindow.WindowEnabled, fadeTime);
    }

    private void Exit()
    {
        Application.Quit();
    }

    private void Retry()
    {
        SceneLoader.Instance.ReloadCurrentScene();
    }

    private void CloseMenu()
    {
        mainPanelWindow.Enable(false, fadeTime);
        buttonWindow.Enable(true, fadeTime);
    }

    private void OpenMenu()
    {
        mainPanelWindow.Enable(true, fadeTime);
        buttonWindow.Enable(false, fadeTime);
    }
}
