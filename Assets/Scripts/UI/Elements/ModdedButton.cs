using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Аналог стандартного класса Button, но с поддержкой изменения цвета текста и иконки,
/// отображение тултипа и проигрывание звуков кнопки
/// </summary>
public class ModdedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("General")]
    [SerializeField] private bool interactable = true;
    // Время переключения состояния
    [SerializeField] private float fadeTime = 0.15f;
    // Когда регистрировать клик кнопки
    [SerializeField] private ButtonRegisterClickType registerClick; 
    [Space]
    [SerializeField] private bool useTooltip;
    [SerializeField] private string toolTipText;
    [SerializeField] private bool useSFX;

    private ButtonState currentState = ButtonState.Normal;

    [Header("Graphic")]
    [SerializeField] private ButtonGraphicSettings background;
    [SerializeField] private ButtonGraphicSettings icon;
    [SerializeField] private ButtonGraphicSettings text;

    [Space(20f)]
    public UnityEvent OnClick;

    /// <summary>
    /// Можно ли взаимодействовать, работает ли кнопка?
    /// </summary>
    public bool Interactable { get => interactable; set => SetButtonState(value ? ButtonState.Normal : ButtonState.Disabled); }

    private void Start()
    {
        SetButtonState(currentState);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!interactable)
            return;

        if (Tooltip.Initialiazed && useTooltip)
            Tooltip.Instance.SetTooltip(transform, toolTipText);

        SetButtonState(ButtonState.Highlighted);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!interactable)
            return;

        if (Tooltip.Initialiazed && useTooltip)
            Tooltip.Instance.ClearTooltip();

        SetButtonState(ButtonState.Normal);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!interactable)
            return;

        SetButtonState(ButtonState.Highlighted);

        if (registerClick == ButtonRegisterClickType.OnRelease)
        {
            if (Tooltip.Initialiazed && useTooltip)
                Tooltip.Instance.ClearTooltip();

            OnClick?.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!interactable)
            return;

        SetButtonState(ButtonState.Pressed);

        if (registerClick == ButtonRegisterClickType.OnPress)
        {
            if (Tooltip.Initialiazed && useTooltip)
                Tooltip.Instance.ClearTooltip();

            OnClick?.Invoke();
        }
    }

    private void OnEnable()
    {
        SetButtonState(ButtonState.Normal, false);
    }

    private void OnDisable()
    {
        SetButtonState(ButtonState.Disabled, false);
    }

    private void SetButtonState(ButtonState state, bool useFadeTime = true)
    {
        float finalFadeTime = useFadeTime ? fadeTime : 0;

        background.SetGraphicState(state, finalFadeTime);
        icon.SetGraphicState(state, finalFadeTime);
        text.SetGraphicState(state, finalFadeTime);

        interactable = state != ButtonState.Disabled;

        currentState = state;
    }

    public void DebugMessage()
    {
        Debug.Log("Hello!");
    }

    /// <summary>
    /// Настройки графического элемента кнопки
    /// </summary>
    [Serializable]
    public struct ButtonGraphicSettings
    {
        public Graphic Graphic;
        public Color NormalColor;
        public Color HighlightedColor;
        public Color PressedColor;
        public Color DisabledColor;

        public void SetGraphicState(ButtonState state, float fadeTime = 0f)
        {
            if (Graphic == null)
                return;

            switch (state)
            {
                case ButtonState.Normal:
                    Graphic.DOColor(NormalColor, fadeTime).SetUpdate(true);
                    break;

                case ButtonState.Pressed:
                    Graphic.DOColor(PressedColor, fadeTime).SetUpdate(true);
                    break;

                case ButtonState.Highlighted:
                    Graphic.DOColor(HighlightedColor, fadeTime).SetUpdate(true);
                    break;

                case ButtonState.Disabled:
                    Graphic.DOColor(DisabledColor, fadeTime).SetUpdate(true);
                    break;
            }
        }
    }

    /// <summary>
    /// Состояние кнопки
    /// </summary>
    public enum ButtonState
    {
        Normal,
        Highlighted,
        Pressed,
        Disabled
    }

    /// <summary>
    /// Тип регистрации нажатия кнопки
    /// </summary>
    public enum ButtonRegisterClickType
    {
        OnRelease,
        OnPress
    }
}
