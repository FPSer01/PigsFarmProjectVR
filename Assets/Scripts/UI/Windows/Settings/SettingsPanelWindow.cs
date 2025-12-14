using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using static MainPanelWindow;

/// <summary>
/// Класс для панели настроек
/// </summary>
public class SettingsPanelWindow : SettingsWindow
{
    [Header("Parent")]
    [SerializeField] private MainPanelWindow mainPanel;

    [Header("Buttons")]
    [SerializeField] private ModdedButton returnButton;

    protected override void Start()
    {
        base.Start();

        returnButton.OnClick.AddListener(SummonMenuWindow);
    }

    private void SummonMenuWindow()
    {
        mainPanel.SetWindow(Windows.Menu);
    }
}
