using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Класс тултипа для Modded Button
/// </summary>
public class Tooltip : MonoBehaviour
{
    public static Tooltip Instance { private set; get; }

    /// <summary>
    /// Иницилизирован ли синглтон тултипа
    /// </summary>
    public static bool Initialiazed { private set; get; }

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text tooltipText;
    [Space]
    [SerializeField] private float spacingY;

    private RectTransform rect;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialiazed = true;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        rect = GetComponent<RectTransform>();

        canvasGroup.alpha = 0f;
    }

    /// <summary>
    /// Задать цель тултипа
    /// </summary>
    /// <param name="target"></param>
    /// <param name="text"></param>
    /// <param name="fadeTime"></param>
    public void SetTooltip(Transform target, string text, float fadeTime = 0.1f)
    {
        if (!target.TryGetComponent(out RectTransform rectTarget))
            return;

        tooltipText.text = text;

        Transform targetParent = target.parent;
        transform.SetParent(targetParent);

        Vector3 localPos = rectTarget.localPosition;
        localPos += (Vector3)new Vector2(0, rectTarget.rect.size.y / 2 + rect.rect.size.y / 2 + spacingY);

        StartCoroutine(SetToolTipPosition(localPos));

        canvasGroup.DOFade(1, fadeTime).SetUpdate(true);
    }

    /// <summary>
    /// Очистить и скрыть тултип
    /// </summary>
    /// <param name="fadeTime"></param>
    public void ClearTooltip(float fadeTime = 0.1f)
    {
        tooltipText.text = "";
        canvasGroup.DOFade(0, fadeTime).SetUpdate(true);
    }

    private IEnumerator SetToolTipPosition(Vector3 localPos)
    {
        yield return new WaitForEndOfFrame();

        transform.localPosition = localPos;
        transform.localEulerAngles = Vector3.zero;
    }
}
