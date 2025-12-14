using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс-контейнер для PigStatusData объектов
/// </summary>
[CreateAssetMenu(fileName = "PigStatusDataContainer", menuName = "Data/PigStatusDataContainer")]
public class PigStatusDataContainer : ScriptableObject
{
    [SerializeField] private List<PigStatusData> statusList;

    /// <summary>
    /// Получить лист всех состояний контейнера
    /// </summary>
    /// <returns></returns>
    public List<PigStatusData> GetStatusList()
    {
        return statusList;
    }
}
