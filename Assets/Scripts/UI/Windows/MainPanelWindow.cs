using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// Класс для панели меню
/// </summary>
public class MainPanelWindow : UIWindow
{
    [Header("Windows")]
    [SerializeField] private SettingsPanelWindow settingsWindow;
    [SerializeField] private InfoPanelWindow infoWindow;
    [SerializeField] private ResultPanelWindow resultWindow;

    [Header("Buttons")]
    [SerializeField] private ModdedButton settingsButton;
    [SerializeField] private ModdedButton infoButton;
    [SerializeField] private ModdedButton retryButton;
    [SerializeField] private ModdedButton exitButton;
    [SerializeField] private ModdedButton endExamButton;

    [Header("UI")]
    [SerializeField] private TMP_Text pigsCountText;

    [Header("Settings")]
    [SerializeField] private float fadeTime = 0.1f;

    private Windows currentWindow;

    private UIHighlighter endExamButtonHighlighter;

    private void Awake()
    {
        endExamButtonHighlighter = endExamButton.gameObject.GetComponent<UIHighlighter>();
    }

    private void Start()
    {
        SetWindow(Windows.Menu);

        settingsButton.OnClick.AddListener(SummonSettingsWindow);
        infoButton.OnClick.AddListener(SummonInfoWindow);
        retryButton.OnClick.AddListener(Retry);
        exitButton.OnClick.AddListener(Exit);
        endExamButton.OnClick.AddListener(EndExam);

        EnableEndExamButton(false);
    }

    public void SetPigsCountText(string text)
    {
        pigsCountText.text = text;
    }

    public void EnableEndExamButton(bool enable)
    {
        endExamButton.gameObject.SetActive(enable);
        endExamButtonHighlighter.EnableHighlight(enable);
    }

    private void EndExam()
    {
        ExamSystem.Instance.EndExam();

        var resultsData = ExamSystem.Instance.GetResults();
        resultWindow.SetResults(resultsData);

        SetWindow(Windows.Result);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Retry()
    {
        SceneLoader.Instance.ReloadCurrentScene();
    }

    public void SummonInfoWindow()
    {
        SetWindow(Windows.Info);
    }

    public void SummonSettingsWindow()
    {
        SetWindow(Windows.Settings);
    }

    /// <summary>
    /// Переключить окно панели
    /// </summary>
    /// <param name="window">Тип окна</param>
    public void SetWindow(Windows window)
    {
        currentWindow = window;

        canvasGroup.blocksRaycasts = window == Windows.Menu;
        settingsWindow.Enable(window == Windows.Settings, fadeTime);
        infoWindow.Enable(window == Windows.Info, fadeTime);
        resultWindow.Enable(window == Windows.Result, fadeTime);
    }

    /// <summary>
    /// Типы окон панели
    /// </summary>
    public enum Windows
    {
        Menu,
        Settings,
        Info,
        Result
    }
}
