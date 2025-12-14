using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Класс для контроля статуса свиньи: данные болезни и отображение болезни на модели
/// </summary>
public class PigStatusController : MonoBehaviour, IThermometerInteractable
{
    [Header("Components")]
    [SerializeField] private Renderer modelRenderer;
    [SerializeField] private Outline outline;

    private PigBehaviour pigBehaviour;

    private PigStatusData currentStatus;
    private float currentTemperature;

    public PigStatusData CurrentStatus { get => currentStatus; }

    private void Awake()
    {
        pigBehaviour = GetComponent<PigBehaviour>();
    }

    private void Start()
    {
        HighlightInteractable(false);
    }

    /// <summary>
    /// Задать статус (болезнь) свиньи
    /// </summary>
    /// <remarks>Сразу задает нужное поведение и материалы рендера, согласно статусу</remarks>
    /// <param name="statusData">Данные статуса</param>
    public void SetStatus(PigStatusData statusData)
    {
        currentStatus = statusData;

        // Меняем материал
        modelRenderer.material = statusData.IllnessMaterial;

        // Задаем температуру
        currentTemperature = Random.Range(statusData.MinTemperature, statusData.MaxTemperature + 0.001f);
        currentTemperature = MathF.Round(currentTemperature, 1);

        // Задаем тип поведения
        pigBehaviour.SetBehaviourType(statusData.BehaviourType);
    }

    public float GetTemperature()
    {
        return currentTemperature;
    }

    public void HighlightInteractable(bool highlight)
    {
        if (outline.enabled == highlight)
            return;

        outline.enabled = highlight;
    }
}
