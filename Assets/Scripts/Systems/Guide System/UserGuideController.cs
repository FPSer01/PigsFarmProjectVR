using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Класс системы подсказок (обучения или сценария) для пользователя
/// </summary>
public class UserGuideController : MonoBehaviour
{
    public static UserGuideController Instance { get; private set; }
    
    [SerializeField] private List<GuideInfo> guides;

    private GuideInfo currentGuide;
    private int currentGuideIndex = 0;

    /// <summary>
    /// Событие, возникающее при переключении гайда (подсказки)
    /// </summary>
    public event Action<GuideInfo> OnGuideChange;

    /// <summary>
    /// Индекс гайда, который сейчас активен
    /// </summary>
    public int CurrentGuideIndex { get => currentGuideIndex; }

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
        SetGuide(0);
    }

    /// <summary>
    /// Сменить гайд
    /// </summary>
    /// <param name="guideIndex">Индекс гайда из списка guides</param>
    private void SetGuide(int guideIndex)
    {
        currentGuideIndex = guideIndex;

        currentGuide = guides[CurrentGuideIndex];
        OnGuideChange?.Invoke(currentGuide);
        Debug.Log($"[GUIDE SYSTEM] Current Guide Set: Name {currentGuide.name}, index: {CurrentGuideIndex}");
    }

    /// <summary>
    /// Переключить гайд на следующий
    /// </summary>
    public void TriggerGuideSwitch()
    {
        int newIndex = CurrentGuideIndex + 1;
        newIndex = Mathf.Clamp(newIndex, 0, guides.Count - 1);
        SetGuide(newIndex);
    }

    /// <summary>
    /// Переключить гайд на другой определенный гайд по индексу
    /// </summary>
    /// <param name="guideIndex"></param>
    public void TriggerGuideSwitch(int guideIndex)
    {
        int newIndex = guideIndex;
        newIndex = Mathf.Clamp(newIndex, 0, guides.Count - 1);
        SetGuide(newIndex);
    }
}
