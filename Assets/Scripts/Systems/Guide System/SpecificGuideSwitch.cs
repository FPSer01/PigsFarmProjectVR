using UnityEngine;

/// <summary>
/// Класс переключения гайдов как TriggerGuideSwitch, но с возможностью переключать на определенный гайд
/// </summary>
public class SpecificGuideSwitch : TriggerGuideSwitch
{
    [SerializeField] private int guideIndexToSwitch;

    public override void GuideSwitch()
    {
        if (CheckGuideIndex())
        {
            UserGuideController.Instance.TriggerGuideSwitch(guideIndexToSwitch);
        }
    }
}
