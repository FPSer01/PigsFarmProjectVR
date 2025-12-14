using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Триггер переключения гайда при взаимодействии с XRGrabInteractable
/// </summary>
public class XRGrabTriggerGuideSwitch : TriggerGuideSwitch
{
    [SerializeField] private XRGrabInteractable interactable;

    private void Start()
    {
        interactable.selectEntered.AddListener(OnGrabbed);
    }

    private void OnGrabbed(SelectEnterEventArgs arg0)
    {
        GuideSwitch();
    }
}
