using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс окна подробных результатов
/// </summary>
public class DetailedResultWindow : UIWindow
{
    [Header("Setup")]
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private Transform rowsContainer;

    [Header("Buttons")]
    [SerializeField] private ModdedButton returnButton;

    [Header("Windows")]
    [SerializeField] private ResultPanelWindow resultWindow;
    [SerializeField] private float fadeTime = 0.1f;

    private void Start()
    {
        returnButton.OnClick.AddListener(ReturnToResults);
    }

    private void ReturnToResults()
    {
        Enable(false, fadeTime);
        resultWindow.Enable(true, fadeTime);
    }

    /// <summary>
    /// Подготовить таблицу с подробными результатами
    /// </summary>
    /// <param name="results"></param>
    public void SetupDetailResult(List<DetailedResult> results)
    {
        foreach (DetailedResult result in results)
        {
            GameObject rowObject = Instantiate(rowPrefab, rowsContainer);
            DetailedResultTableRow row = rowObject.GetComponent<DetailedResultTableRow>();

            row.SetupRow(result);
        }
    }
}
