using DG.Tweening;
using UnityEngine;

/// <summary>
/// Базовый класс для окон графического интерфейса
/// </summary>
public class UIWindow : MonoBehaviour
{
    [Header("General: Window")]
    [SerializeField] protected CanvasGroup canvasGroup;

    private bool windowEnabled;

    /// <summary>
    /// Включено ли окно интерфейса
    /// </summary>
    public bool WindowEnabled { get => windowEnabled; }

    /// <summary>
    /// Включить или выключить окно
    /// </summary>
    /// <param name="enable">Включить окно?</param>
    /// <param name="fadeTime">Время переключения</param>
    public virtual void Enable(bool enable, float fadeTime = 0)
    {
        canvasGroup.blocksRaycasts = enable;
        canvasGroup.DOFade(enable ? 1 : 0, fadeTime).SetUpdate(true);
        windowEnabled = enable;
    }
}
