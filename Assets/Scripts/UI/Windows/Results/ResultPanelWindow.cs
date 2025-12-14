using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Класс окна панели для отображения результатов выполнения задания
/// </summary>
public class ResultPanelWindow : UIWindow
{
    [Header("UI")]
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text correctDiagnosisText;
    [SerializeField] private TMP_Text wrongDiagnosisText;

    [Header("Buttons")]
    [SerializeField] private ModdedButton showDetailsButton;
    [SerializeField] private ModdedButton retryButton;
    [SerializeField] private ModdedButton exitButton;

    [Header("Windows")]
    [SerializeField] private DetailedResultWindow detailedResultWindow;
    [SerializeField] private float fadeTime = 0.1f;

    private void Start()
    {
        retryButton.OnClick.AddListener(Retry);
        exitButton.OnClick.AddListener(Exit);
        showDetailsButton.OnClick.AddListener(ShowDetails);
    }

    private void ShowDetails()
    {
        Enable(false, fadeTime);
        detailedResultWindow.Enable(true, fadeTime);
    }

    private void Exit()
    {
        Application.Quit();
    }

    private void Retry()
    {
        SceneLoader.Instance.ReloadCurrentScene();
    }

    /// <summary>
    /// Задать результаты
    /// </summary>
    public void SetResults(ResultsData data)
    {
        // Стиль для выделения текста
        string statusStyleName = data.TaskCompleted ? "Positive" : "Negative";
        // Статус выполнения (прохождения)
        string status = data.TaskCompleted ? "выполнено" : "не выполнено";

        headerText.text = $"Задание <style=\"{statusStyleName}\">{status}</style>";
        scoreText.text = $"Оценка: <style=\"{statusStyleName}\">{data.Score}%</style>";

        correctDiagnosisText.text = $"Верных диагнозов: {data.CorrectAnswers}";
        wrongDiagnosisText.text = $"Неверных диагнозов: {data.WrongAnswers}";

        // Подготавливаем подробные результаты
        detailedResultWindow.SetupDetailResult(data.DetailedResults);
    }
}
