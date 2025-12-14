using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Класс системы экзамена
/// </summary>
public class ExamSystem : MonoBehaviour
{
    /// <summary>
    /// Прямая ссылка на синглтон-экземпляр
    /// </summary>
    public static ExamSystem Instance { get; private set; }

    [Header("General")]
    [Tooltip("Какая доля ответов должна быть верной для выполненого (пройденого или засчитанного) задания")]
    [Range(0f, 1f)] [SerializeField] private float goodScoreThreshold;

    [Header("Pigs")]
    [Tooltip("Префаб свиньи")]
    [SerializeField] private GameObject pigPrefab;
    [Tooltip("Точки спавна свиней, на каждую точку спавниться 1 свинья")]
    [SerializeField] private List<Transform> pigsSpawnPoints;

    [Header("Disease Control")]
    [SerializeField] private PigStatusDataContainer dataContainer;
    [Tooltip("Настройки болезней, которые могут появиться во время экзамена у свиней")]
    [SerializeField] private List<DiseaseSettings> diseaseSettings;

    [Header("Components")]
    [SerializeField] private Tablet tablet;
    [SerializeField] private MainPanelWindow mainPanel;
    [SerializeField] private TriggerGuideSwitch endExamTriggerGuide;

    // Сколько свиней получили диагноз
    private int diagnosedPigs = 0;

    // Заспавненые свиньи
    private List<PigStatusController> currentPigs;

    private void Awake()
    {
        // Задаем синглтон
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
        SetupPigs();

        tablet.OnItemOptionChange += Tablet_OnItemOptionChange;
        tablet.BlockUIInteraction(false);

        UpdateExamStasus();
    }

    // Когда что-то меняется на планшетке
    private void Tablet_OnItemOptionChange(int itemIndex, PigStatusData status)
    {
        // Добавляем, если поставили диагноз
        if (status != null)
        {
            diagnosedPigs++;
        }
        // Убавляем, если не ставили диагноз или его убрали
        else
        {
            diagnosedPigs--;
        }

        diagnosedPigs = Mathf.Clamp(diagnosedPigs, 0, currentPigs.Count);

        UpdateExamStasus();
    }

    /// <summary>
    /// Создать и подготовить свиней для экзамена
    /// </summary>
    private void SetupPigs()
    {
        if (pigPrefab == null)
        {
            Debug.LogError("Нет префаба свиньи");
            return;
        }

        diagnosedPigs = 0;
        currentPigs = new();

        // Спавн свиньи по каждой точке
        foreach (var spawnPoint in pigsSpawnPoints)
        {
            GameObject pigObject = Instantiate(pigPrefab, spawnPoint.position, Quaternion.identity);

            // Выставление статуса свинье
            PigStatusController pigStatusController = pigObject.GetComponent<PigStatusController>();
            PigStatusData randomStatus = GetRandomStatus();

            if (randomStatus == null)
                continue;

            pigStatusController.SetStatus(randomStatus);

            currentPigs.Add(pigStatusController);
            tablet.AddOption(dataContainer);
        }
    }

    /// <summary>
    /// Получить статус свиньи, учитывая их вес появления (шанса появления)
    /// </summary>
    /// <returns></returns>
    private PigStatusData GetRandomStatus()
    {
        // Общий вес
        float allWeight = 0f;
        // Расчет общего веса
        diseaseSettings.ForEach((data) => allWeight += data.AppearWeight);

        // Массив словарь шансов появления в % по индексу появления в diseaseSettings
        List<float> statusChance = new();

        // Расчет шанса появления
        foreach (var data in diseaseSettings)
        {
            statusChance.Add(data.AppearWeight / allWeight);
        }

        float randomValue = Random.value;

        // Нижний порог шанса
        float topCeilChance = 0f;
        // Верхний порог шанса
        float bottomCeilChance = 0f;

        // Выбор по шансу появления
        for (int i = 0; i < statusChance.Count; i++)
        {
            if (statusChance[i] == 0f)
                continue;

            bottomCeilChance = topCeilChance;
            topCeilChance += statusChance[i];

            // Проверка между нижним и верхним порогами шанса появления
            if (bottomCeilChance < randomValue && randomValue <= topCeilChance)
                return diseaseSettings[i].DiseaseData;
        }

        Debug.LogError("С выбором статуса свиньи что-то пошло не так");
        return null;
    }

    /// <summary>
    /// Обновить статус экзамена
    /// </summary>
    private void UpdateExamStasus()
    {
        string statusText;

        if (IsAllPigsDiagnosed())
        {
            statusText = $"<style=\"Positive\">Все диагнозы расставлены</style>";
            mainPanel.EnableEndExamButton(true);
            endExamTriggerGuide.GuideSwitch();
        }
        else
        {
            statusText = $"Осталось диагнозов: <style=\"Negative\">{currentPigs.Count - diagnosedPigs}</style>";
            mainPanel.EnableEndExamButton(false);
        }

        mainPanel.SetPigsCountText(statusText);
    }

    /// <summary>
    /// Проверка на простановку диагнозов всем свиньям
    /// </summary>
    /// <returns>Всем ли свиньям поставили диагноз?</returns>
    private bool IsAllPigsDiagnosed()
    {
        int pigsCount = Mathf.Clamp(currentPigs.Count - diagnosedPigs, 0, currentPigs.Count);

        return pigsCount == 0;
    }

    /// <summary>
    /// Подвести итоги, сгенерировать результаты выполнения задания
    /// </summary>
    public ResultsData GetResults()
    {
        int correctAnswers = 0;

        List<PigStatusData> answers = tablet.GetChosenStatuses();
        List<DetailedResult> detailedResults = new();

        for (int i = 0; i < currentPigs.Count; i++)
        {
            // Правильный ответ
            PigStatusData correctAnswer = currentPigs[i].CurrentStatus;
            // Ответ, который проверяем
            PigStatusData answer = answers[i];

            if (answer == correctAnswer)
            {
                correctAnswers++;
            }

            DetailedResult detailedResult = new DetailedResult(i, answer.Name, correctAnswer.Name);
            detailedResults.Add(detailedResult);
        }

        ResultsData data = new ResultsData(correctAnswers, answers.Count, goodScoreThreshold, detailedResults);
        return data;
    }

    /// <summary>
    /// Закончить экзамен
    /// </summary>
    public void EndExam()
    {
        tablet.BlockUIInteraction(true);
    }

    /// <summary>
    /// Данные настроек болезни в режиме экзамена
    /// </summary>
    [Serializable]
    public struct DiseaseSettings
    {
        [Tooltip("Объект данных болезни свиньи")]
        [SerializeField] private PigStatusData diseaseData;
        [Tooltip("Вес шанса появления болезни. " +
            "Например, первая болезнь => вес = 2, " +
            "вторая болезнь => вес = 8, то значит шанс первой болезни будет 2/10 (20%), " +
            "а второй => 8/10 (80%), т.к. общий вес 2 + 8 = 10")]
        [SerializeField] private float appearWeight;

        public PigStatusData DiseaseData { get => diseaseData; set => diseaseData = value; }
        public float AppearWeight { get => appearWeight; set => appearWeight = value; }
    }
}

/// <summary>
/// Результаты выполнения задания
/// </summary>
public struct ResultsData
{
    public ResultsData(int correctAnswers, int overallAnswers, float goodScoreThreshold, List<DetailedResult> detailedResults)
    {
        CorrectAnswers = correctAnswers;
        WrongAnswers = overallAnswers - correctAnswers;
        RawScore = (float)correctAnswers / overallAnswers;
        Score = Mathf.Round(RawScore * 100f);

        TaskCompleted = RawScore >= goodScoreThreshold;
        DetailedResults = detailedResults;
    }

    /// <summary>
    /// Чистая оценка от 0 до 1
    /// </summary>
    public float RawScore;

    /// <summary>
    /// Оценка в процентах
    /// </summary>
    public float Score;

    public int CorrectAnswers;
    public int WrongAnswers;
    public bool TaskCompleted;
    public List<DetailedResult> DetailedResults;
}

/// <summary>
/// Конкретный результат проверки одного ответа
/// </summary>
public struct DetailedResult
{
    public DetailedResult(int answerIndex, string givenAnswer, string correctAnswer)
    {
        Index = answerIndex;
        GivenAnswerName = givenAnswer;
        CorrectAnswerName = correctAnswer;
    }

    public int Index;
    public string GivenAnswerName;
    public string CorrectAnswerName;
}

