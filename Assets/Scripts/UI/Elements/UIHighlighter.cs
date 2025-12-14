using DG.Tweening;
using System.Collections;
using UnityEngine;

/// <summary>
/// Класс для подсветки UI элементов
/// </summary>
public class UIHighlighter : MonoBehaviour
{
    [Header("Highlighter")]
    [SerializeField] private CanvasGroup highlighterObject;

    [Header("Settings")]
    [SerializeField] private bool highlightOnStart;
    [Range(0f, 1f)] [SerializeField] private float highlightOpacity = 1f;
    [SerializeField] private float flashTime;

    private bool highlighting = false;

    private void Start()
    {
        highlighterObject.alpha = 0f;
        EnableHighlight(highlightOnStart);
    }

    /// <summary>
    /// Включить или выключить анимацию подсветки
    /// </summary>
    /// <param name="enable"></param>
    public void EnableHighlight(bool enable)
    {
        if (highlighting == enable)
            return;

        if (enable)
        {
            StartCoroutine(Highlight());
        }
        else
        {
            StopAllCoroutines();
        }

        highlighting = enable;
    }

    private IEnumerator Highlight()
    {
        while (true)
        {
            highlighterObject.DOFade(highlightOpacity, flashTime / 2).SetUpdate(true);

            yield return new WaitForSecondsRealtime(flashTime / 2);

            highlighterObject.DOFade(0, flashTime / 2).SetUpdate(true);

            yield return new WaitForSecondsRealtime(flashTime / 2);
        }
    }
}
