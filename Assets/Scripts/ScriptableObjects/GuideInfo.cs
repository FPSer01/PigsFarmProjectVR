using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Объект для хранения данных подсказок (обучения) для пользователя
/// </summary>

[CreateAssetMenu(fileName = "GuideInfo", menuName = "Data/GuideData")]
public class GuideInfo : ScriptableObject
{
    [TextArea(5, 20)]
    [SerializeField] private string guideText;

    /// <summary>
    /// Текст, описание гайда
    /// </summary>
    public string GuideText { get => guideText; }
}
