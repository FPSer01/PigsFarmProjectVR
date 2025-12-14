using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static MainPanelWindow;

/// <summary>
/// Класс для панели справочника болезней
/// </summary>
public class InfoPanelWindow : UIWindow
{
    [Header("Parent")]
    [SerializeField] private MainPanelWindow mainPanel;

    [Header("Buttons")]
    [SerializeField] private ModdedButton returnButton;

    [Header("Info Window")]
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonContainer; // куда создавать кнопки состояний (болезней)
    [SerializeField] private PigStatusDataContainer infoContainer; // контейнер состояний (болезней)
    [Space]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [Space]
    [SerializeField] private GameObject imageContainer;
    [SerializeField] private Image imageBox;
    [Space]
    [SerializeField] private GameObject imageButtonsContainer;
    [SerializeField] private ModdedButton previousImageButton;
    [SerializeField] private ModdedButton nextImageButton;
    [SerializeField] private TMP_Text imageNumberText;

    private PigStatusData currentInfo; // Информация просматриваемая в данный момент
    private int currentImageIndex = 1; // Индекс картинки просматриваемой болезни
    private int maxImageNumber; // Последний номер картинки просматриваемой болезни

    private void Start()
    {
        ClearInfo();
        SetupInfo();

        returnButton.OnClick.AddListener(SummonMenuWindow);

        previousImageButton.OnClick.AddListener(ScrollToPreviousImage);
        nextImageButton.OnClick.AddListener(ScrollToNextImage);
    }

    private void SummonMenuWindow()
    {
        mainPanel.SetWindow(Windows.Menu);
    }

    public override void Enable(bool enable, float fadeTime = 0)
    {
        if (enable)
        {
            ClearInfo();
        }

        base.Enable(enable, fadeTime);
    }

    /// <summary>
    /// Подготовить панель, т.е. заполнить информацией о болезнях
    /// </summary>
    private void SetupInfo()
    {
        // Получаем данные о доступных болезнях
        var statusList = infoContainer.GetStatusList();

        // Настраиваем переключение информации о каждой болезни
        foreach (var statusData in statusList)
        {
            // Создаем кнопки для переключения
            GameObject buttonObj = Instantiate(buttonPrefab, buttonContainer);
            ModdedButton button = buttonObj.GetComponent<ModdedButton>();

            // Добавляем функцию переключения кнопке
            button.OnClick.AddListener(() =>
            {
                SetInfo(statusData);
            });

            // Меняем текст кнопки на название болезни
            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
            buttonText.text = statusData.Name;
        }
    }

    /// <summary>
    /// Переключить, поменять информацию о болезни
    /// </summary>
    private void SetInfo(PigStatusData info)
    {
        currentInfo = info;

        // Заполняем название и описание
        nameText.text = info.Name;
        descriptionText.text = info.Description;

        var images = info.Images;
        
        // Проверка на наличие картинок
        if (images.Count > 0)
        {
            imageContainer.SetActive(true);
            maxImageNumber = images.Count;

            // Если у нас есть только одна картинка - отрубаем элемент с переключением картинок
            imageButtonsContainer.SetActive(images.Count != 1);

            // Ставим первую картинку
            SetInfoImage(0);
        }
        // Если нет картинок - отрубаем элемент отображения картинок
        else 
        {
            imageContainer.SetActive(false);
        }
    }

    /// <summary>
    /// Очистить поле с информацией о болезни
    /// </summary>
    private void ClearInfo()
    {
        currentInfo = null;

        // Заполняем название и описание
        nameText.text = "";
        descriptionText.text = "Выберите болезнь справа для просмотра информации о ней";

        // Отключаем картинки
        imageContainer.SetActive(false);
        SetInfoImage(0);
    }

    /// <summary>
    /// Задать картинку болезни по индексу
    /// </summary>
    private void SetInfoImage(int index)
    {
        // Нет информации о просмотре => не ставить картинки
        if (currentInfo == null)
        {
            return;
        }

        // Валидация индекса картинки: не выходит ли он за пределы массива картинок
        int validIndex = Mathf.Clamp(index, 0, currentInfo.Images.Count - 1);
        currentImageIndex = validIndex;

        // Обновление счетчика
        UpdateImageNumberText();

        // Вставка нужной картинки
        imageBox.sprite = currentInfo.Images[validIndex];
    }

    private void UpdateImageNumberText()
    {
        imageNumberText.text = $"{currentImageIndex + 1} / {maxImageNumber}";
    }

    private void ScrollToNextImage()
    {
        SetInfoImage(currentImageIndex + 1);
    }

    private void ScrollToPreviousImage()
    {
        SetInfoImage(currentImageIndex - 1);
    }
}
