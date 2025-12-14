using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Класс для переключения подсказок
/// </summary>
/// <remarks>Можно использовать как компонент для переключения или 
/// как физический триггер который реагирует на пользователя</remarks>
public class TriggerGuideSwitch : MonoBehaviour
{
    [Tooltip("Индексы гайдов, при которых будет срабатывать переключение на следующий гайд.\n" +
        "Например задан индекс 0, значит только во время показа первого гайда этот триггер сработает и переключит на второй гайд (переключит на индекс 1).")]
    [SerializeField] private List<int> triggerOnGuidesIndex;

    private Collider triggerCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();

        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = true;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        GuideSwitch();
    }

    /// <summary>
    /// Сменить гайд на следующий с учетом настроек гайда
    /// </summary>
    public virtual void GuideSwitch()
    {
        if (CheckGuideIndex())
        {
            UserGuideController.Instance.TriggerGuideSwitch();
        }
    }

    /// <summary>
    /// Проверить, соответствует ли нынешний индекс системы гайдов с индексом триггера
    /// </summary>
    /// <returns></returns>
    protected bool CheckGuideIndex()
    {
        int currentIndex = UserGuideController.Instance.CurrentGuideIndex;

        if(triggerOnGuidesIndex.FindAll((i) => i == currentIndex).ToList().Count > 0)
        {
            return true;
        }

        return false;
    }
}
