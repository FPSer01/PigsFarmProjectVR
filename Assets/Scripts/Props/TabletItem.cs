using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Класс предмета списка в планшетке
/// </summary>
public class TabletItem : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text label;
    [SerializeField] private TMP_Dropdown dropdown;

    private List<PigStatusData> mainOptions;
    private PigStatusData chosenStatus;

    /// <summary>
    /// Выбранный статус предмета
    /// </summary>
    public PigStatusData ChosenStatus { get => chosenStatus; }

    /// <summary>
    /// Событие, когда сменяется значение выпадающего списка предмета (ответ)
    /// </summary>
    public event Action<PigStatusData> OnOptionChange;

    private void OnDropdownOptionChange(int optionIndex)
    {
        chosenStatus = mainOptions[optionIndex];

        OnOptionChange?.Invoke(chosenStatus);
    }

    /// <summary>
    /// Задать название предмета списка
    /// </summary>
    /// <param name="labelText"></param>
    public void SetLabel(string labelText)
    {
        label.text = labelText;
    }

    /// <summary>
    /// Задать варианты выбора предмета
    /// </summary>
    /// <param name="options"></param>
    public void SetDropdown(PigStatusDataContainer options)
    {
        dropdown.onValueChanged.RemoveListener(OnDropdownOptionChange);

        mainOptions = new();

        // Новые варинаты выбора
        List<TMP_Dropdown.OptionData> newOptions = new();
        // Полученные статусы
        List<PigStatusData> statusData = options.GetStatusList();

        // Выбор по умолчанию
        TMP_Dropdown.OptionData defaultOption = new("Нет диагноза");
        mainOptions.Add(null);
        newOptions.Add(defaultOption);

        foreach (var status in statusData)
        {
            TMP_Dropdown.OptionData option = new(status.Name);

            mainOptions.Add(status);
            newOptions.Add(option);
        }

        dropdown.options = newOptions;

        dropdown.onValueChanged.AddListener(OnDropdownOptionChange);
    }
}
