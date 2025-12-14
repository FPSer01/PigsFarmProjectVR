using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Класс подсветки объектов взаимодействия
/// </summary>
public class XRInteractableHighlighter : MonoBehaviour
{
    private Outline outline;
    private XRBaseInteractable interactable;

    protected virtual void Awake()
    {
        outline = GetComponent<Outline>();
        interactable = GetComponent<XRBaseInteractable>();
    }

    protected virtual void Start()
    {
        // Валидация компонентов
        if (outline == null || interactable == null)
        {
            Debug.LogError("Нет Outline или XRBaseInteractable у объекта взаимодействия", this);
            return;
        }

        Highlight(false);

        // Выделяем, когда не взят объект в руки, но наведен лучом
        interactable.firstHoverEntered.AddListener((args) => Highlight(true && !interactable.isSelected));
        interactable.lastHoverExited.AddListener((args) => Highlight(false));

        interactable.selectEntered.AddListener((args) => Highlight(false));
        interactable.selectExited.AddListener((args) => Highlight(true && interactable.isHovered));
    }

    /// <summary>
    /// Подсветить объект
    /// </summary>
    /// <param name="enableHighlight">Включить подсветку?</param>
    public void Highlight(bool enableHighlight)
    {
        outline.enabled = enableHighlight;
    }
}
