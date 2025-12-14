using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс-хранилище для информации о состоянии (болезни) свиньи
/// </summary>
[CreateAssetMenu(fileName = "PigStatusData", menuName = "Data/PigStatusData")]
public class PigStatusData : ScriptableObject
{
    [Header("Main Info")]
    [SerializeField] private string illnessName;
    [SerializeField] [TextArea(5, 25)] private string description;
    [SerializeField] private List<Sprite> images;

    [Header("Other Data")]
    [SerializeField] private float minTemperature;
    [SerializeField] private float maxTemperature;
    [Space]
    [SerializeField] private PigBehaviourType behaviourType;

    [Header("Object")]
    [SerializeField] private Material illnessMaterial;

    /// <summary>
    /// Название состояния (болезни)
    /// </summary>
    public string Name { get => illnessName; }

    /// <summary>
    /// Описание и информация состояния (болезни)
    /// </summary>
    public string Description { get => description; }

    /// <summary>
    /// Картинки состояния (болезни)
    /// </summary>
    public List<Sprite> Images { get => images; }

    /// <summary>
    /// Минимальная температура состояния (болезни)
    /// </summary>
    public float MinTemperature { get => minTemperature; }

    /// <summary>
    /// Максимальная температура состояния (болезни)
    /// </summary>
    public float MaxTemperature { get => maxTemperature; }

    /// <summary>
    /// Материал рендера состояния (болезни)
    /// </summary>
    public Material IllnessMaterial { get => illnessMaterial; }

    /// <summary>
    /// Поведение при этом состоянии (болезни)
    /// </summary>
    public PigBehaviourType BehaviourType { get => behaviourType; }
}
