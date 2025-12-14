using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Класс планшетки для записи результатов
/// </summary>
public class Tablet : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private GameObject itemOriginal;

    [Header("Components")]
    [SerializeField] private Outline outline;
    [SerializeField] private CanvasGroup canvasGroup;

    private XRGrabInteractable interactable;

    // Все предметы списка
    private List<TabletItem> items = new();

    // Индекс последнего предмета списка
    private int lastItemIndex = 0;

    // Заблокировать взаимодействие с интерфейсом планшетки
    private bool blockUIInteraction = false;

    /// <summary>
    /// Событие, когда сменяется значение (ответ) выпадающего списка любого предмета в списке планшетки
    /// </summary>
    /// <remarks>Содержит индекс предмета в списке и измененный статус (ответ)</remarks>
    public event Action<int, PigStatusData> OnItemOptionChange;

    private void Awake()
    {
        itemOriginal.SetActive(false);
    }

    private void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();

        Highlight(false);

        // Валидация компонентов
        if (interactable == null)
        {
            Debug.LogError("Нет XRBaseInteractable", this);
        }
        else
        {
            // Выделяем, когда не взят объект в руки, но наведен лучом
            interactable.firstHoverEntered.AddListener((args) => Highlight(true && !interactable.isSelected));
            interactable.lastHoverExited.AddListener((args) => Highlight(false));

            interactable.selectEntered.AddListener((args) => Highlight(false));
            interactable.selectExited.AddListener((args) => Highlight(true && interactable.isHovered));
        }
    }

    /// <summary>
    /// Подсветить объект планшетки
    /// </summary>
    /// <param name="enableHighlight">Включить подсветку?</param>
    private void Highlight(bool enableHighlight)
    {
        outline.enabled = enableHighlight;
        canvasGroup.blocksRaycasts = !enableHighlight && !blockUIInteraction;
    }

    /// <summary>
    /// Добавить свинью в список
    /// </summary>
    public void AddOption(PigStatusDataContainer avalableStatuses)
    {
        GameObject itemObj = Instantiate(itemOriginal, itemsContainer);
        itemObj.SetActive(true);

        TabletItem item = itemObj.GetComponent<TabletItem>();
        item.SetLabel($"Свинья #{lastItemIndex + 1}");
        item.SetDropdown(avalableStatuses);

        // Подписка на вызов события смены значения предмета списка
        item.OnOptionChange += (status) => { OnItemOptionChange?.Invoke(lastItemIndex, status); };

        items.Add(item);
        lastItemIndex++;
    }

    /// <summary>
    /// Получить выбранные ответы в планшетке
    /// </summary>
    /// <returns></returns>
    public List<PigStatusData> GetChosenStatuses()
    {
        List<PigStatusData> result = new();

        foreach (TabletItem item in items)
        {
            result.Add(item.ChosenStatus);
        }

        return result;
    }

    /// <summary>
    /// Заброкировать или разблокировать взаимодействие с интерфейсом планшетки
    /// </summary>
    /// <param name="block"></param>
    public void BlockUIInteraction(bool block)
    {
        blockUIInteraction = block;
        canvasGroup.interactable = !block;
    }
}
